using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.BoardingPoint)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.DroppingPoint)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Fare)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(t => t.BookingDate)
                .IsRequired();

            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.HasOne(t => t.BusSchedule)
                .WithMany(bs => bs.Tickets)
                .HasForeignKey(t => t.BusScheduleId);

            builder.HasOne(t => t.Passenger)
                .WithMany(p => p.Tickets)
                .HasForeignKey(t => t.PassengerId);

            builder.HasOne(t => t.Seat)
                .WithMany()
                .HasForeignKey(t => t.SeatId);
        }
    }
}