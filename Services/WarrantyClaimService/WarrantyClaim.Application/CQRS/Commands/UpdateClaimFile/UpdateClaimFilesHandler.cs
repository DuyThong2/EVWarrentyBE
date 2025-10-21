using BuildingBlocks.CQRS;
using BuildingBlocks.Storage.Bucket;
using Microsoft.EntityFrameworkCore;
using WarrantyClaim.Application.CQRS.Commands.UpdateClaimFile;
using WarrantyClaim.Application.Data;          // IApplicationDbContext
using WarrantyClaim.Application.Dtos;
using WarrantyClaim.Application.Extension;    // FileRefUtils

namespace WarrantyClaim.Application.Claims.Commands.UpdateClaimFiles
{
    public sealed class UpdateClaimFilesCommandHandler
        : ICommandHandler<UpdateClaimFilesCommand, ReconcileResponseDto>
    {
        private readonly IS3Storage _storage;
        private readonly IApplicationDbContext _db;

        public UpdateClaimFilesCommandHandler(IS3Storage storage, IApplicationDbContext db)
        {
            _storage = storage;
            _db = db;
        }

        public async Task<ReconcileResponseDto> Handle(UpdateClaimFilesCommand request, CancellationToken ct)
        {
            var claim = await _db.Claims.FirstOrDefaultAsync(x => x.Id == request.ClaimId, ct);
            if (claim is null)
                throw new KeyNotFoundException($"Claim {request.ClaimId} not found.");

            // 1️⃣ Giữ lại file cũ
            var keep = FileRefUtils.ParseAny(request.KeepJson);

            // 2️⃣ Upload file mới (nếu có)
            var uploaded = (request.Files is { Count: > 0 })
                ? await _storage.UploadAsync(request.Files!, ct)
                : Array.Empty<UploadedFileDto>();

            // 3️⃣ Nếu không có file mới thì chỉ giữ lại danh sách cũ
            List<FileRefDto> desired;
            if (uploaded.Count == 0)
            {
                desired = FileRefUtils.Distinct(keep);
            }
            else
            {
                var newRefs = FileRefUtils.FromUploaded(uploaded);
                desired = FileRefUtils.Distinct(keep.Concat(newRefs));
            }

            if ((keep == null || keep.Count == 0) && uploaded.Count == 0)
            {
                claim.FileURL = null;
            }
            else
            {
                desired = desired
                    .Where(d =>
                        !string.IsNullOrWhiteSpace(d.Key) ||
                        (!string.IsNullOrWhiteSpace(d.Url) && d.Url != "[]"))
                    .ToList();

                claim.FileURL = desired.Count == 0 ? null : FileRefUtils.ToJson(desired);
            }

            await _db.SaveChangesAsync(ct);

            // 5️⃣ Response
            return new ReconcileResponseDto
            {
                ClaimId = request.ClaimId,
                Desired = desired,
                UploadedFiles = uploaded.ToList(),
                Meta = request.Meta
            };
        }
    }
}
