using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Surveys
{
    public class SurveyInstanceDetailed
    {
        public int Id { get; set; }
        public BaseUserProfile CreatedBy { get; set; } 
        public int SurveyId { get; set; } 
        public string SurveyName { get; set; }
        public string SurveyDescription { get; set; }
        public List<SurveyQuestionAnswer> Questions { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
