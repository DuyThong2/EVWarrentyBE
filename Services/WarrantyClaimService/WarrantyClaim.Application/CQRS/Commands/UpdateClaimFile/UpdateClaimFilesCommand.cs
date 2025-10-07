using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.UpdateClaimFile
{
    public sealed record UpdateClaimFilesCommand(
        Guid ClaimId,
        string? KeepJson,
        ICollection<IFormFile> ? Files,
        string? Meta
    ) : ICommand<ReconcileResponseDto>;
}
