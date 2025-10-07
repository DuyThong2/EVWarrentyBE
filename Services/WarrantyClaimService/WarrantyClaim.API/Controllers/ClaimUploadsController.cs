using BuildingBlocks.Storage.Bucket;
using Microsoft.AspNetCore.Mvc;

namespace WarrantyClaim.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class ClaimUploadsController : ControllerBase
    {
        private readonly IS3Storage _storage;

        public ClaimUploadsController(IS3Storage storage) => _storage = storage;

        // Giới hạn cỡ request tùy nhu cầu (vd 50MB)
        [HttpPost]
        [RequestSizeLimit(50 * 1024 * 1024)]
        public async Task<IActionResult> Upload([FromForm] UploadRequest req, CancellationToken ct)
        {
            if (req.Files == null || req.Files.Count == 0)
                return BadRequest("No files provided");

            // if (req.Files.Any(f => !f.ContentType.StartsWith("image/"))) ...

            var result = await _storage.UploadAsync(req.Files, ct);
            return Created("", result); 
        }

        [HttpGet("{*key}")]
        public async Task<IActionResult> Download([FromRoute] string key, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(key))
                return BadRequest("File key is required");

            var fileResult = await _storage.DownloadAsync(key, ct);
            return fileResult;
        }
    }

    public sealed class UploadRequest
    {
        public List<IFormFile> Files { get; set; } = new();
        public string? Meta { get; set; }
    }
}
