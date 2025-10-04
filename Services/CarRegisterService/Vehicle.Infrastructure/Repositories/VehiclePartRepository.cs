using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vehicle.Application.Filters;
using Vehicle.Application.Repositories;
using Vehicle.Domain.Enums;
using Vehicle.Infrastructure.Data;

namespace Vehicle.Infrastructure.Repositories
{
    public class VehiclePartRepository : IVehiclePartRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VehiclePartRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateAsync(Vehicle.Domain.Models.VehiclePart part, CancellationToken cancellationToken = default)
        {
            if (part.PartId == Guid.Empty)
                part.PartId = Guid.NewGuid();
            await _dbContext.VehicleParts.AddAsync(part, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return part.PartId;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _dbContext.VehicleParts.FirstOrDefaultAsync(p => p.PartId == id, cancellationToken);
            if (existing is null) return false;
            _dbContext.VehicleParts.Remove(existing);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<PaginatedResult<Vehicle.Domain.Models.VehiclePart>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var query = _dbContext.VehicleParts.AsNoTracking().OrderByDescending(p => p.InstalledAt);
            var count = await query.CountAsync(cancellationToken);
            var data = await query.Skip(zeroBased * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PaginatedResult<Vehicle.Domain.Models.VehiclePart>(zeroBased, pageSize, count, data);
        }

        public async Task<Vehicle.Domain.Models.VehiclePart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.VehicleParts.AsNoTracking().FirstOrDefaultAsync(p => p.PartId == id, cancellationToken);
        }

        public async Task<PaginatedResult<Vehicle.Domain.Models.VehiclePart>> FilterAsync(VehiclePartFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var query = _dbContext.VehicleParts.AsNoTracking().AsQueryable();

            if (filter is not null)
            {
                if (filter.PartId.HasValue)
                    query = query.Where(p => p.PartId == filter.PartId.Value);
                if (filter.VehicleId.HasValue)
                    query = query.Where(p => p.VehicleId == filter.VehicleId.Value);
                if (!string.IsNullOrWhiteSpace(filter.SerialNumber))
                    query = query.Where(p => p.SerialNumber.Contains(filter.SerialNumber));
                if (!string.IsNullOrWhiteSpace(filter.PartType))
                    query = query.Where(p => p.PartType != null && p.PartType.Contains(filter.PartType));
                if (!string.IsNullOrWhiteSpace(filter.BatchCode))
                    query = query.Where(p => p.BatchCode != null && p.BatchCode.Contains(filter.BatchCode));

                if (filter.InstalledFrom.HasValue)
                    query = query.Where(p => p.InstalledAt >= filter.InstalledFrom.Value);
                if (filter.InstalledTo.HasValue)
                    query = query.Where(p => p.InstalledAt <= filter.InstalledTo.Value);
                if (filter.WarrantyStartFrom.HasValue)
                    query = query.Where(p => p.WarrantyStartDate >= filter.WarrantyStartFrom.Value);
                if (filter.WarrantyStartTo.HasValue)
                    query = query.Where(p => p.WarrantyStartDate <= filter.WarrantyStartTo.Value);
                if (filter.WarrantyEndFrom.HasValue)
                    query = query.Where(p => p.WarrantyEndDate >= filter.WarrantyEndFrom.Value);
                if (filter.WarrantyEndTo.HasValue)
                    query = query.Where(p => p.WarrantyEndDate <= filter.WarrantyEndTo.Value);

                if (!string.IsNullOrWhiteSpace(filter.Status))
                {
                    if (Enum.TryParse<PartStatus>(filter.Status, true, out var statusEnum))
                        query = query.Where(p => p.Status == statusEnum);
                }
            }

            query = query.OrderByDescending(p => p.InstalledAt);

            var count = await query.CountAsync(cancellationToken);
            var data = await query.Skip(zeroBased * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PaginatedResult<Vehicle.Domain.Models.VehiclePart>(zeroBased, pageSize, count, data);
        }

        public async Task<bool> UpdateAsync(Vehicle.Domain.Models.VehiclePart part, CancellationToken cancellationToken = default)
        {
            var existing = await _dbContext.VehicleParts.FirstOrDefaultAsync(p => p.PartId == part.PartId, cancellationToken);
            if (existing is null) return false;
            _dbContext.Entry(existing).CurrentValues.SetValues(part);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}


