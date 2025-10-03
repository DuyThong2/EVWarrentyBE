using BuildingBlocks.CQRS;
using FluentValidation;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Commands.UpdatePart
{
    // Command nhận dữ liệu từ client (bao gồm Id để update)
    public record UpdatePartCommand(Guid PartId, PartDto Part)
        : ICommand<UpdatePartResult>;

    // Kết quả trả về
    public record UpdatePartResult(bool IsSuccess);

    // Validator
    public class UpdatePartCommandValidator : AbstractValidator<UpdatePartCommand>
    {
        public UpdatePartCommandValidator()
        {
            RuleFor(x => x.Part).NotNull();

            RuleFor(x => x.Part.Id)
                .NotEmpty().WithMessage("PartId is required");

            RuleFor(x => x.Part.Name)
                .NotEmpty().WithMessage("Part Name is required")
                .MaximumLength(128);

            RuleFor(x => x.Part.Description)
                .MaximumLength(512);
        }
    }
}
