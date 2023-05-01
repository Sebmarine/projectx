using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.ExternalLinks
{
    public class ExternalLinksAddRequest
    {
        [Required]
        public List<ExternalLinkUrlAddRequest>  Urls { get; set;}
    }
}
