using control_reserve_back_end.Dto.Requests;
using control_reserve_back_end.Interfaces;
using control_reserve_back_end.Models;
using control_reserve_back_end.Services;
using Microsoft.AspNetCore.Mvc;

namespace control_reserve_back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationsController(IReservationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationRequest reservation)
        {
            try
            {
                var createdReservation = await _service.CreateReservation(reservation);
                return CreatedAtAction(nameof(Create), new { id = createdReservation.Id }, createdReservation);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _service.CancelReservation(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? spaceId, [FromQuery] int? userId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var reservations = await _service.GetReservations(spaceId, userId, startDate, endDate);
            return Ok(reservations);
        }

        [HttpGet("spaces")]
        public async Task<IActionResult> GetSpaces()
        {
            var spaces = await _service.GetSpaces();
            return Ok(spaces);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _service.GetUsers();
            return Ok(users);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ReservationRequest reservation)
        {
            try
            {
                var updatedReservation = await _service.UpdateReservation(reservation);
                return Ok(updatedReservation);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
