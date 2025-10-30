using BuildingBlocks.Pagination;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vehicle.Application.Filters;

namespace Vehicle.Application.Repositories
{
    public interface IVehicleImageRepository
    {
        Task<PaginatedResult<Vehicle.Domain.Models.VehicleImage>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<Vehicle.Domain.Models.VehicleImage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedResult<Vehicle.Domain.Models.VehicleImage>> FilterAsync(VehicleImageFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<Guid> CreateAsync(Vehicle.Domain.Models.VehicleImage image, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Vehicle.Domain.Models.VehicleImage image, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}


