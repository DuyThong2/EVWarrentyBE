using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.CreateClaim
{
    using FluentValidation;
    using global::WarrantyClaim.Domain.Enums;

    public class CreateClaimsCommandValidator : AbstractValidator<CreateClaimsCommand>
    {
        public CreateClaimsCommandValidator()
        {
            RuleFor(x => x.Claim).NotNull();

            RuleFor(x => x.Claim.VIN)
                .NotEmpty().WithMessage("VIN is required")
                .MaximumLength(64);

            RuleFor(x => x.Claim.DistanceMeter)
                .GreaterThanOrEqualTo(0).When(x => x.Claim.DistanceMeter.HasValue);

            RuleFor(x => x.Claim.TotalPrice)
                .GreaterThanOrEqualTo(0).When(x => x.Claim.TotalPrice.HasValue);

            // Validate từng item nếu có
            When(x => x.Claim.Items != null, () =>
            {
                RuleForEach(x => x.Claim.Items!)
                    .SetValidator(new CreateClaimItemDtoValidator());
            });
        }
    }

    public class CreateClaimItemDtoValidator : AbstractValidator<CreateClaimItemDto>
    {
        public CreateClaimItemDtoValidator()
        {
            RuleFor(x => x.PartSerialNumber)
                .MaximumLength(128);

            RuleFor(x => x.PayAmount)
                .GreaterThanOrEqualTo(0).When(x => x.PayAmount.HasValue);

            RuleFor(x => x.PaidBy)
                .MaximumLength(64);

            RuleFor(x => x.Note)
                .MaximumLength(1024);

            RuleFor(x => x.ImgURLs)
                .MaximumLength(4000);

            // Vì Status trong CreateClaimItemDto là string => cần check parse được sang enum
            RuleFor(x => x.Status)
                .Must(s => string.IsNullOrWhiteSpace(s) || Enum.TryParse<ClaimItemStatus>(s, true, out _))
                .WithMessage("Status is invalid");
        }
    }
 }
