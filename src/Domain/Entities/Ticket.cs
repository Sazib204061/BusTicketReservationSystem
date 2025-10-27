using System;

namespace Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; private set; }
        public Guid BusScheduleId { get; private set; }
        public Guid PassengerId { get; private set; }
        public Guid SeatId { get; private set; }
        public string BoardingPoint { get; private set; }
        public string DroppingPoint { get; private set; }
        public decimal Fare { get; private set; }
        public DateTime BookingDate { get; private set; }
        public TicketStatus Status { get; private set; }

        public virtual BusSchedule BusSchedule { get; private set; }
        public virtual Passenger Passenger { get; private set; }
        public virtual Seat Seat { get; private set; }

        public Ticket(Guid busScheduleId, Guid passengerId, Guid seatId,
                     string boardingPoint, string droppingPoint, decimal fare)
        {
            Id = Guid.NewGuid();
            BusScheduleId = busScheduleId;
            PassengerId = passengerId;
            SeatId = seatId;
            BoardingPoint = boardingPoint;
            DroppingPoint = droppingPoint;
            Fare = fare;
            BookingDate = DateTime.UtcNow;
            Status = TicketStatus.Confirmed;
        }

        public void Cancel()
        {
            Status = TicketStatus.Cancelled;
        }

        protected Ticket() { }
    }

    public enum TicketStatus
    {
        Confirmed = 1,
        Cancelled = 2
    }
}