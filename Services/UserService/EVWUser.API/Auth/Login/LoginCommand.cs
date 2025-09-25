using BuildingBlocks.CQRS;
using EVWUser.API.Dtos;
using FluentValidation;

namespace EVWUser.API.Auth.Login
{
    public record LoginCommand(LoginRequest LoginRequest) : ICommand<LoginResult>;
    public record LoginResult(LoginResponse LoginResponse);

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.LoginRequest.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Email should be in a proper email address format");

            RuleFor(x => x.LoginRequest.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
