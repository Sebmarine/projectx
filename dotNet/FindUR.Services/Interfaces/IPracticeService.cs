using Sabio.Models;
using Sabio.Models.Domain.Practices;
using Sabio.Models.Domain.Services;
using Sabio.Models.Requests.Practices;
using System.Collections.Generic;
using System.Data;

namespace Sabio.Services.Interfaces
{
    public interface IPracticeService
    {
        int AddPractice(PracticeAddRequest model, int userId);
        Practice GetPracticeById (int id);
        void UpdatePractice(PracticeUpdateRequest model, int userId);
        Paged<Practice> GetPractices(int PageIndex, int PageSize);
        Paged<Practice> GetPracticesByCreatedByPage(int PageIndex, int PageSize, int userId);

        Paged<Practice> GetPracticeBySearch(int PageIndex, int PageSize, string Query);
        void Delete(int id);
        Paged<Practice> GetPracticeBySearchV2(int pageIndex, int pageSize, string query);
        List<Practice> GetPractiveByServiceType(int id);

        Practice MapSinglePractice(IDataReader reader, ref int startingIndex);

    }
}