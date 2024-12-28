namespace control_reserve_back_end.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
