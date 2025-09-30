using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.UpdateSupplyPart
{
    public record UpdateSupplyPartCommand(
        PartSupplyDto SupplyPart
    ) : ICommand<UpdateSupplyPartResult>;

    public record UpdateSupplyPartResult(bool IsUpdated);

    public class UpdateSupplyPartCommandValidator : AbstractValidator<UpdateSupplyPartCommand>
    {
        public UpdateSupplyPartCommandValidator()
        {
            RuleFor(x => x.SupplyPart).NotNull();

            RuleFor(x => x.SupplyPart.Id)
                .NotEmpty().WithMessage("PartSupply Id is required");

            RuleFor(x => x.SupplyPart.ClaimItemId)
                .NotEmpty().WithMessage("ClaimItemId is required");

            RuleFor(x => x.SupplyPart.Description).MaximumLength(1024);
            RuleFor(x => x.SupplyPart.NewSerialNumber).MaximumLength(128);
            RuleFor(x => x.SupplyPart.ShipmentCode).MaximumLength(128);
            RuleFor(x => x.SupplyPart.ShipmentRef).MaximumLength(128);

            RuleFor(x => x.SupplyPart.Status)
                .Must(s => string.IsNullOrWhiteSpace(s) || Enum.TryParse<SupplyStatus>(s, true, out _))
                .WithMessage("Status is invalid");
        }
    }

}
