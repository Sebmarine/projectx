using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.CharitableFunds
{
    public class CharitableFund
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; } 
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
    }
}
