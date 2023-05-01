using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Likes
{
    public class LikeUpdateRequest
    {
        [Required]
        public bool IsLiked { get; set; }
    }
}