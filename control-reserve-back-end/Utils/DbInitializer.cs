using control_reserve_back_end.Data;
using control_reserve_back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace control_reserve_back_end.Utils
{
    public class DbInitializer
    {
        private readonly AppDbContext _context;

        public DbInitializer(AppDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            // Asegúrate de que los datos de Spaces estén presentes antes de insertar las reservas
            if (!_context.Spaces.Any())
            {
                var spaces = new List<Space>
        {
            new Space { Name = "Room 1", Description = "A meeting room" },
            new Space { Name = "Room 2", Description = "Conference room" }
        };
                _context.Spaces.AddRange(spaces);
                _context.SaveChanges();  // Guarda los espacios primero
            }

            // Ahora puedes agregar usuarios
            if (!_context.Users.Any())
            {
                var users = new List<User>
        {
            new User { Email = "user1@example.com", Name = "User 1" },
            new User { Email = "user2@example.com", Name = "User 2" }
        };
                _context.Users.AddRange(users);
                _context.SaveChanges();  // Guarda los usuarios
            }

            // Finalmente, agrega las reservas, asegurándote de que SpaceId existe
            if (!_context.Reservations.Any())
            {
                var reservations = new List<Reservation>
        {
            new Reservation
            {
                StartTime = DateTime.Now.ToUniversalTime(),
                EndTime = DateTime.Now.AddHours(1).ToUniversalTime(),
                SpaceId = _context.Spaces.First(s => s.Name == "Room 1").Id,
                UserId = _context.Users.First(u => u.Email == "user1@example.com").Id
            },
            new Reservation
            {
                StartTime = DateTime.Now.AddDays(1).ToUniversalTime(),
                EndTime = DateTime.Now.AddDays(1).AddHours(1).ToUniversalTime(),
                SpaceId = _context.Spaces.First(s => s.Name == "Room 2").Id,
                UserId = _context.Users.First(u => u.Email == "user2@example.com").Id
            }
        };
                _context.Reservations.AddRange(reservations);
                _context.SaveChanges();  // Guarda las reservas
            }
        }
    }
}