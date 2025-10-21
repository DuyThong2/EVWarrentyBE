using BuildingBlocks.Storage.Bucket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Application.Extension;

namespace WarrantyClaim.Application.CQRS.Commands.UpdateClaimItemImages
{
    public sealed class UpdateClaimItemFilesCommandHandler
        : ICommandHandler<UpdateClaimItemFilesCommand, ReconcileResponseDto>
    {
        private readonly IS3Storage _storage;
        private readonly IApplicationDbContext _db;

        public UpdateClaimItemFilesCommandHandler(IS3Storage storage, IApplicationDbContext db)
        {
            _storage = storage;
            _db = db;
        }

        public async Task<ReconcileResponseDto> Handle(UpdateClaimItemFilesCommand request, CancellationToken ct)
        {
            var item = await _db.ClaimItems.FirstOrDefaultAsync(x => x.Id == request.ClaimItemId, ct);
            if (item is null)
                throw new KeyNotFoundException($"ClaimItem {request.ClaimItemId} not found.");

            // 1) giữ lại từ KeepJson
            var keep = FileRefUtils.ParseAny(request.KeepJson);

            var uploaded = (request.Files is { Count: > 0 })
                ? await _storage.UploadAsync(request.Files!, ct)
                : Array.Empty<UploadedFileDto>();

            List<FileRefDto> desired;
            if (uploaded.Count == 0)
            {
                desired = FileRefUtils.Distinct(keep);
            }
            else
            {
                var newRefs = FileRefUtils.FromUploaded(uploaded); // đã chứa Name/Size/ContentType/UploadedAtUtc nếu bạn cập nhật như hướng dẫn
                desired = FileRefUtils.Distinct(keep.Concat(newRefs));
            }

            if ((keep == null || keep.Count == 0) && uploaded.Count == 0)
            {
                item.ImgURLs = null;
            }
            else
            {
                desired = desired
                    .Where(d =>
                        !string.IsNullOrWhiteSpace(d.Key) ||
                        (!string.IsNullOrWhiteSpace(d.Url) && d.Url != "[]"))
                    .ToList();

                item.ImgURLs = desired.Count == 0 ? null : FileRefUtils.ToJson(desired);
            }

            await _db.SaveChangesAsync(ct);

            return new ReconcileResponseDto
            {
                ClaimId = item.ClaimId, // hoặc thêm ClaimItemId vào DTO nếu cần
                Desired = desired,
                UploadedFiles = uploaded.ToList(),
                Meta = request.Meta
            };
        }
    }
}
