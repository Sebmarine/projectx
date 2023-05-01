using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Stripe
{
    public class StripeSubscription
    {
       
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string CustomerId { get; set; }
        public string PriceId { get; set; }
        public string imageUrl { get; set; }
        public int Total { get; set; }
    }
}
