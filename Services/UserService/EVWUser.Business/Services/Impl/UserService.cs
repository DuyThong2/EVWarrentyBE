using AutoMapper;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Pagination;
using EVWUser.Business.Dtos;
using EVWUser.Data.Enums;
using EVWUser.Data.Models;
using EVWUser.Data.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace EVWUser.Business.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public UserService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IMapper mapper, IRoleRepository roleRepository, IPublishEndpoint publishEndpoint)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return  await MapRolesToDto(user);
        }
        
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<PaginatedResult<UserDto>> GetPagedAsync(PaginationRequest request)
        {
            var pagedUsers = await _userRepository.GetPagedAsync(request);

            var mapped = new List<UserDto>();

            foreach (var user in pagedUsers.Data)
            {
                var userDto = await MapRolesToDto(user);
                mapped.Add(userDto);
            }

            return new PaginatedResult<UserDto>(
                pagedUsers.PageIndex,
                pagedUsers.PageSize,
                pagedUsers.Count,
                mapped
            );
        }

        public async Task<PaginatedResult<UserDto>> SearchAsync(Guid? roleId, string? email, PaginationRequest request)
        {
            var pagedUsers = await _userRepository.SearchAsync(roleId, email, request);

            var mapped = new List<UserDto>();

            foreach (var user in pagedUsers.Data)
            {
                var userDto = await MapRolesToDto(user);
                mapped.Add(userDto);
            }

            return new PaginatedResult<UserDto>(
                pagedUsers.PageIndex,
                pagedUsers.PageSize,
                pagedUsers.Count,
                mapped
            );
        }

        public async Task<UserDto> AddAsync(UserCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new BadRequestException("Email is required");

            if (string.IsNullOrWhiteSpace(request.Username))
                throw new BadRequestException("Username is required");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new BadRequestException("Password is required");

            if (request.Roles == null || !request.Roles.Any())
                throw new BadRequestException("At least one role is required");

            var user = _mapper.Map<User>(request);
            user.UserId = Guid.NewGuid();
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.Status = UserStatus.ACTIVE;

            var created = await _userRepository.CreateUserAsync(user);

            await AssignRolesToUserAsync(created, request.Roles);

            return await MapRolesToDto(created);
        }

        public async Task<UserDto> UpdateAsync(Guid id, UserUpdateRequest request)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            existingUser = _mapper.Map(request, existingUser);
            var existingRoleIds = await _userRoleRepository.GetRoleIdsByUserIdAsync(id);

            if (request.Roles != null && request.Roles.Any())
            {
                await RemoveRolesFromUserAsync(id, existingRoleIds);
                await AssignRolesToUserAsync(existingUser, request.Roles);
            }

            var updated = await _userRepository.UpdateUserAsync(existingUser);
            await _publishEndpoint.Publish(new UserUpdatedEvent
            {
                UserId = updated.UserId,
                Username = updated.Username,
                Email = updated.Email,
                Phone = updated.Phone,
                Status = updated.Status.ToString()
            });
            return await MapRolesToDto(updated);
        }

        public async Task SoftDeleteAsync(Guid id)
        {
            await _userRepository.SoftDeleteAsync(id);
        }

        public async Task<UserDto> MapRolesToDto(User user)
        {
            var userDto = _mapper.Map<UserDto>(user);

            var roleNames = new List<string>();
            var userRoles = await _userRoleRepository.GetByUserIdAsync(user.UserId);
            foreach (var userRole in userRoles)
            {
                var role = await _roleRepository.GetByIdAsync(userRole.RoleId);
                if (role != null)
                {
                    roleNames.Add(role.Name);
                }
            }
            userDto.Roles = roleNames;
            return userDto;
        }

        private async Task AssignRolesToUserAsync(User user, IEnumerable<Guid> roleIds)
        {
            foreach (var roleId in roleIds)
            {
                var role = await _roleRepository.GetByIdAsync(roleId);
                if (role == null)
                {
                    throw new Exception("Role does not exist.");
                }
                var userRole = new UserRole
                {
                    UserId = user.UserId,
                    RoleId = roleId,
                };
                try
                {
                    var newUserRole = await _userRoleRepository.AddAsync(userRole);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to assign role to user: {ex.Message}");
                }
            }
        }

        private async Task RemoveRolesFromUserAsync(Guid userId, IEnumerable<Guid> roleIds)
        {
            if (roleIds == null || !roleIds.Any())
            {
                return;
            }
            foreach (var roleId in roleIds)
            {
                var userRole = await _userRoleRepository.GetByUserIdAndRoleIdAsync(userId, roleId);
                if (userRole != null)
                {
                    await _userRoleRepository.DeleteAsync(userRole.UserRoleId);
                }
            }
        }
    }
}
