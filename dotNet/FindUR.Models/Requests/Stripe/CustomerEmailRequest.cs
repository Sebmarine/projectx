﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Stripe
{
    public class CustomerEmailRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(3)]
        [MaxLength(255)]
        public string Email { get; set; }

        
    }
}