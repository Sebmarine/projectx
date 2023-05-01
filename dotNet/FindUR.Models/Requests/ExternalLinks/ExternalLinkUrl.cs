using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.ExternalLinks
{
    public class ExternalLinkUrlAddRequest
    {

        [Required]
        [Range(1, Int32.MaxValue)]
        public int UrlTypeId { get; set; }

        [Required]
        [DataType(DataType.Url)]
        [MinLength(2), MaxLength(255)]
        public string Url { get; set; }


        [Required]
        [Range(1, Int32.MaxValue)]
        public int EntityId { get; set; }

        [Required]
        [Range(1, Int32.MaxValue)]
        public int EntityTypeId { get; set; }
    }
}
