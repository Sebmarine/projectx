using Sabio.Data.Providers;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Sabio.Models.Domain.SurveyQuestions;
using Sabio.Data;
using Sabio.Models;
using Sabio.Models.Requests.SurveyQuestions;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class SurveyQuestionService : ISurveyQuestionService
    {
        ILookUpService _lookUpService = null;
        IDataProvider _data = null;
        public SurveyQuestionService(IDataProvider data, ILookUpService lookUpService)
        {
            _data = data;
            _lookUpService = lookUpService;
        }
        public void Delete(int id)
        {
            string procName = "[dbo].[SurveyQuestions_Delete_ById]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);

            }, returnParameters: null);
        }
        public void Update(SurveyQuestionUpdateRequest model, int currentUser)
        {
            string procName = "[dbo].[SurveyQuestions_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@UserId", currentUser);
                AddCommonParams(model, col);
            }, returnParameters: null);
        }
        public int Insert(SurveyQuestionAddRequest model, int currentUser)
        {
            int id = 0;
            string procName = "[dbo].[SurveyQuestions_Insert]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@UserId", currentUser);
                AddCommonParams(model, col);
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                col.Add(idOut);
            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;
                int.TryParse(oId.ToString(), out id);
            });
            return id;
        }
        public Paged<SurveyQuestion> GetAllByPagination(int pageIndex, int pageSize)
        {
            Paged<SurveyQuestion> pagedResult = null;

            List<SurveyQuestion> result = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[SurveyQuestions_SelectAll]",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {

                    SurveyQuestion aSQ = MapSingleSurveyQuestion(reader);

                    int index = 0;
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }


                    if (result == null)
                    {
                        result = new List<SurveyQuestion>();
                    }

                    result.Add(aSQ);
                }

            );
            if (result != null)
            {
                pagedResult = new Paged<SurveyQuestion>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }
        public Paged<SurveyQuestion> GetByUserIdPaginated(int pageIndex, int pageSize, string userId)
        {
            Paged<SurveyQuestion> pagedResult = null;
            List<SurveyQuestion> result = null;
            int totalCount = 0;
            _data.ExecuteCmd(
                "[dbo].[SurveyQuestions_Select_ByCreatedBy]",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                    parameterCollection.AddWithValue("@UserId", userId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    SurveyQuestion aSQ = MapSingleSurveyQuestion(reader);
                    int index = 0;
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }
                    if (result == null)
                    {
                        result = new List<SurveyQuestion>();
                    }
                    result.Add(aSQ);
                }
            );
            if (result != null)
            {
                pagedResult = new Paged<SurveyQuestion>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;
        }
        public SurveyQuestion GetById(int id)
        {
            string procName = "[dbo].[SurveyQuestions_Select_ById]";
            SurveyQuestion sQ = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                sQ = MapSingleSurveyQuestion(reader);
            }
            );
            return sQ;
        }
        private static void AddCommonParams(SurveyQuestionAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Question", model.Question);
            col.AddWithValue("@HelpText", model.HelpText);
            col.AddWithValue("@IsRequired", model.IsRequired);
            col.AddWithValue("@IsMultipleAllowed", model.IsMultipleAllowed);
            col.AddWithValue("@QuestionTypeId", model.QuestionTypeId);
            col.AddWithValue("@SurveyId", model.SurveyId);
            col.AddWithValue("@StatusId", model.StatusId);
            col.AddWithValue("@SortOrder", model.SortOrder);
        }
        private SurveyQuestion MapSingleSurveyQuestion(IDataReader reader)
        {
            SurveyQuestion aSQ = new SurveyQuestion();
            int startingIndex = 0;

            aSQ.Id = reader.GetSafeInt32(startingIndex++);
            aSQ.UserId = reader.GetSafeInt32(startingIndex++);
            aSQ.Question = reader.GetSafeString(startingIndex++);
            aSQ.HelpText = reader.GetSafeString(startingIndex++);
            aSQ.IsRequired = reader.GetSafeBool(startingIndex++);
            aSQ.IsMultipleAllowed = reader.GetSafeBool(startingIndex++);

            aSQ.QuestionTypeId = _lookUpService.MapSingleLookUp(reader, ref startingIndex);

            aSQ.SurveyId = reader.GetSafeInt32(startingIndex++);

            aSQ.SurveyStatusId = _lookUpService.MapSingleLookUp(reader, ref startingIndex);

            aSQ.SortOrder = reader.GetSafeInt32(startingIndex++);
            aSQ.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aSQ.DateModified = reader.GetSafeDateTime(startingIndex++);
            return aSQ;
        }
    }
}
