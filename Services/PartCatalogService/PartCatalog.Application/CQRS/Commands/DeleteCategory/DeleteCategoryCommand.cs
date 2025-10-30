using BuildingBlocks.CQRS;
using FluentValidation;

namespace PartCatalog.Application.CQRS.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(Guid CateId)
        : ICommand<DeleteCategoryResult>;

    public record DeleteCategoryResult(bool IsSuccess, string Message);

    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.CateId)
                .NotEmpty().WithMessage("Category ID is required.");
        }
    }
}
