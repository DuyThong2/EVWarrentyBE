using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.CreateWorkOrder
{
    public record CreateWorkOrderCommand(
        CreateWorkOrderDto WorkOrder
    ) : ICommand<CreateWorkOrderResult>;

    public record CreateWorkOrderResult(Guid WorkOrderId);

    public class CreateWorkOrderCommandValidator : AbstractValidator<CreateWorkOrderCommand>
    {
        public CreateWorkOrderCommandValidator()
        {
            RuleFor(x => x.WorkOrder).NotNull();

            RuleFor(x => x.WorkOrder.ClaimItemId)
                .NotEmpty().WithMessage("ClaimItemId is required");

            RuleFor(x => x.WorkOrder.TechnicianId)
                .NotEmpty().WithMessage("TechnicianId is required");

            RuleFor(x => x.WorkOrder.WorkingHours)
                .GreaterThanOrEqualTo(0).When(x => x.WorkOrder.WorkingHours.HasValue);

            // Status là string -> cho phép null/rỗng; nếu có phải parse được sang enum
            RuleFor(x => x.WorkOrder.Status)
                .Must(s => string.IsNullOrWhiteSpace(s) || Enum.TryParse<WorkOrderStatus>(s, true, out _))
                .WithMessage("Invalid WorkOrder status");

            // Nếu cả StartedAt và EndDate có giá trị thì EndDate >= StartedAt
            When(x => x.WorkOrder.StartedAt.HasValue && x.WorkOrder.EndDate.HasValue, () =>
            {
                RuleFor(x => x.WorkOrder)
                    .Must(w => w.EndDate!.Value >= w.StartedAt!.Value)
                    .WithMessage("EndDate must be greater than or equal to StartedAt");
            });
        }
    }
}
