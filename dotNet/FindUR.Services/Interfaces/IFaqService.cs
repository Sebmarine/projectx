using Sabio.Models.Domain.FAQ;
using Sabio.Models.Requests.FAQ;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IFAQService
    {
        int Add(FAQAddRequest model);
        void Delete(int id);
        List<FAQ> Get(int CategoryId);
        List<FAQ> GetAll();
        void Update(FAQUpdateRequest model);
    }
}