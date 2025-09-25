using AutoMapper;
using AutoMapper.QueryableExtensions;
using BuildingBlocks.CQRS;
using BuildingBlocks.Exceptions;
using EVWUser.API.Data;
using EVWUser.API.Dtos;
using EVWUser.API.Extensions.Jwt;
using EVWUser.API.Models;
using EVWUser.API.Enums;
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

        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == request.LoginRequest.Email.ToLower(), cancellationToken)
            ?? throw new BadRequestException("Email does not exist");

            if (user.Status != UserStatus.ACTIVE)
            {
                throw new BadRequestException("User is inactive or locked by the EVM Staff");
            }

            var userDto = await _context.Users
                .Where(u => u.UserId == user.UserId)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstAsync(cancellationToken);

            var isPasswordValid = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.LoginRequest.Password);
            if (isPasswordValid == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid password");
            }

            var token = _jwtService.GenerateJwtToken(user);

            return new LoginResult(new LoginResponse
            {
                User = userDto,
                Token = new TokenResponse
                {
                    AccessToken = token,
                    ExpiresAt = DateTime.Now.AddMinutes(60)
                }
            });
        }
    }
}
