using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.FAQ
{
    public class FAQAddRequest
    {
        [Required]
        public string Question { get; set; }
        [Required]
        public string Answer { get; set; }
        [Required]
        [Range(1, Int32.MaxValue)]
        public int CategoryId { get; set; }
        [Required, Range(1, Int32.MaxValue)]
        public int SortOrder { get; set; }
       
        public int CreatedBy  { get; set; }
      
        public int ModifiedBy { get; set; }
    }
}
