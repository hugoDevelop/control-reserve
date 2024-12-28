using control_reserve_back_end.Dto.Requests;
using control_reserve_back_end.Dto.Responses;
using control_reserve_back_end.Models;

namespace control_reserve_back_end.Interfaces
{
    public interface ISpaceService
    {
        Task<Space> CreateSpaceAsync(CreateSpaceRequest createSpaceRequest);
        Task<IEnumerable<SpacesResponse>> GetAllSpacesAsync();
        Task<Space> UpdateSpaceAsync(UpdateSpaceRequest updateSpaceRequest);
        Task DeleteSpaceAsync(int id);
    }
}
