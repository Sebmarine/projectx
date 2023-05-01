using Sabio.Models.Domain.CharitableFunds;
using Sabio.Models.Requests.CharitableFunds;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface ICharitableFundService
    {
        int Add(CharitableFundAddRequest model, int userId);
        void Delete(int id);
        CharitableFund Get(int id);
        List<CharitableFund> GetAll();
        void Update(CharitableFundUpdateRequest model, int userId);
    }
}