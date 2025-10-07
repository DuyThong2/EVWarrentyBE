using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarrantyClaim.Application.CQRS.Commands.UpdateTechnican
{
    public record UpdateTechnicianCommand(
        TechnicianDto Technician
    ) : ICommand<UpdateTechnicianResult>;

    public record UpdateTechnicianResult(bool IsUpdated);

    public class UpdateTechnicianCommandValidator : AbstractValidator<UpdateTechnicianCommand>
    {
        public UpdateTechnicianCommandValidator()
        {
            RuleFor(x => x.Technician).NotNull();

            RuleFor(x => x.Technician.Id)
                .NotEmpty().WithMessage("Technician Id is required");

            RuleFor(x => x.Technician.FullName)
                .NotEmpty().WithMessage("FullName is required")
                .MaximumLength(128);

            RuleFor(x => x.Technician.Email)
                .MaximumLength(256)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Technician.Email));

            RuleFor(x => x.Technician.Phone)
                .MaximumLength(32);

            // Status (string) phải parse được sang enum nếu được cung cấp
            RuleFor(x => x.Technician.Status)
                .Must(s => !string.IsNullOrWhiteSpace(s))
                .WithMessage("Invalid Technician status value");
        }
    }
}
