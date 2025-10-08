// API/Controllers/ClaimUploadsController.cs
using BuildingBlocks.Storage.Bucket;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarrantyClaim.Application.CQRS.Commands.UpdateClaimFile;
using WarrantyClaim.Application.Data;
using WarrantyClaim.Application.Dtos;
using WarrantyClaim.Application.Extension;         // FileRefUtils

[ApiController]
[Route("api/[controller]")]
public sealed class ClaimUploadsController : ControllerBase
{
    private readonly IS3Storage _storage;
    private readonly ISender _mediator;
    public ClaimUploadsController(IS3Storage storage, ISender mediator)
    {
        _storage = storage;
        _mediator = mediator;
    }

    [HttpPost("{claimId:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(100 * 1024 * 1024)]
    [ProducesResponseType(typeof(ReconcileResponseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Reconcile(
        [FromRoute] Guid claimId,
        [FromForm] UploadRequest req,
        CancellationToken ct)
    {
        var resp = await _mediator.Send(
            new UpdateClaimFilesCommand(claimId, req.KeepJson, req.Files, req.Meta), ct);

        // Log nhẹ để bạn thấy response khi FE onSave
        Console.WriteLine($"[Reconcile] claim={claimId} uploaded={resp.UploadedFiles.Count} desired={resp.Desired.Count}");

        return Created(string.Empty, resp);
    }

    [HttpPost("{claimItemId:guid}")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(100 * 1024 * 1024)]
    [ProducesResponseType(typeof(ReconcileResponseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> ReconcileItem(
        [FromRoute] Guid claimItemId,
        [FromForm] UploadRequest req,
        CancellationToken ct)
    {
        var resp = await _mediator.Send(
            new UpdateClaimItemFilesCommand(claimItemId, req.KeepJson, req.Files, req.Meta), ct);

        Console.WriteLine($"[ClaimItem Reconcile] item={claimItemId} uploaded={resp.UploadedFiles.Count} desired={resp.Desired.Count}");

        return Created(string.Empty, resp);
    }

    [HttpGet("download/{**key}")]
    public async Task<IActionResult> Download([FromRoute] string key, [FromQuery] string? filename)
    {
        var file = await _storage.DownloadAsync(key);
        if (!string.IsNullOrWhiteSpace(filename))
            file.FileDownloadName = filename;
        return file; 
    }
}

public sealed class UploadRequest
{
    public List<IFormFile>? Files { get; set; }
    public string? KeepJson { get; set; }
    public string? Meta { get; set; }
}
