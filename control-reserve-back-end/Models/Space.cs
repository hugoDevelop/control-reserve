namespace control_reserve_back_end.Models
{
    public class Space
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
