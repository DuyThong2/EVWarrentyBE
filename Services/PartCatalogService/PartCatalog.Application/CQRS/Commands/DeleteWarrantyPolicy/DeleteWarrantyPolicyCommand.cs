using BuildingBlocks.CQRS;
using FluentValidation;

namespace PartCatalog.Application.CQRS.Commands.DeleteWarrantyPolicy
{
    public record DeleteWarrantyPolicyCommand(Guid PolicyId)
        : ICommand<DeleteWarrantyPolicyResult>;

    public record DeleteWarrantyPolicyResult(bool IsSuccess, string Message);

    public class DeleteWarrantyPolicyCommandValidator : AbstractValidator<DeleteWarrantyPolicyCommand>
    {
        public DeleteWarrantyPolicyCommandValidator()
        {
            RuleFor(x => x.PolicyId)
                .NotEmpty().WithMessage("PolicyId is required");
        }
    }
}
