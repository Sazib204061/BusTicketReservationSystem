using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                

                // Clear existing data first in correct order
                await ClearExistingData(context);

                // Seed data in correct order
                await SeedBuses(context);
                await SeedRoutes(context);
                await SeedBusSchedules(context);
                await SeedSeats(context);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static async Task ClearExistingData(ApplicationDbContext context)
        {
            // Clear in reverse order of dependencies
            if (await context.Tickets.AnyAsync())
                context.Tickets.RemoveRange(await context.Tickets.ToListAsync());

            if (await context.Passengers.AnyAsync())
                context.Passengers.RemoveRange(await context.Passengers.ToListAsync());

            if (await context.BusSchedules.AnyAsync())
                context.BusSchedules.RemoveRange(await context.BusSchedules.ToListAsync());

            if (await context.Seats.AnyAsync())
                context.Seats.RemoveRange(await context.Seats.ToListAsync());

            if (await context.Routes.AnyAsync())
                context.Routes.RemoveRange(await context.Routes.ToListAsync());

            if (await context.Buses.AnyAsync())
                context.Buses.RemoveRange(await context.Buses.ToListAsync());

            await context.SaveChangesAsync();
        }

        private static async Task SeedBuses(ApplicationDbContext context)
        {
            if (await context.Buses.AnyAsync()) return;

            var buses = new List<Bus>
            {
                new Bus("Green Line Paribahan", "Business Class", "GL-2024", 40, true),
                new Bus("Shyamoli Paribahan", "Express Service", "SH-1234", 52, false),
                new Bus("Hanif Enterprise", "AC Coach", "HE-5678", 40, true),
                new Bus("Ena Transport", "Non-AC Service", "ET-9012", 52, false),
                new Bus("Soudia Coach", "Deluxe Coach", "SC-3456", 40, true)
            };

            await context.Buses.AddRangeAsync(buses);
            await context.SaveChangesAsync();
            Console.WriteLine($"Seeded {buses.Count} buses");
        }

        private static async Task SeedRoutes(ApplicationDbContext context)
        {
            if (await context.Routes.AnyAsync()) return;

            var routes = new List<Route>
            {
                new Route("Dhaka", "Chittagong", 1200, 250),
                new Route("Dhaka", "Rajshahi", 800, 240),
                new Route("Dhaka", "Khulna", 700, 200),
                new Route("Dhaka", "Sylhet", 900, 230),
                new Route("Chittagong", "Cox's Bazar", 500, 150),
                new Route("Rajshahi", "Rangpur", 400, 120),
                new Route("Chittagong", "Dhaka", 1200, 250),
                new Route("Rajshahi", "Dhaka", 800, 240),
                new Route("Khulna", "Dhaka", 700, 200),
                new Route("Sylhet", "Dhaka", 900, 230)
            };

            await context.Routes.AddRangeAsync(routes);
            await context.SaveChangesAsync();
            Console.WriteLine($"Seeded {routes.Count} routes");
        }

        private static async Task SeedBusSchedules(ApplicationDbContext context)
        {
            if (await context.BusSchedules.AnyAsync()) return;

            var buses = await context.Buses.ToListAsync();
            var routes = await context.Routes.ToListAsync();

            if (!buses.Any() || !routes.Any())
            {
                throw new Exception("Buses or Routes not seeded properly");
            }

            var random = new Random();
            var schedules = new List<BusSchedule>();

            // Create schedules for next 7 days
            for (int day = 1; day <= 7; day++)
            {
                var journeyDate = DateTime.Today.AddDays(day).Date;

                foreach (var route in routes)
                {
                    // Create 1-3 schedules per route per day
                    var scheduleCount = random.Next(1, 4);

                    for (int i = 0; i < scheduleCount; i++)
                    {
                        var bus = buses[random.Next(buses.Count)];

                        // Generate departure time between 6:00 AM and 10:00 PM
                        var departureHour = random.Next(6, 22);
                        var departureMinute = random.Next(0, 12) * 5;

                        // Create DateTime and explicitly convert to UTC
                        var departureTimeLocal = new DateTime(
                            journeyDate.Year, journeyDate.Month, journeyDate.Day,
                            departureHour, departureMinute, 0, DateTimeKind.Local
                        );
                        var departureTimeUtc = departureTimeLocal.ToUniversalTime();

                        // Calculate arrival time based on distance
                        var travelHours = route.DistanceInKm / 60.0;
                        var arrivalTimeUtc = departureTimeUtc.AddHours(travelHours);

                        var schedule = new BusSchedule(
                            bus.Id,
                            route.Id,
                            departureTimeUtc,
                            arrivalTimeUtc,
                            journeyDate.ToUniversalTime()
                        );

                        schedules.Add(schedule);
                    }
                }
            }

            await context.BusSchedules.AddRangeAsync(schedules);
            await context.SaveChangesAsync();
            Console.WriteLine($"Seeded {schedules.Count} bus schedules");
        }
        private static async Task SeedSeats(ApplicationDbContext context)
        {
            if (await context.Seats.AnyAsync()) return;

            var buses = await context.Buses.ToListAsync();

            if (!buses.Any())
            {
                throw new Exception("Buses not seeded properly");
            }

            var seats = new List<Seat>();

            foreach (var bus in buses)
            {
                var seatNumber = 1;

                // Create seat layout: 4 seats per row
                for (int row = 1; row <= Math.Ceiling(bus.TotalSeats / 4.0); row++)
                {
                    for (int col = 1; col <= 4 && seatNumber <= bus.TotalSeats; col++)
                    {
                        var seat = new Seat(
                            bus.Id,
                            seatNumber.ToString("D2"),
                            row,
                            col
                        );
                        seats.Add(seat);
                        seatNumber++;
                    }
                }
            }

            await context.Seats.AddRangeAsync(seats);
            await context.SaveChangesAsync();
            Console.WriteLine($"Seeded {seats.Count} seats");
        }
    }
}