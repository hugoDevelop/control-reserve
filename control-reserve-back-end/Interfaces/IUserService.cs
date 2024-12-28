using control_reserve_back_end.Dto.Requests;
using control_reserve_back_end.Dto.Responses;
using control_reserve_back_end.Models;

namespace control_reserve_back_end.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(CreateUserRequest createUserRequest);
        Task<IEnumerable<UsersResponse>> GetAllUsersAsync();
        Task<User> UpdateUserAsync(UpdateUserRequest updateUserRequest);
        Task DeleteUserAsync(int id);
    }
}
