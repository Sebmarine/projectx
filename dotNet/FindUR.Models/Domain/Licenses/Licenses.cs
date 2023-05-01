using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Licenses
{
    public class License
    {
        public int Id { get; set; }
        public StateLookUp LicenseState { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime DateExpires { get; set; }
        public User CreatedBy { get; set; }
        public User ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
      
    }
}
