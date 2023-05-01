using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Surveys
{
    public class SurveyAddRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(2000)]
        public string Description { get; set; }
        [Required]
        [Range(0, 4)]
        public int StatusId { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int SurveyTypeId { get; set; }
    }
}
