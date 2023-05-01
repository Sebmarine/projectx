using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Inventory
{
    public class InventoryUpdateRequest: InventoryAddRequest, IModelIdentifier
    {
        public int Id { get; set; }
    }
}
