using Sabio.Models;
using Sabio.Models.Domain.NewsletterSubscriptions;
using Sabio.Models.Requests.NewsletterSubscriptions;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface INewsletterSubService
    {
        void Update(NewsSubUpdateRequest model);
        string Add(NewsSubAddRequest model);
        Paged<NewsSub> GetAllPagination(int pageIndex, int pageSize);
        List<NewsSub> GetAllSubscribed();
        Paged<NewsSub> SelectByDate(int pageIndex, int pageSize, string date);
    }
}