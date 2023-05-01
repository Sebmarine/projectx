using Sabio.Models;
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Requests.Surveys;

namespace Sabio.Services
{
    public interface ISurveyService
    {
        Survey GetSurveyById(int id);
        Paged<Survey> GetSurveysPaginated(int pageSize, int pageIndex);
        Paged<Survey> GetSurveyByCreator(int creatorId, int pageSize, int pageIndex);
        int InsertSurvey(SurveyAddRequest request, int userId);
        void UpdateSurvey(SurveyUpdateRequest request);
        void DeleteSurvey(int surveyId);
    };
}