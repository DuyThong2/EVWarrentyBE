using BuildingBlocks.Pagination;
using EVWUser.API.Dtos;

namespace EVWUser.API.Services
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<UserDto?> GetByEmailAsync(string email);
        Task<PaginatedResult<UserDto>> GetPagedAsync(PaginationRequest request);
        Task<PaginatedResult<UserDto>> SearchByEmailAsync(string? email, PaginationRequest request);
        Task<UserDto> AddAsync(UserRequest request);
        Task<UserDto> UpdateAsync(Guid id, UserRequest request);
        Task SoftDeleteAsync(Guid id);
    }
}
