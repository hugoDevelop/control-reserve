using control_reserve_back_end.Data;
using control_reserve_back_end.Dto.Requests;
using control_reserve_back_end.Dto.Responses;
using control_reserve_back_end.Interfaces;
using control_reserve_back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace control_reserve_back_end.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUserAsync(CreateUserRequest createUserRequest)
        {
            createUserRequest.Name = createUserRequest.Name.Trim();
            createUserRequest.Email = createUserRequest.Email.Trim();
            if (string.IsNullOrEmpty(createUserRequest.Name) || string.IsNullOrEmpty(createUserRequest.Email))
            {
                throw new ArgumentException("Nombre y/o Email no pueden estar vacíos");
            }
            // verify that the user does not already exist
            var existingUserByEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == createUserRequest.Email.ToLower());
            if (existingUserByEmail != null)
            {
                throw new ArgumentException($"El usuario con el email '{createUserRequest.Email}' ya existe");
            }

            var existingUserByName = await _context.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == createUserRequest.Name.ToLower());
            if (existingUserByName != null)
            {
                throw new ArgumentException($"El usuario con el nombre '{createUserRequest.Name}' ya existe");
            }

            var user = new User
            {
                Name = createUserRequest.Name,
                Email = createUserRequest.Email
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new ArgumentException("Usuario no encontrado");
            }
            // verify that the user is not being used in a reservation
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.UserId == id);
            if (reservation != null)
            {
                throw new ArgumentException("El usuario está siendo usado en una reserva");
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UsersResponse>> GetAllUsersAsync()
        {
            return await _context.Users.Select(u => new UsersResponse
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            }).ToListAsync();
        }

        public async Task<User> UpdateUserAsync(UpdateUserRequest updateUserRequest)
        {
            updateUserRequest.Name = updateUserRequest.Name.Trim();
            updateUserRequest.Email = updateUserRequest.Email.Trim();
            if (string.IsNullOrEmpty(updateUserRequest.Name) || string.IsNullOrEmpty(updateUserRequest.Email))
            {
                throw new ArgumentException("Nombre y/o Email no pueden estar vacíos");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == updateUserRequest.Id);
            if (user == null)
            {
                throw new ArgumentException("Usuario no encontrado");
            }
            // verify that the user does not already exist
            var existingUserByEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == updateUserRequest.Email.ToLower() && u.Id != updateUserRequest.Id);
            if (existingUserByEmail != null && existingUserByEmail.Id != updateUserRequest.Id)
            {
                throw new ArgumentException($"El usuario con el email '{updateUserRequest.Email}' ya existe");
            }

            var existingUserByName = await _context.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == updateUserRequest.Name.ToLower() && u.Id != updateUserRequest.Id);
            if (existingUserByName != null && existingUserByName.Id != updateUserRequest.Id)
            {
                throw new ArgumentException($"El usuario con el nombre '{updateUserRequest.Name}' ya existe");
            }

            user.Name = updateUserRequest.Name;
            user.Email = updateUserRequest.Email;
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
