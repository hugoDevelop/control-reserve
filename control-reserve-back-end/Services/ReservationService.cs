using control_reserve_back_end.Data;
using control_reserve_back_end.Dto.Requests;
using control_reserve_back_end.Dto.Responses;
using control_reserve_back_end.Interfaces;
using control_reserve_back_end.Models;
using Microsoft.EntityFrameworkCore;

namespace control_reserve_back_end.Services
{
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _context;

        public ReservationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation> CreateReservation(ReservationRequest reservation)
        {
            // Convertir a UTC
            var startTimeUtc = DateTime.SpecifyKind(reservation.StartTime, DateTimeKind.Utc);
            var endTimeUtc = DateTime.SpecifyKind(reservation.EndTime, DateTimeKind.Utc);

            // Validar que el startTimeUtc no este en el pasado
            var utcNow = DateTime.UtcNow.ToLocalTime();
            var utcNowWithoutSeconds = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, 0, DateTimeKind.Utc);
            if (startTimeUtc < utcNowWithoutSeconds)
                throw new Exception("No se puede reservar en el pasado.");

            // Validar que el endTimeUtc sea mayor al startTimeUtc
            if (endTimeUtc <= startTimeUtc)
                throw new Exception("La hora de fin debe ser mayor a la hora de inicio.");

            // Validar que el endTimeUtc no sea mayor a 30 días del startTimeUtc
            if (endTimeUtc > startTimeUtc.AddDays(30))
                throw new Exception("La reserva no puede durar más de 30 días.");

            // Validar que no haya solapamientos
            bool overlaps = await _context.Reservations
                .AnyAsync(r => r.SpaceId == reservation.SpaceId &&
                               r.StartTime < endTimeUtc &&
                               startTimeUtc < r.EndTime);

            if (overlaps)
                throw new Exception("El espacio ya está reservado en este horario.");

            // Validar que el usuario no tenga otra reserva en el mismo horario
            bool userHasOtherReservation = await _context.Reservations
                .AnyAsync(r => r.UserId == reservation.UserId &&
                               r.StartTime < endTimeUtc &&
                               startTimeUtc < r.EndTime);

            if (userHasOtherReservation)
                throw new Exception("El usuario ya tiene otra reserva en este horario.");

            // Validar tiempos mínimos y máximos
            TimeSpan duration = endTimeUtc - startTimeUtc;
            if (duration < TimeSpan.FromMinutes(30) || duration > TimeSpan.FromDays(30))
                throw new Exception("La duración de la reserva debe ser entre 30 minutos y 30 días.");

            var reservationToCreate = new Reservation
            {
                UserId = reservation.UserId,
                SpaceId = reservation.SpaceId,
                StartTime = startTimeUtc,
                EndTime = endTimeUtc
            };

            _context.Reservations.Add(reservationToCreate);
            await _context.SaveChangesAsync();

            return reservationToCreate;
        }

        public async Task CancelReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
                throw new Exception("Reserva no encontrada.");

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ReservationResponse>> GetReservations(int? spaceId, int? userId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Reservations.AsQueryable();

            if (spaceId.HasValue)
                query = query.Where(r => r.SpaceId == spaceId.Value);

            if (userId.HasValue)
                query = query.Where(r => r.UserId == userId.Value);

            if (startDate.HasValue)
                query = query.Where(r => r.StartTime >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(r => r.EndTime <= endDate.Value);

            return await query
                .Select(r => new ReservationResponse
                {
                    Id = r.Id,
                    StartTime = r.StartTime,
                    EndTime = r.EndTime,
                    Space = r.Space.Name,
                    User = r.User.Name
                })
                .ToListAsync();
        }

        public async Task<List<SpacesResponse>> GetSpaces()
        {
            return await _context.Spaces
                .Select(s => new SpacesResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                })
                .ToListAsync();
        }

        public async Task<List<UsersResponse>> GetUsers()
        {
            return await _context.Users
                .Select(u => new UsersResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                })
                .ToListAsync();
        }

        public async Task<Reservation> UpdateReservation(ReservationRequest reservation)
        {
            var reservationToUpdate = await _context.Reservations.FindAsync(reservation.Id);
            if (reservationToUpdate == null)
                throw new Exception("Reserva no encontrada.");

            // Convertir a UTC
            var startTimeUtc = DateTime.SpecifyKind(reservation.StartTime, DateTimeKind.Utc);
            var endTimeUtc = DateTime.SpecifyKind(reservation.EndTime, DateTimeKind.Utc);

            // Validar que el startTimeUtc no este en el pasado
            var utcNow = DateTime.UtcNow;
            var utcNowWithoutSeconds = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, utcNow.Hour, utcNow.Minute, 0, DateTimeKind.Utc);
            if (startTimeUtc < utcNowWithoutSeconds)
                throw new Exception("No se puede reservar en el pasado.");

            // Validar que el endTimeUtc sea mayor al startTimeUtc
            if (endTimeUtc <= startTimeUtc)
                throw new Exception("La hora de fin debe ser mayor a la hora de inicio.");

            // Validar que el endTimeUtc no sea mayor a 30 días del startTimeUtc
            if (endTimeUtc > startTimeUtc.AddDays(30))
                throw new Exception("La reserva no puede durar más de 30 días.");

            // Validar que no haya solapamientos
            bool overlaps = await _context.Reservations
                .AnyAsync(r => r.Id != reservation.Id &&
                               r.SpaceId == reservation.SpaceId &&
                               r.StartTime < endTimeUtc &&
                               startTimeUtc < r.EndTime);
            if (overlaps)
                throw new Exception("El espacio ya está reservado en este horario.");

            // Validar que el usuario no tenga otra reserva en el mismo horario
            bool userHasOtherReservation = await _context.Reservations
                .AnyAsync(r => r.UserId == reservation.UserId &&
                               r.StartTime < endTimeUtc &&
                               startTimeUtc < r.EndTime &&
                               r.Id != reservation.Id);

            if (userHasOtherReservation)
                throw new Exception("El usuario ya tiene otra reserva en este horario.");

            // Validar tiempos mínimos y máximos
            TimeSpan duration = endTimeUtc - startTimeUtc;
            if (duration < TimeSpan.FromMinutes(30) || duration > TimeSpan.FromHours(8))
                throw new Exception("La duración de la reserva debe ser entre 30 minutos y 8 horas.");

            reservationToUpdate.UserId = reservation.UserId;
            reservationToUpdate.SpaceId = reservation.SpaceId;
            reservationToUpdate.StartTime = startTimeUtc;
            reservationToUpdate.EndTime = endTimeUtc;
            await _context.SaveChangesAsync();
            return reservationToUpdate;
        }
    }

}
