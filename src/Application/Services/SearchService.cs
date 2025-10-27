using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.DTOs.Search;
using Application.Contracts.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly IApplicationDbContext _context;

        public SearchService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AvailableBusDto>> SearchAvailableBusesAsync(string from, string to, DateTime journeyDate)
        {
            // Convert DateTime to UTC and use Date property for comparison
            var utcJourneyDate = journeyDate.Date.ToUniversalTime();
            var currentUtcTime = DateTime.UtcNow;

            var schedules = await _context.BusSchedules
                .Include(bs => bs.Bus)
                .Include(bs => bs.Route)
                .Include(bs => bs.Tickets)
                .Where(bs => bs.Route.FromCity.ToLower() == from.ToLower() &&
                           bs.Route.ToCity.ToLower() == to.ToLower() &&
                           bs.JourneyDate.Date == utcJourneyDate.Date && // Compare only dates
                           bs.DepartureTime > currentUtcTime) // Compare with UTC time
                .ToListAsync();

            var result = new List<AvailableBusDto>();

            foreach (var schedule in schedules)
            {
                var bookedSeatsCount = await _context.Tickets
                    .CountAsync(t => t.BusScheduleId == schedule.Id &&
                                   t.Status != TicketStatus.Cancelled);

                var seatsLeft = schedule.Bus.TotalSeats - bookedSeatsCount;

                if (seatsLeft > 0)
                {
                    result.Add(new AvailableBusDto
                    {
                        BusScheduleId = schedule.Id,
                        CompanyName = schedule.Bus.CompanyName,
                        BusName = schedule.Bus.BusName,
                        BusNumber = schedule.Bus.BusNumber,
                        StartTime = schedule.DepartureTime,
                        ArrivalTime = schedule.ArrivalTime,
                        SeatsLeft = seatsLeft,
                        Price = schedule.Route.BasePrice,
                        HasAC = schedule.Bus.HasAC
                    });
                }
            }

            return result;
        }
    }
}