using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.DTOs.Booking;
using Application.Contracts.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IApplicationDbContext _context;

        public BookingService(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SeatPlanDto> GetSeatPlanAsync(Guid busScheduleId)
        {
            var schedule = await _context.BusSchedules
                .Include(bs => bs.Bus)
                    .ThenInclude(b => b.Seats)
                .Include(bs => bs.Route)
                .FirstOrDefaultAsync(bs => bs.Id == busScheduleId);

            if (schedule == null)
                throw new ArgumentException("Bus schedule not found");

            var tickets = await _context.Tickets
                .Include(t => t.Seat)
                .Where(t => t.BusScheduleId == busScheduleId &&
                           t.Status != TicketStatus.Cancelled)
                .ToListAsync();

            var seatDtos = schedule.Bus.Seats
                .OrderBy(s => s.Row)
                .ThenBy(s => s.Column)
                .Select(seat =>
                {
                    var ticket = tickets.FirstOrDefault(t => t.SeatId == seat.Id);
                    var status = ticket != null ?
                        (ticket.Status == TicketStatus.Confirmed ? SeatStatus.Sold : SeatStatus.Available)
                        : seat.Status;

                    return new SeatDto
                    {
                        SeatId = seat.Id,
                        SeatNumber = seat.SeatNumber,
                        Row = seat.Row,
                        Column = seat.Column,
                        Status = status
                    };
                })
                .ToList();

            var boardingPoints = new List<string>
            {
                $"{schedule.DepartureTime:hh:mm tt} - {schedule.Route.FromCity} Counter",
                $"{schedule.DepartureTime.AddHours(1):hh:mm tt} - {schedule.Route.FromCity} Main Terminal"
            };

            var droppingPoints = new List<string>
            {
                $"{schedule.ArrivalTime:hh:mm tt} - {schedule.Route.ToCity} Counter",
                $"{schedule.ArrivalTime.AddHours(-1):hh:mm tt} - {schedule.Route.ToCity} Main Terminal"
            };

            return new SeatPlanDto
            {
                BusScheduleId = busScheduleId,
                BusName = schedule.Bus.BusName,
                CompanyName = schedule.Bus.CompanyName,
                Seats = seatDtos,
                BoardingPoints = boardingPoints,
                DroppingPoints = droppingPoints
            };
        }

        public async Task<BookSeatResultDto> BookSeatAsync(BookSeatInputDto input)
        {
            using var transaction = await _context.BeginTransactionAsync();

            try
            {
                var seat = await _context.Seats.FindAsync(input.SeatId);
                if (seat == null)
                    return new BookSeatResultDto
                    {
                        Success = false,
                        Message = "Seat not found"
                    };

                var existingTicket = await _context.Tickets
                    .FirstOrDefaultAsync(t => t.BusScheduleId == input.BusScheduleId &&
                                            t.SeatId == input.SeatId &&
                                            t.Status != TicketStatus.Cancelled);

                if (existingTicket != null)
                    return new BookSeatResultDto
                    {
                        Success = false,
                        Message = "Seat is already booked"
                    };

                var passenger = await _context.Passengers
                    .FirstOrDefaultAsync(p => p.MobileNumber == input.MobileNumber);

                if (passenger == null)
                {
                    passenger = new Passenger(input.PassengerName, input.MobileNumber);
                    await _context.Passengers.AddAsync(passenger);
                }

                var schedule = await _context.BusSchedules
                    .Include(bs => bs.Route)
                    .FirstOrDefaultAsync(bs => bs.Id == input.BusScheduleId);

                var ticket = new Ticket(
                    input.BusScheduleId,
                    passenger.Id,
                    input.SeatId,
                    input.BoardingPoint,
                    input.DroppingPoint,
                    schedule.Route.BasePrice
                );

                seat.Book();

                await _context.Tickets.AddAsync(ticket);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new BookSeatResultDto
                {
                    Success = true,
                    Message = "Seat booked successfully",
                    TicketId = ticket.Id,
                    TicketNumber = $"TKT-{ticket.Id.ToString().Substring(0, 8).ToUpper()}"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new BookSeatResultDto
                {
                    Success = false,
                    Message = $"Booking failed: {ex.Message}"
                };
            }
        }
    }
}