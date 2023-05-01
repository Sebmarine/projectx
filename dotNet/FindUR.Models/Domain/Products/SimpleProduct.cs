using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Products
{
    public class SimpleProduct
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string PrimaryImage { get; set; }
    }
}
