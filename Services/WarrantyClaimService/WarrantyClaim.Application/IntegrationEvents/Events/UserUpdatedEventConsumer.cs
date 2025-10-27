using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WarrantyClaim.Application.Data;
using WarrantyClaim.Domain.Models;

namespace WarrantyClaim.Application.IntegrationEvents.Events
{
    public class UserUpdatedEventConsumer : IConsumer<UserUpdatedEvent>
    {
        private readonly IApplicationDbContext _context;

        public UserUpdatedEventConsumer(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserUpdatedEvent> context)
        {
            var userEvent = context.Message;

            var technician = await _context.Technicians
                .FirstOrDefaultAsync(t => t.StaffId == userEvent.UserId, context.CancellationToken);

            if (technician != null)
            {
                technician.FullName = userEvent.Username; // Assuming FullName is Username
                technician.Email = userEvent.Email;
                technician.Phone = userEvent.Phone;
                // Status mapping if needed

                await _context.SaveChangesAsync(context.CancellationToken);
            }
        }
    }
}
