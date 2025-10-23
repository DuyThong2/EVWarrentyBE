namespace BuildingBlocks.Messaging.Events.UserEvent
{
    public record TechnicianUpsertEvent : IntegrationEvent
    {
        public Guid StaffId { get; init; }
        public string FullName { get; init; } = default!;
        public string? Email { get; init; }
        public string? Phone { get; init; }
        public string Status { get; init; } = "ACTIVE"; 
    }
}
