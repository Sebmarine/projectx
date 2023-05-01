using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.NewsletterSubscriptions
{
    public class NewsSubAddRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(6)]
        [MaxLength(255)]
        public string Email { get; set; }
    }
}
