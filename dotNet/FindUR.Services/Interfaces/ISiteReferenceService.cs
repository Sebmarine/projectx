using Sabio.Models.Requests.SiteReference;

namespace Sabio.Services.Interfaces
{
    public interface ISiteReferenceService
    {
        void Add(SiteReferenceAddRequest model);
    }
}