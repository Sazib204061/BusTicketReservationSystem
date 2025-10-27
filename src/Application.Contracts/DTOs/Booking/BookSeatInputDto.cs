using System;

namespace Application.Contracts.DTOs.Booking
{
    public class BookSeatInputDto
    {
        public Guid BusScheduleId { get; set; }
        public Guid SeatId { get; set; }
        public string PassengerName { get; set; }
        public string MobileNumber { get; set; }
        public string BoardingPoint { get; set; }
        public string DroppingPoint { get; set; }
    }

    public class BookSeatResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid TicketId { get; set; }
        public string TicketNumber { get; set; }
    }
}