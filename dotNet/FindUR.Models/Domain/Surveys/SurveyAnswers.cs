using Sabio.Models.Domain.SurveyQuestions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Surveys
{
    public class SurveyAnswers
    {
        public int Id { get; set; }
        public SurveyInstance SurveyInstance { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }
        public int AnswerOptionId { get; set; }
        public string Answer { get; set; }
        public int AnswerNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
