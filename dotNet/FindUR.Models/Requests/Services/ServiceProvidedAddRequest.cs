using System.ComponentModel.DataAnnotations;

namespace Sabio.Models.Requests.Services
{
    public class ServiceProvidedAddRequest
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int ServiceTypeId { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Length must be between 2 and 50 characters", MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "Length must be between 2 and 250 characters", MinimumLength = 2)]
        public string Description { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive numbers allowed")]
        public decimal Total { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Length must be between 2 and 50 characters", MinimumLength = 2)]
        public string ServiceCode { get; set; }
        [Required]
        public int isActive { get; set; }

    }
}