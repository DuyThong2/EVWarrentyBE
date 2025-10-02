using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.CreateTechician
{
    public record CreateTechnicianCommand(
        TechnicianDto Technician
    ) : ICommand<CreateTechnicianResult>;

    public record CreateTechnicianResult(Guid TechnicianId);

    public class CreateTechnicianCommandValidator : AbstractValidator<CreateTechnicianCommand>
    {
        public CreateTechnicianCommandValidator()
        {
            RuleFor(x => x.Technician).NotNull();

            RuleFor(x => x.Technician.FullName)
                .NotEmpty().WithMessage("FullName is required")
                .MaximumLength(128);

            RuleFor(x => x.Technician.Email)
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Technician.Email))
                .MaximumLength(256);

            RuleFor(x => x.Technician.Phone)
                .MaximumLength(32);

            // Status là string → phải parse được sang enum (ví dụ TechnicianStatus)
            RuleFor(x => x.Technician.Status)
                .Must(s => !string.IsNullOrWhiteSpace(s))
                .WithMessage("Invalid Technician status value");
        }
    }
}
