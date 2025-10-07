// WarrantyClaim.Application/Dtos/FileRefDto.cs
namespace WarrantyClaim.Application.Dtos
{
    public sealed class FileRefDto
    {
        public string? Key { get; set; }   // ví dụ: uploads/images/abc.jpg
        public string? Url { get; set; }   // permanent url (CDN hoặc /api/download/{key})
    }
}
