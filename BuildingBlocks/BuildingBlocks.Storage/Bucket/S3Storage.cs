using Amazon.S3;
using Amazon.S3.Model;
using BuildingBlocks.Storage.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Storage.Bucket
{
    public sealed class S3Storage : IS3Storage
    {
        private readonly IAmazonS3 _s3;
        private readonly AwsOptions _opt;

        public S3Storage(IAmazonS3 s3, IOptions<AwsOptions> opt)
        {
            _s3 = s3;
            _opt = opt.Value;
        }

        public async Task<IReadOnlyList<UploadedFileDto>> UploadAsync(
            IEnumerable<IFormFile> files, CancellationToken ct = default)
        {
            var results = new List<UploadedFileDto>();

            foreach (var file in files)
            {
                if (file.Length == 0) continue;

                // tạo key an toàn: prefix/yyyy/MM/dd/{guid}{ext}
                var ext = Path.GetExtension(file.FileName);
                var key = S3KeyBuilder.BuildKey(file, _opt.KeyPrefix); 


                // upload
                using var stream = file.OpenReadStream();
                var put = new PutObjectRequest
                {
                    BucketName = _opt.Bucket,
                    Key = key,
                    InputStream = stream,
                    ContentType = file.ContentType,
                    //ACL = S3CannedACL.PublicRead, // nếu muốn public (cân nhắc bảo mật)
                    Metadata =
                {
                    ["x-amz-meta-original-name"] = file.FileName
                }
                };

                var resp = await _s3.PutObjectAsync(put, ct);

                // tạo presigned URL (tạm thời) để client truy cập xem thử
                var url = _s3.GetPreSignedURL(new GetPreSignedUrlRequest
                {
                    BucketName = _opt.Bucket,
                    Key = key,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

                results.Add(new UploadedFileDto
                {
                    Key = key,
                    ContentType = file.ContentType,
                    Size = file.Length,
                    PreviewUrl = url
                });
            }

            return results;
        }

        public async Task<FileStreamResult> DownloadAsync(string key, CancellationToken ct = default)
        {
            var resp = await _s3.GetObjectAsync(_opt.Bucket, key, ct);
            return new FileStreamResult(resp.ResponseStream, resp.Headers.ContentType)
            {
                FileDownloadName = Path.GetFileName(key)
            };
        }
    }



    public sealed class UploadedFileDto
    {
        public string Key { get; set; } = default!;
        public string ContentType { get; set; } = default!;
        public long Size { get; set; }
        public string PreviewUrl { get; set; } = default!;
    }
}
