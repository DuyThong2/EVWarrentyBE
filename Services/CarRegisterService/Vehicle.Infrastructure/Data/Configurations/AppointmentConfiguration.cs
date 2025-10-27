using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vehicle.Domain.Models;
using Vehicle.Domain.Enums;

namespace Vehicle.Infrastructure.Data.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> b)
        {
            b.ToTable("appointment");

            b.HasKey(x => x.AppointmentId);
            b.Property(x => x.AppointmentId).HasColumnName("appointmentId");

            b.Property(x => x.VehicleId).HasColumnName("vehicleId");

            b.Property(x => x.ScheduledDateTime).HasColumnName("scheduledDateTime").IsRequired();
            b.Property(x => x.Notes).HasColumnName("notes").HasMaxLength(500);

            b.Property(x => x.AppointmentType)
                .HasColumnName("appointmentType")
                .HasConversion(
                    v => v.ToString(),
                    v => (AppointmentType)Enum.Parse(typeof(AppointmentType), v))
                .HasDefaultValue(AppointmentType.Other);

            b.Property(x => x.Status)
                .HasColumnName("status")
                .HasDefaultValue(AppointmentStatus.Scheduled)
                .HasConversion(
                    v => v.ToString(),
                    v => (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), v));

            b.Property(x => x.CreatedAt).HasColumnName("createdAt");
            b.Property(x => x.UpdatedAt).HasColumnName("updatedAt");
            b.Property(x => x.CreatedBy).HasColumnName("createdBy");

            b.HasIndex(x => x.VehicleId).HasDatabaseName("ix_appointment_vehicle");
            b.HasIndex(x => x.ScheduledDateTime).HasDatabaseName("ix_appointment_scheduleddatetime");
            b.HasIndex(x => x.Status).HasDatabaseName("ix_appointment_status");

            b.HasOne(x => x.Vehicle)
                .WithMany(v => v.Appointments)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
