namespace Domain.Entities
{
    public class Passenger
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string MobileNumber { get; private set; }
        public string Email { get; private set; }

        public virtual ICollection<Ticket> Tickets { get; private set; }

        public Passenger(string name, string mobileNumber, string email = "example@gmail.com")
        {
            Id = Guid.NewGuid();
            Name = name;
            MobileNumber = mobileNumber;
            Email = email;
            Tickets = new List<Ticket>();
        }

        protected Passenger() { }
    }
}