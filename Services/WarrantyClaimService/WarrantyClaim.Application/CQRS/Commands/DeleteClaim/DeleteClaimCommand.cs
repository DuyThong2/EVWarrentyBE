using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.DeleteClaim
{
    public record DeleteClaimCommand(Guid ClaimId) : ICommand<DeleteClaimResult>;

    public record DeleteClaimResult(bool IsDeleted);

    public class DeleteClaimCommandValidator : AbstractValidator<DeleteClaimCommand>
    {
        public DeleteClaimCommandValidator()
        {
            RuleFor(x => x.ClaimId)
                .NotEmpty().WithMessage("ClaimId is required");
        }
    }
}
