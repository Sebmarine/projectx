﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.ShoppingCarts
{
    public class ShoppingCartAddRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int InventoryId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
