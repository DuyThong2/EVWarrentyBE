using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using EVWUser.API.Data;
using EVWUser.API.Enums;
using EVWUser.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EVWUser.API.Repositories.Impl
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    { 
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    throw new BadRequestException("Email is required");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                    throw new NotFoundException("Email not found");

                return user;
            }
            catch (BadRequestException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("Error retrieving user by email");
            }
        }

        public async Task<PaginatedResult<User>> GetPagedAsync(PaginationRequest request)
        {
            try
            {
                var query = _context.Users.AsQueryable();

                var totalCount = await query.LongCountAsync();

                var data = await query
                    .Skip(request.PageIndex * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                return new PaginatedResult<User>(
                    request.PageIndex,
                    request.PageSize,
                    totalCount,
                    data
                );
            }
            catch (Exception ex)
            {
                throw new InternalServerException("Error retrieving paged users");
            }
        }

        public async Task<PaginatedResult<User>> SearchByEmailAsync(string? email, PaginationRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return await GetPagedAsync(request);

                var query = _context.Users
                    .Where(u => u.Email.Contains(email, StringComparison.OrdinalIgnoreCase));

                var totalCount = await query.LongCountAsync();

                var data = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                return new PaginatedResult<User>(
                    request.PageIndex,
                    request.PageSize,
                    totalCount,
                    data
                );
            }
            catch (Exception ex)
            {
                throw new InternalServerException("Error searching users by email");
            }
        }

        public async Task SoftDeleteAsync(Guid id)
        {
            try
            {
                var user = await GetByIdAsync(id);

                user.Status = UserStatus.INACTIVE;
                _context.Users.Update(user);

                await _context.SaveChangesAsync();
            }
            catch (NotFoundException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InternalServerException("Error soft deleting user");
            }
        }
    }
}
