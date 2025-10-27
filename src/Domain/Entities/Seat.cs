using System;

namespace Domain.Entities
{
    public class Seat
    {
        public Guid Id { get; private set; }
        public Guid BusId { get; private set; }
        public string SeatNumber { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        public SeatStatus Status { get; private set; }

        public virtual Bus Bus { get; private set; }

        public Seat(Guid busId, string seatNumber, int row, int column)
        {
            Id = Guid.NewGuid();
            BusId = busId;
            SeatNumber = seatNumber;
            Row = row;
            Column = column;
            Status = SeatStatus.Available;
        }

        public void Book()
        {
            if (Status != SeatStatus.Available)
                throw new InvalidOperationException("Seat is not available for booking");

            Status = SeatStatus.Booked;
        }

        public void Sell()
        {
            if (Status != SeatStatus.Booked && Status != SeatStatus.Available)
                throw new InvalidOperationException("Seat cannot be sold");

            Status = SeatStatus.Sold;
        }

        public void MakeAvailable()
        {
            Status = SeatStatus.Available;
        }

        protected Seat() { }
    }
}