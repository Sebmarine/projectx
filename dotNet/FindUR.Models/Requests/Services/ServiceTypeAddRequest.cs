using System.ComponentModel.DataAnnotations;

namespace Sabio.Models.Requests.Services
{
    public class ServiceTypeAddRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "Length must be between 2 and 50 characters", MinimumLength = 2)]
        public string Name { get; set; }
    }
}
