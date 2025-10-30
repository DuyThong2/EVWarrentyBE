using System;
using System.Threading;
using System.Threading.Tasks;
using Vehicle.Domain.Models;
using Vehicle.Application.Filters;
using BuildingBlocks.Pagination;

namespace Vehicle.Application.Repositories
{
    public interface IAppointmentRepository
    {
        Task<PaginatedResult<Appointment>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<PaginatedResult<Appointment>> FilterAsync(AppointmentFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedResult<Appointment>> GetByVehicleIdAsync(Guid vehicleId, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<PaginatedResult<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
