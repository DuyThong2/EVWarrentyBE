using BuildingBlocks.Messaging.Events.UserEvent;
using EVWUser.Data.Enums;
using EVWUser.Data.Models;

namespace EVWUser.Business.AutoMapper
{
    // Mapper thuần: User -> TechnicianUpsertEvent / TechnicianDeletedEvent
    internal static class EventUtils
    {
        private static string ToStatusString(UserStatus status)
            => status == UserStatus.ACTIVE ? "ACTIVE" : "INACTIVE";

        private static string BuildFullName(User u)
            => string.IsNullOrWhiteSpace(u.Username) ? (u.Email ?? string.Empty) : u.Username;

        public static TechnicianUpsertEvent ToTechnicianUpsert(User u)
            => new TechnicianUpsertEvent
            {
                StaffId = u.UserId,
                FullName = BuildFullName(u),
                Email = u.Email,
                Phone = u.Phone,
                Status = ToStatusString(u.Status)
            };

        public static TechnicianDeletedEvent ToTechnicianDeleted(Guid staffId)
            => new TechnicianDeletedEvent
            {
                StaffId = staffId
            };
    }
}
