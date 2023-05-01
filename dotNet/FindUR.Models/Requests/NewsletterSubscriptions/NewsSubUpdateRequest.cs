using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.NewsletterSubscriptions
{
    public class NewsSubUpdateRequest: NewsSubAddRequest
    {
        [Required]
        public bool isSubscribed { get; set; }
    }
}
