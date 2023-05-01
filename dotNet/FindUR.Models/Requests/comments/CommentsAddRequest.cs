using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.comments
{
    public class CommentsAddRequest
    {
        [Required]
        [MinLength(1), MaxLength(50)]
        public string Subject { get; set; }

        [Required]
        [MinLength(1), MaxLength(3000)]
        public string Text { get; set; }

        public int ParentId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int EntityTypeId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int EntityId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CreatedBy { get; set; }

        [Required]
        public byte IsDeleted { get; set; }

        public DateTime DateCreated { get; set; }


    }
}
