using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using System;

namespace EVWUser.Data.Repositories.Impl
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly UserDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(UserDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                    throw new NotFoundException($"{typeof(TEntity).Name} not found");

                return entity;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new InternalServerException($"Error retrieving {typeof(TEntity).Name} by id");
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception)
            {
                throw new InternalServerException($"Error adding {typeof(TEntity).Name}");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InternalServerException($"Error deleting {typeof(TEntity).Name}");
            }
        }

        public async Task<TEntity> UpdateAsync(Guid id, TEntity entity)
        {
            try
            {
                var existingEntity = await GetByIdAsync(id);
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
                return existingEntity;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new InternalServerException($"Error updating {typeof(TEntity).Name}");
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception)
            {
                throw new InternalServerException($"Error retrieving all {typeof(TEntity).Name}");
            }
        }
    }
}
