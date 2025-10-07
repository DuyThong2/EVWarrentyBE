using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.DeleteSupplyPart
{
    public record DeleteSupplyPartCommand(Guid PartSupplyId) : ICommand<DeleteSupplyPartResult>;

    public record DeleteSupplyPartResult(bool IsDeleted);

    public class DeleteSupplyPartCommandValidator : AbstractValidator<DeleteSupplyPartCommand>
    {
        public DeleteSupplyPartCommandValidator()
        {
            RuleFor(x => x.PartSupplyId)
                .NotEmpty().WithMessage("PartSupplyId is required");
        }
    }
}
