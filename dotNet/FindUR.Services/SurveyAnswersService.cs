using System.Collections.Generic;
using System.Data;
using Sabio.Models.Domain.Surveys;
using Sabio.Data;
using System.Data.SqlClient;
using Sabio.Models.Requests.Surveys;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.SurveyQuestions;
using Sabio.Services.Interfaces;
using EllipticCurve;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Users;

namespace Sabio.Services
{
    public class SurveyAnswersService : ISurveyAnswersService
    {
        private IDataProvider _data = null;
        private ILookUpService _lookup = null;

        public SurveyAnswersService(IDataProvider data,ILookUpService lookup)
        {
            _data = data;
            _lookup = lookup;
        }

        #region ---GET---
        public SurveyAnswers GetSurveyAnswersById(int id)
        {
            string procName = "[dbo].[SurveyAnswers_Select_ById]";
            SurveyAnswers surveyAnswer = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    surveyAnswer = MapSingleSurveyAnswers(reader, ref startingIndex);
                });

            return surveyAnswer;
        }

        public Paged<SurveyAnswers> GetSurveyAnswersPaginated(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[SurveyAnswers_SelectAll]";

            Paged<SurveyAnswers> pagedList = null;
            List<SurveyAnswers> list = null;
            int totalCount = 0;

            _data.ExecuteCmd(
                procName,
                inputParamMapper: delegate (SqlParameterCollection coll)
                {
                    coll.AddWithValue("@PageIndex", pageIndex);
                    coll.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int i = 0;
                    SurveyAnswers suverAnswer = MapSingleSurveyAnswers(reader, ref i);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(i++);
                    }
                    if (list == null)
                    {
                        list = new List<SurveyAnswers>();
                    }
                    list.Add(suverAnswer);
                });
            if (list != null)
            {
                pagedList = new Paged<SurveyAnswers>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<SurveyAnswers> SurveyAnswersCreatedBy(int pageIndex, int pageSize, int userId)
        {
            Paged<SurveyAnswers> pagedList = null;
            List<SurveyAnswers> list = null;
            int totalCount = 0;
            string procName = "[dbo].[SurveyAnswers_Select_ByInstanceId]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
                paramCollection.AddWithValue("@Query", userId);
            },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    SurveyAnswers suverAnswer = MapSingleSurveyAnswers(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (list == null)
                    {
                        list = new List<SurveyAnswers>();
                    }
                    list.Add(suverAnswer);
                });
            if (list != null)
            {
                pagedList = new Paged<SurveyAnswers>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        #endregion

        #region ---POST&PUT---
        public int AddSurveyAnswer(SurveyAnswerAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[SurveyAnswers_Insert]";

            _data.ExecuteNonQuery(procName
            , inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonSurveyAnswerParams(model, col);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }
            , returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;

                int.TryParse(oId.ToString(), out id);

            });

            return id;
        }
        public void UpdateSurveyAnswers(ServeyAnswerUpdateRequest model, int userId)
        {
            string procName = "[dbo].[SurveyAnswers_Update]";
            _data.ExecuteNonQuery(procName
            , inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonSurveyAnswerParams(model, col);
                col.AddWithValue("@Id", model.Id);
            }
            , returnParameters: null);
        }

        #endregion

        #region ---DELETE--
        public void DeleteSurveyAnswers(int id)
        {
            string procName = "[dbo].[SurveyAnswers_Delete_ById]";
            _data.ExecuteNonQuery(procName
            , inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }
            , returnParameters: null);
        }
        #endregion

        private SurveyAnswers MapSingleSurveyAnswers(IDataReader reader, ref int startingIndex)
        {
            SurveyAnswers surveyAnswers = new SurveyAnswers();
            surveyAnswers.SurveyInstance = new SurveyInstance();
            surveyAnswers.SurveyInstance.Survey = new Survey();
            surveyAnswers.SurveyQuestion = new SurveyQuestion();
            surveyAnswers.SurveyInstance.Survey.Status = new LookUp();
            surveyAnswers.SurveyInstance.Survey.SurveyType = new LookUp();
            surveyAnswers.SurveyQuestion.QuestionTypeId = new LookUp();
            surveyAnswers.SurveyQuestion.SurveyStatusId = new LookUp();
            BaseUserProfile user = new BaseUserProfile();

            surveyAnswers.Id = reader.GetSafeInt32(startingIndex++);
            surveyAnswers.SurveyInstance.Id = reader.GetSafeInt32(startingIndex++);
            surveyAnswers.SurveyInstance.Survey.Id = reader.GetSafeInt32(startingIndex++);
            surveyAnswers.SurveyInstance.Survey.Name = reader.GetSafeString(startingIndex++);
            surveyAnswers.SurveyInstance.Survey.Description = reader.GetSafeString(startingIndex++);
            surveyAnswers.SurveyInstance.Survey.Status = _lookup.MapSingleLookUp(reader, ref startingIndex);
            surveyAnswers.SurveyInstance.Survey.SurveyType = _lookup.MapSingleLookUp(reader, ref startingIndex);
            user.Email = reader.GetSafeString(startingIndex++);
            user.FirstName = reader.GetSafeString(startingIndex++);
            user.LastName = reader.GetSafeString(startingIndex++);
            user.Id = reader.GetSafeInt32(startingIndex++);
            user.AvatarUrl = reader.GetSafeString(startingIndex++);
            surveyAnswers.SurveyInstance.Survey.CreatedBy = user;
            surveyAnswers.SurveyInstance.Survey.DateCreated = reader.GetSafeDateTime(startingIndex++);
            surveyAnswers.SurveyInstance.Survey.DateModified = reader.GetSafeDateTime(startingIndex++);
            surveyAnswers.SurveyInstance.UserId = reader.GetSafeInt32(startingIndex++);
            surveyAnswers.SurveyInstance.DateCreated = reader.GetSafeDateTime(startingIndex++);
            surveyAnswers.SurveyInstance.DateModified = reader.GetSafeDateTime(startingIndex++);
            surveyAnswers.SurveyQuestion.Id = reader.GetSafeInt32(startingIndex++);
            surveyAnswers.SurveyQuestion.UserId = reader.GetSafeInt32(startingIndex++);
            surveyAnswers.SurveyQuestion.Question = reader.GetSafeString(startingIndex++);
            surveyAnswers.SurveyQuestion.HelpText = reader.GetSafeString(startingIndex++);
            surveyAnswers.SurveyQuestion.IsRequired = reader.GetSafeBool(startingIndex++);
            surveyAnswers.SurveyQuestion.IsMultipleAllowed = reader.GetSafeBool(startingIndex++);
            surveyAnswers.SurveyQuestion.QuestionTypeId = _lookup.MapSingleLookUp(reader, ref startingIndex);
            surveyAnswers.SurveyQuestion.SurveyId = reader.GetSafeInt32(startingIndex++);
            surveyAnswers.SurveyQuestion.SurveyStatusId = _lookup.MapSingleLookUp(reader, ref startingIndex);
            surveyAnswers.SurveyQuestion.SortOrder = reader.GetSafeInt32(startingIndex++);
            surveyAnswers.SurveyQuestion.DateCreated = reader.GetSafeDateTime(startingIndex++);
            surveyAnswers.SurveyQuestion.DateModified = reader.GetSafeDateTime(startingIndex++);
            surveyAnswers.AnswerOptionId = reader.GetSafeInt32(startingIndex++);
            surveyAnswers.Answer = reader.GetSafeString(startingIndex++);
            surveyAnswers.AnswerNumber = reader.GetSafeInt32(startingIndex++);
            surveyAnswers.DateCreated = reader.GetSafeDateTime(startingIndex++);
            surveyAnswers.DateModified = reader.GetSafeDateTime(startingIndex++);
            return surveyAnswers;
        }

        private static void AddCommonSurveyAnswerParams(SurveyAnswerAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@InstanceId", model.SurveyInstance);
            col.AddWithValue("@QuestionId", model.SurveyQuestion);
            col.AddWithValue("@AnswerOptionId", model.AnswerOptionId);
            col.AddWithValue("@Answer", model.Answer);
            col.AddWithValue("@AnswerNumber", model.AnswerNumber);
        }

    }

}
