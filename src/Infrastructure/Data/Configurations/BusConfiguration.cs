using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class BusConfiguration : IEntityTypeConfiguration<Bus>
    {
        public void Configure(EntityTypeBuilder<Bus> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.CompanyName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.BusName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.BusNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.TotalSeats)
                .IsRequired();

            builder.HasMany(b => b.Schedules)
                .WithOne(bs => bs.Bus)
                .HasForeignKey(bs => bs.BusId);

            builder.HasMany(b => b.Seats)
                .WithOne(s => s.Bus)
                .HasForeignKey(s => s.BusId);
        }
    }
}