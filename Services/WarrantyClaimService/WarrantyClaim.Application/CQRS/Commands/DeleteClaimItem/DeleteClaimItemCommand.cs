using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.DeleteClaimItem
{
    public record DeleteClaimItemCommand(Guid ClaimItemId) : ICommand<DeleteClaimItemResult>;

    public record DeleteClaimItemResult(bool IsDeleted);

    public class DeleteClaimItemCommandValidator : AbstractValidator<DeleteClaimItemCommand>
    {
        public DeleteClaimItemCommandValidator()
        {
            RuleFor(x => x.ClaimItemId)
                .NotEmpty().WithMessage("ClaimItemId is required");
        }
    }
}
