using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.UpdateClaimItemImages
{
    public sealed record UpdateClaimItemFilesCommand(
        Guid ClaimItemId,
        string? KeepJson,
        List<IFormFile>? Files,
        string? Meta
    ) : ICommand<ReconcileResponseDto>;
}
