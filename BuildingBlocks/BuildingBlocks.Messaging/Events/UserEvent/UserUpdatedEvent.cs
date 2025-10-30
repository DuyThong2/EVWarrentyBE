using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging.Events.UserEvent
{
    public record UserUpdatedEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
        public string Email { get; init; } = default!;
        public string Username { get; init; } = default!;
        public string? Phone { get; init; }
        public string? AvatarUrl { get; init; }
        public string? Status { get; init; }
        public IReadOnlyList<Guid> RoleIds { get; init; } = Array.Empty<Guid>();
        public IReadOnlyList<string> RoleNames { get; init; } = Array.Empty<string>();
    }
}
