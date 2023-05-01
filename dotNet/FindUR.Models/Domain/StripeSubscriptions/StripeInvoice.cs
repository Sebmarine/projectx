using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.StripeSubscriptions
{
    public class StripeInvoice
    {
        public string CustomerId { get; set; }

        public int VetId { get; set; }

        public int Id { get; set; }

        public string Currency { get; set; }

        public string Description { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

       

       

      
    }
}
