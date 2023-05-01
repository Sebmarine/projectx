using System.ComponentModel.DataAnnotations;

namespace Sabio.Models.Requests.Services
{
    public class ServiceProvidedUpdateRequest : ServiceProvidedAddRequest, IModelIdentifier
    {
        [Required]
        public int Id { get; set; }
    }
}

