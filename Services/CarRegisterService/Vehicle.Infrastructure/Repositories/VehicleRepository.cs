using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public VehicleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateAsync(Vehicle.Domain.Models.Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            if (vehicle.VehicleId == Guid.Empty)
                vehicle.VehicleId = Guid.NewGuid();

            await _dbContext.Vehicles.AddAsync(vehicle, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return vehicle.VehicleId;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == id, cancellationToken);
            if (existing is null) return false;
            _dbContext.Vehicles.Remove(existing);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<PaginatedResult<Vehicle.Domain.Models.Vehicle>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var query = _dbContext.Vehicles.AsNoTracking()
                .Include(v => v.Customer)
                .Include(v => v.Parts)
                .OrderByDescending(v => v.CreatedAt);
            var count = await query.CountAsync(cancellationToken);
            var data = await query.Skip(zeroBased * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PaginatedResult<Vehicle.Domain.Models.Vehicle>(zeroBased, pageSize, count, data);
        }

        public async Task<Vehicle.Domain.Models.Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Vehicles.AsNoTracking()
                .Include(v => v.Customer)
                .Include(v => v.Parts)
                .FirstOrDefaultAsync(v => v.VehicleId == id, cancellationToken);
        }

        public async Task<PaginatedResult<Vehicle.Domain.Models.Vehicle>> FilterAsync(VehicleFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var query = _dbContext.Vehicles.AsNoTracking()
                .Include(v => v.Customer)
                .Include(v => v.Parts)
                .AsQueryable();

            if (filter is not null)
            {
                if (filter.VehicleId.HasValue)
                    query = query.Where(v => v.VehicleId == filter.VehicleId.Value);
                if (filter.CustomerId.HasValue)
                    query = query.Where(v => v.CustomerId == filter.CustomerId.Value);
                if (!string.IsNullOrWhiteSpace(filter.VIN))
                    query = query.Where(v => v.VIN.Contains(filter.VIN));
                if (!string.IsNullOrWhiteSpace(filter.PlateNumber))
                    query = query.Where(v => v.PlateNumber != null && v.PlateNumber.Contains(filter.PlateNumber));
                if (!string.IsNullOrWhiteSpace(filter.Model))
                    query = query.Where(v => v.Model != null && v.Model.Contains(filter.Model));
                if (!string.IsNullOrWhiteSpace(filter.Trim))
                    query = query.Where(v => v.Trim != null && v.Trim.Contains(filter.Trim));
                if (filter.ModelYear.HasValue)
                    query = query.Where(v => v.ModelYear == filter.ModelYear.Value);
                if (!string.IsNullOrWhiteSpace(filter.Color))
                    query = query.Where(v => v.Color != null && v.Color.Contains(filter.Color));
                if (!string.IsNullOrWhiteSpace(filter.Status))
                {
                    if (Enum.TryParse<VehicleStatus>(filter.Status, true, out var statusEnum))
                        query = query.Where(v => v.Status == statusEnum);
                }

                if (filter.PurchaseDateFrom.HasValue)
                    query = query.Where(v => v.PurchaseDate >= filter.PurchaseDateFrom.Value);
                if (filter.PurchaseDateTo.HasValue)
                    query = query.Where(v => v.PurchaseDate <= filter.PurchaseDateTo.Value);
                if (filter.WarrantyStartFrom.HasValue)
                    query = query.Where(v => v.WarrantyStartDate >= filter.WarrantyStartFrom.Value);
                if (filter.WarrantyStartTo.HasValue)
                    query = query.Where(v => v.WarrantyStartDate <= filter.WarrantyStartTo.Value);
                if (filter.WarrantyEndFrom.HasValue)
                    query = query.Where(v => v.WarrantyEndDate >= filter.WarrantyEndFrom.Value);
                if (filter.WarrantyEndTo.HasValue)
                    query = query.Where(v => v.WarrantyEndDate <= filter.WarrantyEndTo.Value);
                if (filter.CreatedFrom.HasValue)
                    query = query.Where(v => v.CreatedAt >= filter.CreatedFrom.Value);
                if (filter.CreatedTo.HasValue)
                    query = query.Where(v => v.CreatedAt <= filter.CreatedTo.Value);
            }

            query = query.OrderByDescending(v => v.CreatedAt);

            var count = await query.CountAsync(cancellationToken);
            var data = await query.Skip(zeroBased * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return new PaginatedResult<Vehicle.Domain.Models.Vehicle>(zeroBased, pageSize, count, data);
        }
        public async Task<Vehicle.Domain.Models.Vehicle?> GetByVinAsync(string vin, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Vehicles.AsNoTracking()
                .Include(v => v.Customer)
                .Include(v => v.Parts)
                .FirstOrDefaultAsync(v => v.VIN == vin, cancellationToken);
        }


        public async Task<bool> UpdateAsync(Vehicle.Domain.Models.Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            var existing = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == vehicle.VehicleId, cancellationToken);
            if (existing is null) return false;
            _dbContext.Entry(existing).CurrentValues.SetValues(vehicle);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}


