using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.UpdateClaim
{
    public record UpdateClaimCommand(
    UpdateClaimDto Claim,                  // đổi sang ClaimDto để có Id
    bool ReplaceAllItems = false
) : ICommand<UpdateClaimResult>;

    public record UpdateClaimResult(bool IsUpdated);

    public class UpdateClaimCommandValidator : AbstractValidator<UpdateClaimCommand>
    {
        public UpdateClaimCommandValidator()
        {
            RuleFor(x => x.Claim).NotNull();

            RuleFor(x => x.Claim.Id)
                .NotEmpty().WithMessage("Claim Id is required");

            RuleFor(x => x.Claim.VIN)
                .NotEmpty().WithMessage("VIN is required")
                .MaximumLength(64);

            RuleFor(x => x.Claim.TotalPrice)
                .GreaterThanOrEqualTo(0).When(x => x.Claim.TotalPrice.HasValue);

            // Validate Items nếu có
            When(x => x.Claim.Items != null, () =>
            {
                RuleForEach(x => x.Claim.Items!)
                    .SetValidator(new ClaimItemDtoValidator());
            });
        }
    }

    public class ClaimItemDtoValidator : AbstractValidator<UpdateClaimItemDto>
    {
        public ClaimItemDtoValidator()
        {
            RuleFor(x => x.PartSerialNumber).MaximumLength(128);

            RuleFor(x => x.PayAmount)
                .GreaterThanOrEqualTo(0).When(x => x.PayAmount.HasValue);

            RuleFor(x => x.PaidBy).MaximumLength(64);
            RuleFor(x => x.Note).MaximumLength(1024);
            RuleFor(x => x.ImgURLs).MaximumLength(4000);

            // Nếu Status là string thì cần check parse được sang enum
            RuleFor(x => x.Status)
                .Must(s => string.IsNullOrWhiteSpace(s) || Enum.TryParse<ClaimItemStatus>(s, true, out _))
                .WithMessage("Invalid ClaimItem status value");
        }
    }
}
