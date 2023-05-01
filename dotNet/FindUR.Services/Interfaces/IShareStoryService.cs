using Sabio.Models;
using Sabio.Models.Domain.ShareStory;
using Sabio.Models.Requests.ShareStory;

namespace Sabio.Services.Interfaces
{
    public interface IShareStoryService
    {
        int Create(StoryAddRequest model, int userId);
        void Delete(int id, int userId);
        Story GetById(int id);
        Paged<Story> GetPaginated(int pageIndex, int pageSize);
        void Update(StoryUpdateRequest model, int userId);
        void UpdateApproval(int id, int userId, bool isApproved);
    }
}