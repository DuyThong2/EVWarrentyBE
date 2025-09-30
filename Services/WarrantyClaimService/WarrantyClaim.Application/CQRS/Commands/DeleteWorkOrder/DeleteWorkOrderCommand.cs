using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.DeleteWorkOrder
{
    public record DeleteWorkOrderCommand(Guid WorkOrderId) : ICommand<DeleteWorkOrderResult>;

    public record DeleteWorkOrderResult(bool IsDeleted);

    public class DeleteWorkOrderCommandValidator : AbstractValidator<DeleteWorkOrderCommand>
    {
        public DeleteWorkOrderCommandValidator()
        {
            RuleFor(x => x.WorkOrderId)
                .NotEmpty().WithMessage("WorkOrderId is required");
        }
    }
}
