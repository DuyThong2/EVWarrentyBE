using AutoMapper;
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

        public AuthService(IUserService userService, IJwtService jwtService, IMapper mapper)
        {
            _userService = userService;
            _jwtService = jwtService;
            _mapper = mapper;
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
