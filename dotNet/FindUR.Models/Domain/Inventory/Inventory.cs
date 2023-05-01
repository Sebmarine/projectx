using Sabio.Models.Domain.Products;
using Sabio.Models.Domain.Vendors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Inventory
{
    public class Inventory
    {
        public int Id { get; set; }
        public SimpleProduct Product { get; set; }
        public int Quantity { get; set; }
        public decimal BasePrice { get; set; }
        public LookUp ProductType { get; set; }
        public SimpleVendor Vendor { get; set; }
        public int VetId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}
