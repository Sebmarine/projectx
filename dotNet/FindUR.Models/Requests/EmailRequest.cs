using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests
{
    public class EmailRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(6),MaxLength(100)]
        public string From { get; set; }
        [Required]
        [MinLength(2),MaxLength(100)]
        public string Subject { get; set; }
        [Required]
        [MinLength(2)]
        public string PlainTextContent { get; set; }
        [Required]
        [MinLength(2)]
        public string HtmlContent { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(6),MaxLength(100)]
        public string To { get; set; }
    }
}
