using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.DeleteTechnican
{
    public record DeleteTechnicianCommand(Guid TechnicianId) : ICommand<DeleteTechnicianResult>;

    public record DeleteTechnicianResult(bool IsDeleted);

    public class DeleteTechnicianCommandValidator : AbstractValidator<DeleteTechnicianCommand>
    {
        public DeleteTechnicianCommandValidator()
        {
            RuleFor(x => x.TechnicianId)
                .NotEmpty().WithMessage("TechnicianId is required");
        }
    }
}
