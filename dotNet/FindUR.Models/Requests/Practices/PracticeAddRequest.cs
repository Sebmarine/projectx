using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EllipticCurve.Utils;

namespace Sabio.Models.Requests.Practices
{
    public class PracticeAddRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int PrimaryImageId { get; set; }

        [Required]
        [MinLength(5)]
        public string LineOne { get; set; }
           
        public string LineTwo { get; set; }
        [Required]
        [MinLength(2)]
        public string City { get; set; }
        [Required]
        [DataType(DataType.PostalCode)]
        public string Zip { get; set; }
        [Required]
        public int StateId { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int LocationTypeId { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Fax { get; set; }
        [Required]      
        [DataType(DataType.EmailAddress)]
        public string BusinessEmail { get; set; }
        [Required]
        [DataType(DataType.Url)]
        public string SiteUrl { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int ScheduleId { get; set; }
        public List<int> VetProfileIds { get; set; }
        

    }
}
