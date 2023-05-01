using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.VetProfiles
{
    public class Practice
    {
        public int PracticeId { get; set; }
        public string PracticeName { get; set; }
        public string PracticePhone { get; set; }
        public string PracticeBusinessEmail { get; set; }
        public string PracticeSiteUrl { get; set; }
    }
}
