using Vehicle.Domain.Models;
using Vehicle.Application.Filters;
using BuildingBlocks.Pagination;

namespace Vehicle.Application.Repositories
{
    public interface IWarrantyHistoryRepository
    {
        Task<PaginatedResult<WarrantyHistory>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<WarrantyHistory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedResult<WarrantyHistory>> FilterAsync(WarrantyHistoryFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<IEnumerable<WarrantyHistory>> GetByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken = default);
        Task<IEnumerable<WarrantyHistory>> GetByVinAsync(string vin, CancellationToken cancellationToken = default);
        Task<WarrantyHistory> AddAsync(WarrantyHistory warrantyHistory, CancellationToken cancellationToken = default);
        Task<WarrantyHistory> UpdateAsync(WarrantyHistory warrantyHistory, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
