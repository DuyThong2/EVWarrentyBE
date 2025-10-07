using BuildingBlocks.Pagination;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vehicle.Application.Filters;

namespace Vehicle.Application.Repositories
{
    public interface IVehicleRepository
    {
        Task<PaginatedResult<Vehicle.Domain.Models.Vehicle>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<Vehicle.Domain.Models.Vehicle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedResult<Vehicle.Domain.Models.Vehicle>> FilterAsync(VehicleFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<Guid> CreateAsync(Vehicle.Domain.Models.Vehicle vehicle, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Vehicle.Domain.Models.Vehicle vehicle, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}


