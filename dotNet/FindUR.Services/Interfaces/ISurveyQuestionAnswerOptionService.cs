using Sabio.Models;
using Sabio.Models.Domain.SurveyQuestions;
using Sabio.Models.Requests.SurveyQuestions;

namespace Sabio.Services.Interfaces
{
    public interface ISurveyQuestionAnswerOptionService
    {
        void Delete(int id);
        Paged<SurveyQuestionAnswerOption> GetAllByPagination(int pageIndex, int pageSize);
        SurveyQuestionAnswerOption GetById(int id);
        Paged<SurveyQuestionAnswerOption> GetByUserIdPaginated(int pageIndex, int pageSize, string userId);
        int Insert(SurveyQuestionAnswerOptionAddRequest model, int currentUser);
        void Update(SurveyQuestionAnswerOptionUpdateRequest model, int currentUser);
    }
}