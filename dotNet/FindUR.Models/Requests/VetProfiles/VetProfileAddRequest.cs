using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.VetProfiles
{
    public class VetProfileAddRequest
    {
        [Required]
        [StringLength(1000, MinimumLength = 2)]
        public string Bio { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 2)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string BusinessEmail { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string EmergencyLine { get; set; }
    }
}
