using control_reserve_back_end.Models;

namespace control_reserve_back_end.Dto.Requests
{
    public class ReservationRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SpaceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
