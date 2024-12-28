using control_reserve_back_end.Dto.Requests;
using control_reserve_back_end.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace control_reserve_back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequest user)
        {
            try
            {
                var createdUser = await _service.CreateUserAsync(user);
                return CreatedAtAction(nameof(Create), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteUserAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _service.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserRequest user)
        {
            try
            {
                var updatedUser = await _service.UpdateUserAsync(user);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
