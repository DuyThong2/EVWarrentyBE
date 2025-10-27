namespace BuildingBlocks.Messaging.Events.UserEvent
{
    public record TechnicianDeletedEvent : IntegrationEvent
    {
        public Guid StaffId { get; init; }
    }
}
