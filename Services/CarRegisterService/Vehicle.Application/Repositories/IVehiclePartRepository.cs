using BuildingBlocks.Pagination;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vehicle.Application.Filters;

namespace Vehicle.Application.Repositories
{
    public interface IVehiclePartRepository
    {
        Task<PaginatedResult<Vehicle.Domain.Models.VehiclePart>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<Vehicle.Domain.Models.VehiclePart?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedResult<Vehicle.Domain.Models.VehiclePart>> FilterAsync(VehiclePartFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<Guid> CreateAsync(Vehicle.Domain.Models.VehiclePart part, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Vehicle.Domain.Models.VehiclePart part, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}


