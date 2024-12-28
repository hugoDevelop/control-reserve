using control_reserve_back_end.Data;
using control_reserve_back_end.Dto.Requests;
using control_reserve_back_end.Dto.Responses;
using control_reserve_back_end.Interfaces;
using control_reserve_back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace control_reserve_back_end.Services
{
    public class SpaceService : ISpaceService
    {
        private readonly AppDbContext _context;

        public SpaceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Space> CreateSpaceAsync(CreateSpaceRequest createSpaceRequest)
        {
            createSpaceRequest.Name = createSpaceRequest.Name.Trim();
            createSpaceRequest.Description = createSpaceRequest.Description.Trim();

            if (string.IsNullOrEmpty(createSpaceRequest.Name) || string.IsNullOrEmpty(createSpaceRequest.Description))
            {
                throw new ArgumentException("Nombre y/o Descripción no pueden estar vacíos");
            }
            // verify that the space does not already exist
            var existingSpace = await _context.Spaces.FirstOrDefaultAsync(s => s.Name.ToLower() == createSpaceRequest.Name.ToLower());
            if (existingSpace != null)
            {
                throw new ArgumentException($"El espacio con el nombre '{createSpaceRequest.Name}' ya existe");
            }

            var space = new Space
            {
                Name = createSpaceRequest.Name,
                Description = createSpaceRequest.Description
            };
            await _context.Spaces.AddAsync(space);
            await _context.SaveChangesAsync();
            return space;
        }

        public async Task DeleteSpaceAsync(int id)
        {
            var space = await _context.Spaces.FirstOrDefaultAsync(s => s.Id == id);
            if (space == null)
            {
                throw new ArgumentException("Espacio no encontrado");
            }
            // verify that the space is not being used in a reservation
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.SpaceId == id);
            if (reservation != null)
            {
                throw new ArgumentException("El espacio está siendo usado en una reserva");
            }

            _context.Spaces.Remove(space);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SpacesResponse>> GetAllSpacesAsync()
        {
            return await _context.Spaces
                .Select(s => new SpacesResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description
                })
                .ToListAsync();
        }

        public async Task<Space> UpdateSpaceAsync(UpdateSpaceRequest updateSpaceRequest)
        {
            updateSpaceRequest.Name = updateSpaceRequest.Name.Trim();
            updateSpaceRequest.Description = updateSpaceRequest.Description.Trim();
            if (string.IsNullOrEmpty(updateSpaceRequest.Name) || string.IsNullOrEmpty(updateSpaceRequest.Description))
            {
                throw new ArgumentException("Nombre y/o Descripción no pueden estar vacíos");
            }
            var space = await _context.Spaces.FirstOrDefaultAsync(s => s.Id == updateSpaceRequest.Id);
            if (space == null)
            {
                throw new ArgumentException("Espacio no encontrado");
            }

            // verify that the space does not already exist
            var existingSpace = await _context.Spaces.FirstOrDefaultAsync(s => s.Name.ToLower() == updateSpaceRequest.Name.ToLower() && s.Id != updateSpaceRequest.Id);
            if (existingSpace != null)
            {
                throw new ArgumentException($"El espacio con el nombre '{updateSpaceRequest.Name}' ya existe");
            }

            space.Name = updateSpaceRequest.Name;
            space.Description = updateSpaceRequest.Description;
            await _context.SaveChangesAsync();
            return space;
        }
    }
}
