using BuildingBlocks.CQRS;
using FluentValidation;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Commands.UpdateCategory
{
    public record UpdateCategoryCommand(CategoryDto Category)
        : ICommand<UpdateCategoryResult>;

    public record UpdateCategoryResult(bool IsSuccess, string Message);

    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Category.CateId)
                .NotEmpty().WithMessage("Category ID is required.");

            RuleFor(x => x.Category.CateCode)
                .NotEmpty().WithMessage("Category code is required.");

            RuleFor(x => x.Category.CateName)
                .NotEmpty().WithMessage("Category name is required.");
        }
    }
}
