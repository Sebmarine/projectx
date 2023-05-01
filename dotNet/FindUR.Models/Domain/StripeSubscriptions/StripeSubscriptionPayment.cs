using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.StripeSubscriptions
{
    public class StripeSubscriptionPayment
    {
        public int Id { get; set; }
        public string SubscriptionId { get; set; }
        public DateTime DateEnded { get; set; }
        public int Total { get; set; }
    }
}
