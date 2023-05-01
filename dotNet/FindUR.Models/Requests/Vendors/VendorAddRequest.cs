using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Vendors
{
    public class VendorAddRequest
    {
        [Required]
        [MinLength(1), MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MinLength(1), MaxLength(4000)]
        public string Description { get; set; }

        [Required]
        [MinLength(1), MaxLength(200)]
        public string Headline { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PrimaryImageId { get; set; }

    }
}
