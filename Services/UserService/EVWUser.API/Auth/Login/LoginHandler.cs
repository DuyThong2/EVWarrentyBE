using AutoMapper;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using EVWUser.API.Data;
using EVWUser.API.Dtos;
using EVWUser.API.Extensions.Jwt;
using EVWUser.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EVWUser.API.Auth.Login
{
    public class LoginHandler : ICommandHandler<LoginCommand, LoginResult>
    {
        private readonly UserDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public LoginHandler(UserDbContext context, IJwtService jwtService, IMapper mapper)
        {
            _context = context;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Where(u => u.Email.ToLower() == command.LoginRequest.Email.ToLower()).FirstOrDefaultAsync(cancellationToken) ?? 
                throw new BadRequestException("Email does not exist");

            var isPasswordValid = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, command.LoginRequest.Password);
            if (isPasswordValid == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid password");
            }

            var userDto = _mapper.Map<UserDto>(user);
            var token = _jwtService.GenerateJwtToken(user);

            return new LoginResult(new LoginResponse
            {
                User = userDto,
                Token = new TokenResponse
                {
                    AccessToken = token,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                }
            });
        }
    }
}
