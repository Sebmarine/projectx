using Sabio.Models.Domain;
using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Licenses
{
    public class LicenseAddRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int LicenseStateId { get; set; }

        [Required]
        [MinLength(1), MaxLength(50)]
        public string LicenseNumber { get; set; }

        public DateTime DateExpires { get; set; }

    }
}
