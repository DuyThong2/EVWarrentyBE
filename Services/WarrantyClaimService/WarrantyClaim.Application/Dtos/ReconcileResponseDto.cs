using BuildingBlocks.Storage.Bucket;

namespace WarrantyClaim.Application.Dtos
{
    public sealed class ReconcileResponseDto
    {
        public Guid ClaimId { get; set; }
        public List<FileRefDto> Desired { get; set; } = new();
        public List<UploadedFileDto> UploadedFiles { get; set; } = new();
        public string? Meta { get; set; }
    }
}