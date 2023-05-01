using Sabio.Models.Domain.Users;
using Sabio.Models.Domain.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sabio.Models.Domain.ShoppingCart
{
    public class ShoppingCart
    {
        public int Id { get; set; } 
        public Inventory.Inventory Inventory { get; set; }
        public string PrimaryImage { get; set; }
        public int Quantity { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}

