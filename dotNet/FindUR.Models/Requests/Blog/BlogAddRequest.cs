using System;
using System.ComponentModel.DataAnnotations;

namespace Sabio.Models.Requests.Blog
{
    public class BlogAddRequest
    {
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int BlogTypeId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Name can not be shorter than 2 characters.")]
        [MaxLength(50, ErrorMessage = "Name can not be longer than 50 characters.")]
        public string Title { get; set; }
        
        [Required]
        [MinLength(2, ErrorMessage = "Name can not be shorter than 2 characters.")]
        [MaxLength(50, ErrorMessage = "Name can not be longer than 50 characters.")]
        public string Subject { get; set; }
        
        [MaxLength(50000, ErrorMessage = "Name can not be longer than 50000 characters.")]
        public string Content { get; set; }

        [Required]
        public bool IsPublished { get; set; }

        [Required]
        [Url]
        [MaxLength(500, ErrorMessage = "Name can not be longer than 500 characters.")]
        public string ImageUrl { get; set; }

        [DataType(DataType.DateTime)]
        public string DatePublish { get; set; }
    }
}
