using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Products
{
    public class ProductAddRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string SKU { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Manufacturer { get; set; }
        [Required]
        [Range(minimum: 1, maximum: 4000)]
        public int Year { get; set; }
        [Required]
        [StringLength(400, MinimumLength = 2)]
        public string Description { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 2)]
        public string Specifications { get; set; }
        [Required]
        [Range(minimum: 1, maximum: 100)]
        public int ProductTypeId { get; set; }
        [Required]
        [Range(minimum: 1, maximum: 100)]
        public int VendorId { get; set; }
        [Required]
        public bool IsVisible { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 2)]
        public string PrimaryImage { get; set; }
    }
}
