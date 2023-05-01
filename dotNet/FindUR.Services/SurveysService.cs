using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.Surveys;
using System.Data.SqlClient;
using System.Data;
using Sabio.Data;
using Microsoft.AspNetCore.Http;
using Sabio.Models;
using Sabio.Models.Requests.Surveys;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Users;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class SurveyService : ISurveyService
    {
        private IDataProvider _data;
        private ILookUpService _lookup = null;

        public SurveyService(IDataProvider data, ILookUpService lookup)
        {
            _lookup = lookup;
            _data = data;
        }
        public Survey GetSurveyById(int id)
        {
            Survey survey = null;

            string procName = "[dbo].[Surveys_GetById]";

            _data.ExecuteCmd(procName,
                delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@Id", id);
                },
                delegate (IDataReader reader, short set)
                {
                    int i = 0;
                    survey = MapCommonParams(reader, ref  i);
                });
            return survey;
        }
        public Paged<Survey> GetSurveyByCreator(int creatorId, int pageSize, int pageIndex)
        {
            Paged<Survey> pagedSurvey = null;
            List<Survey> surveys = null;
            int totalCount = 0;

            string procName = "[dbo].[Surveys_SelectCreatedBy]";

            _data.ExecuteCmd(procName,
                delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@pageSize", pageSize);
                    paramCol.AddWithValue("@pageIndex", pageIndex);
                    paramCol.AddWithValue("@query", creatorId);
                },
                delegate (IDataReader reader, short set)
                {
                    int i = 0;

                    Survey survey = MapCommonParams(reader, ref i); 

                    if (surveys == null)
                    {
                        surveys = new List<Survey>();
                    }
                    surveys.Add(survey);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(i++);
                    }
                });
            if (surveys != null)
            {
                pagedSurvey = new Paged<Survey>(surveys, pageIndex, pageSize, totalCount);
            }
            return pagedSurvey;
        }
        
        public Paged<Survey> GetSurveysPaginated(int pageSize, int pageIndex)
        {
            Paged<Survey> pagedSurvey = null;
            List<Survey> surveys = null;
            int totalCount = 0;

            string procName = "[dbo].[Surveys_SelectAll]";

            _data.ExecuteCmd(procName,
                delegate(SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@PageSize", pageSize);
                    paramCol.AddWithValue("@PageIndex", pageIndex);
                },
                delegate (IDataReader reader, short set)
                {
                    int i = 0;

                    Survey survey = MapCommonParams(reader, ref i); 

                    if (surveys == null)
                    {
                        surveys = new List<Survey>();
                    }
                    surveys.Add(survey);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(i++);
                    }
                });
            if (surveys != null)
            {
                pagedSurvey = new Paged<Survey>(surveys, pageIndex, pageSize, totalCount);
            }
            return pagedSurvey;
        }
        public int InsertSurvey(SurveyAddRequest request, int userId)
        {
            string procName = "[dbo].[Surveys_Insert]";
            int id = 0;

            _data.ExecuteNonQuery(procName,
                delegate (SqlParameterCollection paramCol)
                {
                    AddCommonParams(request, paramCol);
                    paramCol.AddWithValue("@CreatedBy", userId);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    paramCol.Add(idOut);

                },
                returnParameters: delegate (SqlParameterCollection returnParams)
                {
                    object oId = returnParams["@Id"].Value;
                    int.TryParse(oId.ToString(), out id);
                });
            return id;
        }
        public void UpdateSurvey(SurveyUpdateRequest request) 
        {
            string procName = "[dbo].[Surveys_Update]";

            _data.ExecuteNonQuery(procName,
                delegate (SqlParameterCollection paramCol)
                {
                    AddCommonParams(request, paramCol);
                    paramCol.AddWithValue("@Id", request.Id);
                });
        }
        public void DeleteSurvey(int surveyId)
        {
            string procName = "[dbo].[Surveys_DeleteById]";

            _data.ExecuteNonQuery(procName,
                delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@Id", surveyId);
                });
        }
        // aSQ.QuestionTypeId = _lookUpService.MapSingleLookUp(reader, ref startingIndex);


        private Survey MapCommonParams(IDataReader reader, ref int i)
        {
            Survey survey = new Survey();
            BaseUserProfile user = new BaseUserProfile();

            survey.Id = reader.GetSafeInt32(i++);
            survey.Name = reader.GetSafeString(i++);
            survey.Description = reader.GetSafeString(i++);
            survey.Status = _lookup.MapSingleLookUp(reader, ref i);
            survey.SurveyType = _lookup.MapSingleLookUp(reader, ref i);
            user.Email = reader.GetSafeString(i++);
            user.FirstName = reader.GetSafeString(i++);
            user.LastName = reader.GetSafeString(i++);
            user.Id = reader.GetSafeInt32(i++);
            user.AvatarUrl = reader.GetSafeString(i++);
            survey.CreatedBy = user;
            survey.DateCreated = reader.GetSafeDateTime(i++);
            survey.DateModified = reader.GetSafeDateTime(i++);

            return survey;
        }
        private static void AddCommonParams(SurveyAddRequest request, SqlParameterCollection paramCol)
        {
            paramCol.AddWithValue("@Name", request.Name);
            paramCol.AddWithValue("@Description", request.Description);
            paramCol.AddWithValue("@StatusId", request.StatusId);
            paramCol.AddWithValue("@SurveyTypeId", request.SurveyTypeId);
        }
    }
}
