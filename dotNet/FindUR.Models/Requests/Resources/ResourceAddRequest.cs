using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Resources
{
    public class ResourceAddRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Title { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Subject { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Description { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Url { get; set; }
        [Required]
        [Range(1, 360)]
        public int Duration { get; set; }
        [Required]
        [Range(minimum: 1, maximum: 100)]
        public int ResourceTypeId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string CoverImageUrl { get; set; }

    }
}
