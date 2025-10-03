using FluentValidation;
using MediatR;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Applications.UpdatePackage
{
    public record UpdatePackageCommand(UpdatePackageDto Package) : IRequest<UpdatePackageResult>;

    public record UpdatePackageResult(bool Success, string? Message);

    // Validator gộp chung vào đây
    public class UpdatePackageCommandValidator : AbstractValidator<UpdatePackageCommand>
    {
        public UpdatePackageCommandValidator()
        {
            RuleFor(x => x.Package).NotNull();

            RuleFor(x => x.Package.PackageId)
                .NotEmpty().WithMessage("PackageId is required");

            RuleFor(x => x.Package.PackageCode)
                .NotEmpty().WithMessage("PackageCode is required.")
                .MaximumLength(50);

            RuleFor(x => x.Package.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Package.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
