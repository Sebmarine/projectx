using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Products
{
    public class ProductUpdateRequest: ProductAddRequest, IModelIdentifier
    {
        public int Id { get; set; }
        
    }
}
