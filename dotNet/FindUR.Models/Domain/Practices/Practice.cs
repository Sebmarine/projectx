using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.Services;
using Sabio.Models.Domain.File;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Domain.Schedules;
using Sabio.Models.Domain.Users;

namespace Sabio.Models.Domain.Practices
{
    public class Practice
    {
        [Required]
        [Range(1,int.MaxValue)]
        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public List<FileModel> PrimaryImage { get; set; }
        public Location Location { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Fax { get; set; }
        [Required]
        [EmailAddress]
        public string BusinessEmail { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string SiteUrl { get; set; }
        public List<Service> Services { get; set; }
        public List<Schedule> Schedule { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DateModified { get; set; }
        public List<User> CreatedBy { get; set; }
        public List<User> ModifiedBy { get; set; }
        [Required]
        public bool IsActive { get; set; }
      

    }
}
