namespace control_reserve_back_end.Dto.Responses
{
    public class ReservationResponse
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Space { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
