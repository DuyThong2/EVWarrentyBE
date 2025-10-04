using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Domain.Enums;

namespace WarrantyClaim.Application.CQRS.Commands.CreateClaim
{
    public record CreateClaimsCommand(
            CreateClaimDto Claim
        ) : ICommand<CreateClaimsResult>;

    public record CreateClaimsResult(Guid ClaimId);


}
