using BuildingBlocks.Pagination;
using EVWUser.API.Models;

namespace EVWUser.API.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<PaginatedResult<User>> GetPagedAsync(PaginationRequest request);
        Task<PaginatedResult<User>> SearchByEmailAsync(string? email, PaginationRequest request);
        Task SoftDeleteAsync(Guid id);
    }
}
