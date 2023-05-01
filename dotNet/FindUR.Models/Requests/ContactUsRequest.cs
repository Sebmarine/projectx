using SendGrid.Helpers.Mail;
using System.ComponentModel.DataAnnotations;

namespace Sabio.Models.Requests
{
    public class ContactUsRequest
    {
        [Required]
        [MinLength(2),MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2),MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(6),MaxLength(100)]
        public string From { get; set; }
        [Required]
        [MinLength(2)]
        public string Subject { get; set; }
        [Required]
        [MinLength(2)]
        public string Message { get; set; }
    }
}
