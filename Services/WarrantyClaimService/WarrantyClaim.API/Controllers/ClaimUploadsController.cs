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
    private readonly ISender _mediator;
    public ClaimUploadsController(ISender mediator)
    {
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
}

public sealed class UploadRequest
{
    public List<IFormFile>? Files { get; set; }
    public string? KeepJson { get; set; }
    public string? Meta { get; set; }
}
