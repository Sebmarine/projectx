using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Inventory
{
    public class InventoryAddRequest
    {
        [Required]
        [Range(minimum: 1, int.MaxValue)]
        public int ProductId { get; set; }
        [Required]
        [Range(minimum: 1, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public decimal BasePrice { get; set; }
    }
}
