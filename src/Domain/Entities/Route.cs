using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Route
    {
        public Guid Id { get; private set; }
        public string FromCity { get; private set; }
        public string ToCity { get; private set; }
        public decimal BasePrice { get; private set; }
        public int DistanceInKm { get; private set; }

        public virtual ICollection<BusSchedule> Schedules { get; private set; }

        public Route(string fromCity, string toCity, decimal basePrice, int distanceInKm)
        {
            Id = Guid.NewGuid();
            FromCity = fromCity;
            ToCity = toCity;
            BasePrice = basePrice;
            DistanceInKm = distanceInKm;
            Schedules = new List<BusSchedule>();
        }

        protected Route() { }
    }
}