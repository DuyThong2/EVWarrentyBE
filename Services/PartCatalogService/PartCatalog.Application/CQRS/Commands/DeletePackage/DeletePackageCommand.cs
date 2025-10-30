using BuildingBlocks.CQRS;
using FluentValidation;

namespace PartCatalog.Application.CQRS.Commands.DeletePackage
{
    public record DeletePackageCommand(Guid PackageId)
        : ICommand<DeletePackageResult>;

    public record DeletePackageResult(bool IsSuccess);

    public class DeletePackageCommandValidator : AbstractValidator<DeletePackageCommand>
    {
        public DeletePackageCommandValidator()
        {
            RuleFor(x => x.PackageId)
                .NotEmpty().WithMessage("PackageId is required");
        }
    }
}
