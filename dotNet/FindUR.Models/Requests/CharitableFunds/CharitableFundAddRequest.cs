using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.CharitableFunds
{
    public class CharitableFundAddRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Url)]
        [MinLength(2), MaxLength(255)]
        public string Url { get; set; } 

    }
}
