using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vehicle.Application.Filters;
using Vehicle.Application.Repositories;
using Vehicle.Infrastructure.Data;
using Vehicle.Domain.Enums;

namespace Vehicle.Infrastructure.Repositories
{
    public class VehicleImageRepository : IVehicleImageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VehicleImageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateAsync(Vehicle.Domain.Models.VehicleImage image, CancellationToken cancellationToken = default)
        {
            if (image.ImageId == Guid.Empty)
                image.ImageId = Guid.NewGuid();
            await _dbContext.VehicleImages.AddAsync(image, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return image.ImageId;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _dbContext.VehicleImages.FirstOrDefaultAsync(p => p.ImageId == id, cancellationToken);
            if (existing is null) return false;
            _dbContext.VehicleImages.Remove(existing);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<PaginatedResult<Vehicle.Domain.Models.VehicleImage>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var query = _dbContext.VehicleImages.AsNoTracking().OrderByDescending(p => p.CreatedAt);
            var count = await query.CountAsync(cancellationToken);
            var data = await query.Skip(zeroBased * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PaginatedResult<Vehicle.Domain.Models.VehicleImage>(zeroBased, pageSize, count, data);
        }

        public async Task<Vehicle.Domain.Models.VehicleImage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.VehicleImages.AsNoTracking().FirstOrDefaultAsync(p => p.ImageId == id, cancellationToken);
        }

        public async Task<PaginatedResult<Vehicle.Domain.Models.VehicleImage>> FilterAsync(VehicleImageFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var query = _dbContext.VehicleImages.AsNoTracking().AsQueryable();

            if (filter is not null)
            {
                if (filter.ImageId.HasValue)
                    query = query.Where(p => p.ImageId == filter.ImageId.Value);
                if (filter.VehicleId.HasValue)
                    query = query.Where(p => p.VehicleId == filter.VehicleId.Value);
                if (!string.IsNullOrWhiteSpace(filter.Url))
                    query = query.Where(p => p.Url.Contains(filter.Url));
                if (!string.IsNullOrWhiteSpace(filter.Status))
                {
                    if (Enum.TryParse<VehicleImageStatus>(filter.Status, true, out var statusEnum))
                        query = query.Where(p => p.Status == statusEnum);
                }
            }

            query = query.OrderByDescending(p => p.CreatedAt);

            var count = await query.CountAsync(cancellationToken);
            var data = await query.Skip(zeroBased * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PaginatedResult<Vehicle.Domain.Models.VehicleImage>(zeroBased, pageSize, count, data);
        }

        public async Task<bool> UpdateAsync(Vehicle.Domain.Models.VehicleImage image, CancellationToken cancellationToken = default)
        {
            var existing = await _dbContext.VehicleImages.FirstOrDefaultAsync(p => p.ImageId == image.ImageId, cancellationToken);
            if (existing is null) return false;
            _dbContext.Entry(existing).CurrentValues.SetValues(image);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}


