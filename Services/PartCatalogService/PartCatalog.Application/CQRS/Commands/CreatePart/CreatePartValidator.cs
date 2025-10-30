using FluentValidation;

namespace PartCatalog.Application.CQRS.Commands.CreatePart
{
    public class CreatePartValidator : AbstractValidator<CreatePartCommand>
    {
        public CreatePartValidator()
        {
            RuleFor(x => x.Part).NotNull().WithMessage("Part data is required");

            RuleFor(x => x.Part.Name)
                .NotEmpty().WithMessage("Part name is required")
                .MaximumLength(160).WithMessage("Part name cannot exceed 160 characters");

            RuleFor(x => x.Part.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.Part.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");



        }
    }
}
