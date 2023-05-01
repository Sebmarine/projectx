using Sabio.Models.Domain;
using Sabio.Models.Domain.Surveys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Surveys
{
    public class SurveysInstancesAddRequest
    {
        [Required] 
        public int Survey { get; set; }

    }
}
