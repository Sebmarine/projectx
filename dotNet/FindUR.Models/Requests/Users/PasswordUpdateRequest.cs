using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Users
{
    public class PasswordUpdateRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
    ErrorMessage = "Password must contain at least 8 characters with an uppercase letter, lowercase letter, a number, and a symbol.")]
        [MaxLength(100)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [MaxLength(100)]
        public string PasswordConfirm { get; set; }
    }
}
