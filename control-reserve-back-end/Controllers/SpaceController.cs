using control_reserve_back_end.Dto.Requests;
using control_reserve_back_end.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace control_reserve_back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpaceController : ControllerBase
    {
        private readonly ISpaceService _service;
        public SpaceController(ISpaceService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSpaceRequest space)
        {
            try
            {
                var createdSpace = await _service.CreateSpaceAsync(space);
                return CreatedAtAction(nameof(Create), new { id = createdSpace.Id }, createdSpace);
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
                await _service.DeleteSpaceAsync(id);
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
            var spaces = await _service.GetAllSpacesAsync();
            return Ok(spaces);
        }
        [HttpPut]
        public async Task<IActionResult> Update(UpdateSpaceRequest space)
        {
            try
            {
                var updatedSpace = await _service.UpdateSpaceAsync(space);
                return Ok(updatedSpace);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
