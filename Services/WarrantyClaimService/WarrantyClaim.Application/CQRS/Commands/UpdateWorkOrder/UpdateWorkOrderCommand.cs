using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.UpdateWorkOrder
{
    public record UpdateWorkOrderCommand(
        WorkOrderDto WorkOrder
    ) : ICommand<UpdateWorkOrderResult>;

    public record UpdateWorkOrderResult(bool IsUpdated);

    public class UpdateWorkOrderCommandValidator : AbstractValidator<UpdateWorkOrderCommand>
    {
        public UpdateWorkOrderCommandValidator()
        {
            RuleFor(x => x.WorkOrder).NotNull();

            RuleFor(x => x.WorkOrder.Id)
                .NotEmpty().WithMessage("WorkOrder Id is required");

            RuleFor(x => x.WorkOrder.ClaimItemId)
                .NotEmpty().WithMessage("ClaimItemId is required");

            RuleFor(x => x.WorkOrder.TechnicianId)
                .NotEmpty().WithMessage("TechnicianId is required");

            RuleFor(x => x.WorkOrder.WorkingHours)
                .GreaterThanOrEqualTo(0).When(x => x.WorkOrder.WorkingHours.HasValue);

            // Status là string: cho phép null/rỗng; nếu có thì phải parse được
            RuleFor(x => x.WorkOrder.Status)
                .Must(s => string.IsNullOrWhiteSpace(s) || Enum.TryParse<WorkOrderStatus>(s, true, out _))
                .WithMessage("Invalid WorkOrder status");

            // Nếu có cả StartedAt & EndDate thì EndDate >= StartedAt
            When(x => x.WorkOrder.StartedAt.HasValue && x.WorkOrder.EndDate.HasValue, () =>
            {
                RuleFor(x => x.WorkOrder)
                    .Must(w => w.EndDate!.Value >= w.StartedAt!.Value)
                    .WithMessage("EndDate must be greater than or equal to StartedAt");
            });
        }
    }
}
