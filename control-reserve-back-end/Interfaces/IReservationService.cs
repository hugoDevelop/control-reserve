using control_reserve_back_end.Dto.Requests;
using control_reserve_back_end.Dto.Responses;
using control_reserve_back_end.Models;

namespace control_reserve_back_end.Interfaces
{
    public interface IReservationService
    {
        Task<Reservation> CreateReservation(ReservationRequest reservation);
        Task CancelReservation(int id);
        Task<List<ReservationResponse>> GetReservations(int? spaceId, int? userId, DateTime? startDate, DateTime? endDate);
        Task<List<SpacesResponse>> GetSpaces();
        Task<List<UsersResponse>> GetUsers();
        Task<Reservation> UpdateReservation(ReservationRequest reservation);
    }
}
