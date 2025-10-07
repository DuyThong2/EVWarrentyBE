using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarrantyClaim.Domain.Enums;

namespace WarrantyClaim.Application.CQRS.Commands.CreateSupplyPart
{
    public record CreateSupplyPartCommand(
                    PartSupplyDto SupplyPart
                ) : ICommand<CreateSupplyPartResult>;

    public record CreateSupplyPartResult(Guid PartSupplyId);

    public class CreateSupplyPartCommandValidator : AbstractValidator<CreateSupplyPartCommand>
    {
        public CreateSupplyPartCommandValidator()
        {
            RuleFor(x => x.SupplyPart).NotNull();

            RuleFor(x => x.SupplyPart.ClaimItemId)
                .NotEmpty().WithMessage("ClaimItemId is required");

            RuleFor(x => x.SupplyPart.Description)
                .MaximumLength(1024);

            RuleFor(x => x.SupplyPart.NewSerialNumber)
                .MaximumLength(128);

            RuleFor(x => x.SupplyPart.ShipmentCode)
                .MaximumLength(128);

            RuleFor(x => x.SupplyPart.ShipmentRef)
                .MaximumLength(128);

            RuleFor(x => x.SupplyPart.Status).NotEmpty();
        }
    }
}
