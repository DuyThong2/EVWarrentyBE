using BuildingBlocks.Pagination;
using EVWUser.Business.Dtos;
using EVWUser.Data.Models;

namespace EVWUser.Business.Services
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
        Task SetActiveAsync(Guid id);
        Task<UserDto> MapRolesToDto(User user);
        Task<PaginatedResult<UserDto>> FilterAsync(string? username, string? email, string? phone, string? role, PaginationRequest request, Guid? excludeUserId = null);
    }
}
