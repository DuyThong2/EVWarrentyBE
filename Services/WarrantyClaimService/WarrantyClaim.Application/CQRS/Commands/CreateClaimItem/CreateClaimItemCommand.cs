using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.CreateClaimItem
{
    public record CreateClaimItemCommand(
        Guid ClaimId,                
        CreateClaimItemDto Item
    ) : ICommand<CreateClaimItemResult>;

    public record CreateClaimItemResult(Guid ClaimItemId);

    public class CreateClaimItemCommandValidator : AbstractValidator<CreateClaimItemCommand>
    {
        public CreateClaimItemCommandValidator()
        {
            RuleFor(x => x.ClaimId)
                .NotEmpty().WithMessage("ClaimId is required");

            RuleFor(x => x.Item).NotNull();

            RuleFor(x => x.Item.PartSerialNumber).MaximumLength(128);

            RuleFor(x => x.Item.PayAmount)
                .GreaterThanOrEqualTo(0).When(x => x.Item.PayAmount.HasValue);

            RuleFor(x => x.Item.PaidBy).MaximumLength(64);
            RuleFor(x => x.Item.Note).MaximumLength(1024);
            RuleFor(x => x.Item.ImgURLs).MaximumLength(4000);

            // Status là string → phải parse được sang enum
            RuleFor(x => x.Item.Status)
                .Must(s => string.IsNullOrWhiteSpace(s) || Enum.TryParse<ClaimItemStatus>(s, true, out _))
                .WithMessage("Invalid ClaimItem status value");
        }
    }
}
