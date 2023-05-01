using Sabio.Models;
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Requests.Surveys;

namespace Sabio.Services
{
    public interface ISurveyInstanceService
    {
        int AddSurveyInstance(SurveysInstancesAddRequest model);
        void DeleteSurveyInstance(int id);
        Paged<SurveyInstance> GetSurveyInstancePaginated(int pageIndex, int pageSize);
        SurveyInstance GetSurveyInstancesById(int id);
        Paged<SurveyInstance> SurveyInstanceCreatedBy(int pageIndex, int pageSize, int query);
        void UpdateSurveyInstances(SurveysInstancesUpdateRequest model);
        SurveyInstanceDetailed GetInstanceDetailed(int id);
        Paged<SurveyInstanceDetailed> GetInstancesDetailed(int pageSize, int pageIndex);
    }
}