using Sabio.Models;
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Requests.Surveys;

namespace Sabio.Services
{
    public interface ISurveyAnswersService
    {
        int AddSurveyAnswer(SurveyAnswerAddRequest model, int currentUserId);
        void DeleteSurveyAnswers(int id);
        SurveyAnswers GetSurveyAnswersById(int id);
        Paged<SurveyAnswers> GetSurveyAnswersPaginated(int pageIndex, int pageSize);
        Paged<SurveyAnswers> SurveyAnswersCreatedBy(int pageIndex, int pageSize, int query);
        void UpdateSurveyAnswers(ServeyAnswerUpdateRequest model, int currentUserId);
    }
}