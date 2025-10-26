namespace BuildingBlocks.Messaging.Events
{
    public record UserUpdatedEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
        public string Username { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string? Phone { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
