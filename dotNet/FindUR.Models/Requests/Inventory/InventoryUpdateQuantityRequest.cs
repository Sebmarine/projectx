using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Inventory
{
    public class InventoryUpdateQuantityRequest: IModelIdentifier
    {
        public int Id { get; set; }
        [Required]
        [Range(minimum: 1, maximum: 9000)]
        public int Quantity { get; set; }
    }
}
