using Sabio.Models;
using Sabio.Models.Domain.Newsfeed;
using Sabio.Models.Requests;
using Sabio.Models.Requests.Newsfeed;

namespace Sabio.Services.Interfaces
{
    public interface INewsfeedService
    {
        int Insert(NewsfeedAddRequest model, int userId);
        public void UpdateToInactive(NewsfeedUpdateToInactiveRequest model);
        public Paged<Newsfeed> GetNewsfeedByPage(int pageIndex, int pageSize);
        public Paged<Newsfeed> GetPagedCreatedBy(int id, int pageIndex, int pageSize);
        public void UpdateNewsfeed(NewsfeedUpdateRequest model, int userId);
        public Newsfeed GetNewsfeedById(int id);
        public Paged<Newsfeed> Search(int pageIndex, int pageSize, string query);
    }
}