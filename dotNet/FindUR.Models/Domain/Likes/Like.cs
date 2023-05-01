using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Likes
{
    public class Like
    {
        public int EntityId { get; set; }
        public int EntityTypeId { get; set; }
        public int UserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsLiked { get; set; }
    }
}
