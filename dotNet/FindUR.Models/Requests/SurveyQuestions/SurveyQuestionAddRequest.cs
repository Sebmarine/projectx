using Sabio.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.SurveyQuestions
{
    public class SurveyQuestionAddRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 2)]
        public string Question { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string HelpText { get; set; }
        [Required]
        public bool IsRequired { get; set; }
        [Required]
        public bool IsMultipleAllowed { get; set; }
        [Required]
        public int QuestionTypeId { get; set; }
        [Required]
        public int SurveyId { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required]
        public int SortOrder { get; set; }
    }
}
