using System;

namespace Application.Contracts.DTOs.Search
{
    public class AvailableBusDto
    {
        public Guid BusScheduleId { get; set; }
        public string CompanyName { get; set; }
        public string BusName { get; set; }
        public string BusNumber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int SeatsLeft { get; set; }
        public decimal Price { get; set; }
        public bool HasAC { get; set; }
    }
}