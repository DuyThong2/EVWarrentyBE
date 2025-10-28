using BuildingBlocks.Pagination;
using EVWUser.Data.Models;

namespace EVWUser.Data.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<PaginatedResult<User>> GetPagedAsync(PaginationRequest request);
        Task<PaginatedResult<User>> SearchAsync(Guid? roleId, string? email, PaginationRequest request);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task SoftDeleteAsync(Guid id);
        Task<PaginatedResult<User>> FilterAsync(string? username, string? email, string? phone, string? role, PaginationRequest request);
    }
}
