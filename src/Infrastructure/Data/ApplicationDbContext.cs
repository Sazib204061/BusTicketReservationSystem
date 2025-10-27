using System.Reflection;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Bus> Buses => Set<Bus>();
        public DbSet<Route> Routes => Set<Route>();
        public DbSet<BusSchedule> BusSchedules => Set<BusSchedule>();
        public DbSet<Seat> Seats => Set<Seat>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<Passenger> Passengers => Set<Passenger>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        public async Task<Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction> BeginTransactionAsync()
        {
            return await Database.BeginTransactionAsync();
        }
    }
}