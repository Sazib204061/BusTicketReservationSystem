namespace Domain.Entities
{
    public class BusSchedule
    {
        public Guid Id { get; private set; }
        public Guid BusId { get; private set; }
        public Guid RouteId { get; private set; }
        public DateTime DepartureTime { get; private set; }
        public DateTime ArrivalTime { get; private set; }
        public DateTime JourneyDate { get; private set; }

        public virtual Bus Bus { get; private set; }
        public virtual Route Route { get; private set; }
        public virtual ICollection<Ticket> Tickets { get; private set; }

        public BusSchedule(Guid busId, Guid routeId, DateTime departureTime,
                         DateTime arrivalTime, DateTime journeyDate)
        {
            Id = Guid.NewGuid();
            BusId = busId;
            RouteId = routeId;
            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            JourneyDate = journeyDate;
            Tickets = new List<Ticket>();
        }

        protected BusSchedule() { }
    }
}