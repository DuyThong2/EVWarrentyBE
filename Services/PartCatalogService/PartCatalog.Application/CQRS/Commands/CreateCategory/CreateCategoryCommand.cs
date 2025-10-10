using BuildingBlocks.CQRS;
using FluentValidation;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Commands.CreateCategory
{
    public record CreateCategoryCommand(CategoryDto Category)
        : ICommand<CreateCategoryResult>;

    public record CreateCategoryResult(Guid CateId, bool IsSuccess, string Message);

    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Category.CateCode)
                .NotEmpty().WithMessage("Category code is required.");

            RuleFor(x => x.Category.CateName)
                .NotEmpty().WithMessage("Category name is required.");
        }
    }
}
