using BuildingBlocks.CQRS;
using FluentValidation;

namespace PartCatalog.Application.CQRS.Commands.DeletePart
{
    // Command chỉ cần Id để xoá
    public record DeletePartCommand(Guid PartId)
        : ICommand<DeletePartResult>;

    // Kết quả trả về
    public record DeletePartResult(bool IsSuccess);

    // Validator
    public class DeletePartCommandValidator : AbstractValidator<DeletePartCommand>
    {
        public DeletePartCommandValidator()
        {
            RuleFor(x => x.PartId)
                .NotEmpty().WithMessage("PartId is required");
        }
    }
}
