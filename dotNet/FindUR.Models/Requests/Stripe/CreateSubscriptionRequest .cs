using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Stripe
{
    public class CreateSubscriptionRequest
    {
        [Required]
        [MinLength(2), MaxLength(255)]
        public string CustomerId { get; set; }

        [Required]
        [MinLength(2), MaxLength(255)]
        public string PriceId { get; set; }
    }
}
