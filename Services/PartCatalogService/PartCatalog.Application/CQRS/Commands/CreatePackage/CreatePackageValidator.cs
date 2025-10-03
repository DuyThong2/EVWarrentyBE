using FluentValidation;
using PartCatalog.Application.Commands.CreatePackage;

namespace PartCatalog.Application.Features.Packages.Commands.CreatePackage
{
    public class CreatePackageCommandValidator : AbstractValidator<CreatePackageCommand>
    {
        public CreatePackageCommandValidator()
        {
            RuleFor(x => x.Package).NotNull();

            RuleFor(x => x.Package.PackageCode)
                .NotEmpty().WithMessage("PackageCode is required.")
                .MaximumLength(50);

            RuleFor(x => x.Package.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Package.Model)
                .MaximumLength(100);

            RuleFor(x => x.Package.Note)
                .MaximumLength(1024);

            RuleFor(x => x.Package.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
