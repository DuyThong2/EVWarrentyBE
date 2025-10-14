using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using EVWUser.Data.Enums;
using EVWUser.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EVWUser.Data.Repositories.Impl
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

                var user = await _context.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower()) ?? null;

                if (user == null)
                    throw new NotFoundException("Email not found");

                if (user.Status == UserStatus.INACTIVE)
                    throw new BadRequestException("User is inactive");

                return user;
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

        public async Task<PaginatedResult<User>> SearchAsync(Guid? roleId, string? email, PaginationRequest request)
        {
            try
            {
                var query = _context.Users.AsQueryable();

                if (!string.IsNullOrWhiteSpace(email))
                {
                    query = query.Where(u => u.Email.ToLower().Contains(email.ToLower()));
                }

                if (roleId.HasValue && roleId != Guid.Empty)
                {
                    query = query.Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId));
                }

                var totalCount = await query.LongCountAsync();

                var data = await query
                    .Skip((request.PageIndex) * request.PageSize)
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
                throw new InternalServerException("Error searching users", ex.Message);
            }
        }


        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                var emailExists = await _context.Users.AnyAsync(u => u.Email.ToLower() == user.Email.ToLower());
                if (emailExists)
                    throw new BadRequestException("Email already exists");

                var usernameExists = await _context.Users.AnyAsync(u => u.Username == user.Username);
                if (usernameExists)
                    throw new BadRequestException("Username already exists");

                await AddAsync(user);

                return user;
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new InternalServerException("Error creating user");
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.Username))
                    throw new BadRequestException("Username is required");

                if (user.UserRoles == null || !user.UserRoles.Any())
                    throw new BadRequestException("At least one role is required");

                var usernameExists = await _context.Users.AnyAsync(u => u.Username == user.Username && u.UserId != user.UserId);
                if (usernameExists)
                    throw new BadRequestException("Username already exists");

                return await UpdateAsync(user.UserId, user);
            }
            catch (BadRequestException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new InternalServerException("Error updating user");
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
