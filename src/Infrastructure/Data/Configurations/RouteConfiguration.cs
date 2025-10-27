using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.FromCity)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.ToCity)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.BasePrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasMany(r => r.Schedules)
                .WithOne(bs => bs.Route)
                .HasForeignKey(bs => bs.RouteId);
        }
    }
}