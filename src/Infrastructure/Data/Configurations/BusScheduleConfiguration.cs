using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class BusScheduleConfiguration : IEntityTypeConfiguration<BusSchedule>
    {
        public void Configure(EntityTypeBuilder<BusSchedule> builder)
        {
            builder.HasKey(bs => bs.Id);

            builder.Property(bs => bs.DepartureTime)
                .IsRequired();

            builder.Property(bs => bs.ArrivalTime)
                .IsRequired();

            builder.Property(bs => bs.JourneyDate)
                .IsRequired();

            builder.HasOne(bs => bs.Bus)
                .WithMany(b => b.Schedules)
                .HasForeignKey(bs => bs.BusId);

            builder.HasOne(bs => bs.Route)
                .WithMany(r => r.Schedules)
                .HasForeignKey(bs => bs.RouteId);

            builder.HasMany(bs => bs.Tickets)
                .WithOne(t => t.BusSchedule)
                .HasForeignKey(t => t.BusScheduleId);
        }
    }
}