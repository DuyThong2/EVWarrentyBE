using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vehicle.Application.Filters;
using Vehicle.Application.Repositories;
using Vehicle.Domain.Enums;
using Vehicle.Domain.Models;
using Vehicle.Infrastructure.Data;

namespace Vehicle.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AppointmentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedResult<Appointment>> GetPagedAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var query = _dbContext.Appointments.AsNoTracking()
                .Include(a => a.Vehicle)
                .OrderByDescending(a => a.ScheduledDateTime);
            
            var count = await query.CountAsync(cancellationToken);
            var data = await query.Skip(zeroBased * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            
            return new PaginatedResult<Appointment>(zeroBased, pageSize, count, data);
        }

        public async Task<PaginatedResult<Appointment>> FilterAsync(AppointmentFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var query = _dbContext.Appointments.AsNoTracking()
                .Include(a => a.Vehicle)
                .AsQueryable();

            if (filter is not null)
            {
                if (filter.AppointmentId.HasValue)
                    query = query.Where(a => a.AppointmentId == filter.AppointmentId.Value);
                if (filter.VehicleId.HasValue)
                    query = query.Where(a => a.VehicleId == filter.VehicleId.Value);
                if (filter.ScheduledDateTimeFrom.HasValue)
                    query = query.Where(a => a.ScheduledDateTime >= filter.ScheduledDateTimeFrom.Value);
                if (filter.ScheduledDateTimeTo.HasValue)
                    query = query.Where(a => a.ScheduledDateTime <= filter.ScheduledDateTimeTo.Value);
                if (!string.IsNullOrWhiteSpace(filter.AppointmentType))
                {
                    if (Enum.TryParse<AppointmentType>(filter.AppointmentType, true, out var appointmentTypeEnum))
                        query = query.Where(a => a.AppointmentType == appointmentTypeEnum);
                }
                if (!string.IsNullOrWhiteSpace(filter.Status))
                {
                    if (Enum.TryParse<AppointmentStatus>(filter.Status, true, out var statusEnum))
                        query = query.Where(a => a.Status == statusEnum);
                }
                if (filter.CreatedFrom.HasValue)
                    query = query.Where(a => a.CreatedAt >= filter.CreatedFrom.Value);
                if (filter.CreatedTo.HasValue)
                    query = query.Where(a => a.CreatedAt <= filter.CreatedTo.Value);
            }

            query = query.OrderByDescending(a => a.ScheduledDateTime);

            var count = await query.CountAsync(cancellationToken);
            var data = await query.Skip(zeroBased * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            
            return new PaginatedResult<Appointment>(zeroBased, pageSize, count, data);
        }

        public async Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Appointments.AsNoTracking()
                .Include(a => a.Vehicle)
                .FirstOrDefaultAsync(a => a.AppointmentId == id, cancellationToken);
        }

        public async Task<PaginatedResult<Appointment>> GetByVehicleIdAsync(Guid vehicleId, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var query = _dbContext.Appointments.AsNoTracking()
                .Where(a => a.VehicleId == vehicleId)
                .Include(a => a.Vehicle)
                .OrderByDescending(a => a.ScheduledDateTime);
            
            var count = await query.CountAsync(cancellationToken);
            var data = await query.Skip(zeroBased * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            
            return new PaginatedResult<Appointment>(zeroBased, pageSize, count, data);
        }

        public async Task<PaginatedResult<Appointment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var zeroBased = pageIndex <= 0 ? 0 : pageIndex - 1;
            var query = _dbContext.Appointments.AsNoTracking()
                .Where(a => a.ScheduledDateTime >= startDate && a.ScheduledDateTime <= endDate)
                .Include(a => a.Vehicle)
                .OrderBy(a => a.ScheduledDateTime);
            
            var count = await query.CountAsync(cancellationToken);
            var data = await query.Skip(zeroBased * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            
            return new PaginatedResult<Appointment>(zeroBased, pageSize, count, data);
        }

        public async Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
        {
            if (appointment.AppointmentId == Guid.Empty)
                appointment.AppointmentId = Guid.NewGuid();

            await _dbContext.Appointments.AddAsync(appointment, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return appointment;
        }

        public async Task<bool> UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default)
        {
            var existing = await _dbContext.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointment.AppointmentId, cancellationToken);
            if (existing is null) return false;
            
            _dbContext.Entry(existing).CurrentValues.SetValues(appointment);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _dbContext.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == id, cancellationToken);
            if (existing is null) return false;
            
            existing.Status = AppointmentStatus.Deleted;
            existing.UpdatedAt = DateTime.UtcNow;
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
