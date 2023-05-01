using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Stripe
{
    public class InvoiceAddRequest
    {
        [Required]
        [MinLength(2), MaxLength(255)]
        public string CustomerId { get; set; }


        [Required]
        [Range(1, Int32.MaxValue)]
        public int VetId { get; set; }


        [Required]
        [Range(1, Int32.MaxValue)]
        public int UserId { get; set; }

        [Required]
        [MinLength(2), MaxLength(255)]
        public string InvoiceId { get; set; }


        [Required]
        public bool isPaid { get; set; }
        [Required]
        [Range(1, Int32.MaxValue)]
        public int paymentType { get; set; }
        [Required]
        public bool isRefund { get; set; }

        
    }
}
