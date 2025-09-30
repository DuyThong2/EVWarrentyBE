using BuildingBlocks.Pagination;
using EVWUser.API.Dtos;
using EVWUser.API.Models;

namespace EVWUser.API.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<PaginatedResult<UserDto>> GetPagedAsync(PaginationRequest request);
        Task<PaginatedResult<UserDto>> SearchAsync(Guid? roleId, string? email, PaginationRequest request);
        Task<UserDto> AddAsync(UserCreateRequest request);
        Task<UserDto> UpdateAsync(Guid id, UserUpdateRequest request);
        Task SoftDeleteAsync(Guid id);
        Task<UserDto> MapRolesToDto(User user);
    }
}
