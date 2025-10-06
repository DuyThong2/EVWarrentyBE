using BuildingBlocks.CQRS;
using FluentValidation;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Commands.CreateWarrantyPolicy
{
    // Command
    public record CreateWarrantyPolicyCommand(CreateWarrantyPolicyDto Policy)
        : ICommand<CreateWarrantyPolicyResult>;

    // Result
    public record CreateWarrantyPolicyResult(Guid PolicyId, bool IsSuccess, string Message);

    // Validator
    public class CreateWarrantyPolicyCommandValidator : AbstractValidator<CreateWarrantyPolicyCommand>
    {
        public CreateWarrantyPolicyCommandValidator()
        {
            RuleFor(x => x.Policy.PackageId)
                .NotEmpty().WithMessage("PackageId is required");

            RuleFor(x => x.Policy.Code)
                .NotEmpty().WithMessage("Code is required")
                .MaximumLength(50);

            RuleFor(x => x.Policy.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100);
        }
    }
}
