using System.ComponentModel.DataAnnotations;

namespace control_reserve_back_end.Dto.Requests
{
    public class CreateSpaceRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
