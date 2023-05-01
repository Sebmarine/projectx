using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Likes
{
    public class LikeAddRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int EntityId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int EntityTypeId { get; set; }
        [Required]
        public bool IsLiked { get; set; }
    }
}