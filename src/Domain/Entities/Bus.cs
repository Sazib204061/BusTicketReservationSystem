namespace Domain.Entities
{
    public class Bus
    {
        public Guid Id { get; private set; }
        public string CompanyName { get; private set; }
        public string BusName { get; private set; }
        public string BusNumber { get; private set; }
        public int TotalSeats { get; private set; }
        public bool HasAC { get; private set; }

        public virtual ICollection<BusSchedule> Schedules { get; private set; }
        public virtual ICollection<Seat> Seats { get; private set; }

        public Bus(string companyName, string busName, string busNumber, int totalSeats, bool hasAC)
        {
            Id = Guid.NewGuid();
            CompanyName = companyName;
            BusName = busName;
            BusNumber = busNumber;
            TotalSeats = totalSeats;
            HasAC = hasAC;
            Schedules = new List<BusSchedule>();
            Seats = new List<Seat>();
        }

        protected Bus() { }
    }
}
