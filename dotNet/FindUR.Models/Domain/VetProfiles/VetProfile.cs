using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.VetProfiles
{
    public class VetProfile
    {
        public int Id { get; set; }
        public string Bio { get; set; }
        public string Phone { get; set; }
        public string BusinessEmail { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public string EmergencyLine { get; set; }
    }
}
