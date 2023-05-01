using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.SurveyQuestions
{
    public class SurveyQuestionAnswerOptionAddRequest
    {
        [Required]
        public int QuestionId { get; set; }
        [StringLength(500)]
        public string Text { get; set; }
        [StringLength(100)]
        public string Value { get; set; }
        [StringLength(200)]
        public string AdditionalInfo { get; set; }
        //[Required]
        public BaseUserProfile CreatedBy { get; set; }
    }
}
