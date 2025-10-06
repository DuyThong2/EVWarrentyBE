using BuildingBlocks.CQRS;
using FluentValidation;
using PartCatalog.Application.DTOs;

namespace PartCatalog.Application.CQRS.Commands.UpdateWarrantyPolicy
{
    public record UpdateWarrantyPolicyCommand(WarrantyPolicyDto Policy)
        : ICommand<UpdateWarrantyPolicyResult>;

    public record UpdateWarrantyPolicyResult(bool IsSuccess, string Message);

    public class UpdateWarrantyPolicyCommandValidator : AbstractValidator<UpdateWarrantyPolicyCommand>
    {
        public UpdateWarrantyPolicyCommandValidator()
        {
            RuleFor(x => x.Policy.PolicyId)
                .NotEmpty().WithMessage("PolicyId is required");

            RuleFor(x => x.Policy.Name)
                .NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Policy.Code)
                .NotEmpty().WithMessage("Code is required");
        }
    }
}
