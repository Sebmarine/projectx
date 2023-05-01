using System.ComponentModel.DataAnnotations;

namespace Sabio.Models.Requests.Blog
{
    public class BlogUpdateRequest : BlogAddRequest, IModelIdentifier
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
        public int Id { get; set; }

    }
}
