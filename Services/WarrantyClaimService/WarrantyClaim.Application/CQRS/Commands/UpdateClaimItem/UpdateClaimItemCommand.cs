using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.UpdateClaimItem
{
    public record UpdateClaimItemCommand(
        UpdateClaimItemDto Item
    ) : ICommand<UpdateClaimItemResult>;

    public record UpdateClaimItemResult(bool IsUpdated);

    public class UpdateClaimItemCommandValidator : AbstractValidator<UpdateClaimItemCommand>
    {
        public UpdateClaimItemCommandValidator()
        {
            RuleFor(x => x.Item).NotNull();

            RuleFor(x => x.Item.Id)
                .NotEmpty().WithMessage("ClaimItem Id is required");

            RuleFor(x => x.Item.PartSerialNumber)
                .MaximumLength(128);

            RuleFor(x => x.Item.PayAmount)
                .GreaterThanOrEqualTo(0).When(x => x.Item.PayAmount.HasValue);

            RuleFor(x => x.Item.PaidBy)
                .MaximumLength(64);

            RuleFor(x => x.Item.Note)
                .MaximumLength(1024);

            RuleFor(x => x.Item.ImgURLs)
                .MaximumLength(4000);

            RuleFor(x => x.Item.ClaimId).NotEmpty().WithMessage("claim Id is required");

            //RuleFor(x => x.Item.Status)
            //    .Must(s => string.IsNullOrWhiteSpace(s) || Enum.TryParse<ClaimItemStatus>(s, true, out _))
            //    .WithMessage("Invalid ClaimItem status value");
        }
    }
}
