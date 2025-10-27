using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Application.Contracts.DTOs.Booking
{
    public class SeatPlanDto
    {
        public Guid BusScheduleId { get; set; }
        public string BusName { get; set; }
        public string CompanyName { get; set; }
        public List<SeatDto> Seats { get; set; } = new();
        public List<string> BoardingPoints { get; set; } = new();
        public List<string> DroppingPoints { get; set; } = new();
    }

    public class SeatDto
    {
        public Guid SeatId { get; set; }
        public string SeatNumber { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public SeatStatus Status { get; set; }
    }
}