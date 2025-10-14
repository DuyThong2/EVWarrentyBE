using Microsoft.EntityFrameworkCore;
using Vehicle.Application.Data;
using Vehicle.Application.Repositories;
using Vehicle.Application.Filters;
using Vehicle.Domain.Models;
using Vehicle.Domain.Enums;
using BuildingBlocks.Pagination;

namespace Vehicle.Infrastructure.Repositories
{
    public class WarrantyHistoryRepository : IWarrantyHistoryRepository
    {
        private readonly IApplicationDbContext _context;

        public WarrantyHistoryRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<WarrantyHistory>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.WarrantyHistories
                .Include(w => w.Vehicle)
                .Include(w => w.Part)
                .AsQueryable();

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(w => w.CreatedAt)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<WarrantyHistory>(pageIndex, pageSize, totalCount, items);
        }

        public async Task<WarrantyHistory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.WarrantyHistories
                .Include(w => w.Vehicle)
                .Include(w => w.Part)
                .FirstOrDefaultAsync(w => w.HistoryId == id, cancellationToken);
        }

        public async Task<IEnumerable<WarrantyHistory>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default)
        {
            return await _context.WarrantyHistories
                .Include(w => w.Vehicle)
                .Include(w => w.Part)
                .Where(w => w.VehicleId == vehicleId)
                .OrderByDescending(w => w.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<WarrantyHistory>> GetByVinAsync(string vin, CancellationToken cancellationToken = default)
        {
            return await _context.WarrantyHistories
                .Include(w => w.Vehicle)
                .Include(w => w.Part)
                .Where(w => w.Vehicle.VIN == vin)
                .OrderByDescending(w => w.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<PaginatedResult<WarrantyHistory>> FilterAsync(WarrantyHistoryFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _context.WarrantyHistories
                .Include(w => w.Vehicle)
                .Include(w => w.Part)
                .AsQueryable();

            // Apply filters
            if (filter.VehicleId.HasValue)
                query = query.Where(w => w.VehicleId == filter.VehicleId.Value);

            if (!string.IsNullOrEmpty(filter.VIN))
                query = query.Where(w => w.Vehicle.VIN.Contains(filter.VIN));

            if (filter.PartId.HasValue)
                query = query.Where(w => w.PartId == filter.PartId.Value);

            if (filter.ClaimId.HasValue)
                query = query.Where(w => w.ClaimId == filter.ClaimId.Value);

            if (filter.PolicyId.HasValue)
                query = query.Where(w => w.PolicyId == filter.PolicyId.Value);

            if (!string.IsNullOrEmpty(filter.EventType))
            {
                if (Enum.TryParse<WarrantyEventType>(filter.EventType, true, out var eventType))
                    query = query.Where(w => w.EventType == eventType);
            }

            if (!string.IsNullOrEmpty(filter.Description))
                query = query.Where(w => w.Description != null && w.Description.Contains(filter.Description));

            if (filter.PerformedBy.HasValue)
                query = query.Where(w => w.PerformedBy == filter.PerformedBy.Value);

            if (!string.IsNullOrEmpty(filter.ServiceCenterName))
                query = query.Where(w => w.ServiceCenterName != null && w.ServiceCenterName.Contains(filter.ServiceCenterName));

            if (!string.IsNullOrEmpty(filter.Status))
            {
                if (Enum.TryParse<WarrantyHistoryStatus>(filter.Status, true, out var status))
                    query = query.Where(w => w.Status == status);
            }

            if (filter.WarrantyStartDateFrom.HasValue)
                query = query.Where(w => w.WarrantyStartDate.HasValue && w.WarrantyStartDate >= filter.WarrantyStartDateFrom.Value);

            if (filter.WarrantyStartDateTo.HasValue)
                query = query.Where(w => w.WarrantyStartDate.HasValue && w.WarrantyStartDate <= filter.WarrantyStartDateTo.Value);

            if (filter.WarrantyEndDateFrom.HasValue)
                query = query.Where(w => w.WarrantyEndDate.HasValue && w.WarrantyEndDate >= filter.WarrantyEndDateFrom.Value);

            if (filter.WarrantyEndDateTo.HasValue)
                query = query.Where(w => w.WarrantyEndDate.HasValue && w.WarrantyEndDate <= filter.WarrantyEndDateTo.Value);

            if (filter.CreatedDateFrom.HasValue)
                query = query.Where(w => w.CreatedAt >= filter.CreatedDateFrom.Value);

            if (filter.CreatedDateTo.HasValue)
                query = query.Where(w => w.CreatedAt <= filter.CreatedDateTo.Value);

            // Apply sorting
            query = filter.SortBy?.ToLower() switch
            {
                "createdat" => filter.SortDescending ? query.OrderByDescending(w => w.CreatedAt) : query.OrderBy(w => w.CreatedAt),
                "updatedat" => filter.SortDescending ? query.OrderByDescending(w => w.UpdatedAt) : query.OrderBy(w => w.UpdatedAt),
                "eventtype" => filter.SortDescending ? query.OrderByDescending(w => w.EventType) : query.OrderBy(w => w.EventType),
                "status" => filter.SortDescending ? query.OrderByDescending(w => w.Status) : query.OrderBy(w => w.Status),
                "warrantystartdate" => filter.SortDescending ? query.OrderByDescending(w => w.WarrantyStartDate) : query.OrderBy(w => w.WarrantyStartDate),
                "warrantyenddate" => filter.SortDescending ? query.OrderByDescending(w => w.WarrantyEndDate) : query.OrderBy(w => w.WarrantyEndDate),
                _ => query.OrderByDescending(w => w.CreatedAt)
            };

            // Get total count before pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<WarrantyHistory>(pageIndex, pageSize, totalCount, items);
        }

        public async Task<WarrantyHistory> AddAsync(WarrantyHistory warrantyHistory, CancellationToken cancellationToken = default)
        {
            warrantyHistory.CreatedAt = DateTime.UtcNow;
            warrantyHistory.UpdatedAt = DateTime.UtcNow;

            _context.WarrantyHistories.Add(warrantyHistory);
            await _context.SaveChangesAsync(cancellationToken);
            return warrantyHistory;
        }

        public async Task<WarrantyHistory> UpdateAsync(WarrantyHistory warrantyHistory, CancellationToken cancellationToken = default)
        {
            warrantyHistory.UpdatedAt = DateTime.UtcNow;

            _context.WarrantyHistories.Update(warrantyHistory);
            await _context.SaveChangesAsync(cancellationToken);
            return warrantyHistory;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var warrantyHistory = await _context.WarrantyHistories.FindAsync(id);
            if (warrantyHistory == null)
                return false;

            _context.WarrantyHistories.Remove(warrantyHistory);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var warrantyHistory = await _context.WarrantyHistories.FindAsync(id);
            if (warrantyHistory == null)
                return false;

            warrantyHistory.Status = WarrantyHistoryStatus.Deleted;
            warrantyHistory.UpdatedAt = DateTime.UtcNow;

            _context.WarrantyHistories.Update(warrantyHistory);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
