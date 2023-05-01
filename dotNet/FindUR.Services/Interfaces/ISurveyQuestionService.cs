using Sabio.Models;
using Sabio.Models.Domain.SurveyQuestions;
using Sabio.Models.Requests.SurveyQuestions;

namespace Sabio.Services.Interfaces
{
    public interface ISurveyQuestionService
    {
        void Delete(int id);
        void Update(SurveyQuestionUpdateRequest model, int currentUser);
        int Insert(SurveyQuestionAddRequest model, int currentUser);
        Paged<SurveyQuestion> GetAllByPagination(int pageIndex, int pageSize);
        Paged<SurveyQuestion> GetByUserIdPaginated(int pageIndex, int pageSize, string userId);
        SurveyQuestion GetById(int id);
    }
}