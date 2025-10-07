using AutoMapper;
using BuildingBlocks.Email.Abstractions;
using BuildingBlocks.Email.Models;
using BuildingBlocks.Email.Templates;
using BuildingBlocks.Exceptions;
using EVWUser.API.Dtos;
using EVWUser.API.Enums;
using EVWUser.API.Extensions.Jwt;
using EVWUser.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;

namespace EVWUser.API.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        //private readonly TemplateRenderer _renderer;

        public AuthService(IUserService userService, IJwtService jwtService, IMapper mapper, IEmailSender emailSender)
        {
            _userService = userService;
            _jwtService = jwtService;
            _mapper = mapper;
            _emailSender = emailSender;
            //_renderer = new();
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                var user = await _userService.GetByEmailAsync(request.Email);

                var isPasswordValid = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password);
                if (isPasswordValid == PasswordVerificationResult.Failed)
                {
                    throw new BadRequestException("Invalid password");
                }

                var token = _jwtService.GenerateJwtToken(user);

//                var renderer = new TemplateRenderer(
//    Path.Combine(AppContext.BaseDirectory, "Templates", "Layout.html")
//);

//                var body = renderer.Render(
//            "Welcome to EV Warranty",
//            $"<h2>Hello {user.Username},</h2><p>Thank you for registering!</p>"
//        );

//                var message = new EmailMessage
//                {
//                    To = new List<string> { "trankimnha272727@gmail.com" },
//                    Subject = "Welcome to EV Warranty",
//                    Body = body,
//                    IsHtml = true
//                };

//                try
//                {
//                    await _emailSender.SendEmailAsync(message);
//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("Failed to send email: " + ex.Message);
//                }

                return new LoginResponse
                {
                    User = await _userService.MapRolesToDto(user),
                    Token = new TokenResponse
                    {
                        AccessToken = token,
                        ExpiresAt = DateTime.Now.AddMinutes(60)
                    }
                };
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception("An error occurred during login. Please try again later.");
            }
        }

        public Task<UserDto> ExtractTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new BadRequestException("Token is required");

            var jwtToken = _jwtService.ReadToken(token);
            if (jwtToken == null)
                throw new BadRequestException("Invalid token.");

            var userId = jwtToken.Claims
                .FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;
            return _userService.GetByIdAsync(Guid.Parse(userId));
        }
    }
}
