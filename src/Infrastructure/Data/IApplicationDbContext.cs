using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Bus> Buses { get; }
        DbSet<Route> Routes { get; }
        DbSet<BusSchedule> BusSchedules { get; }
        DbSet<Seat> Seats { get; }
        DbSet<Ticket> Tickets { get; }
        DbSet<Passenger> Passengers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}