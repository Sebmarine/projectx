using Sabio.Data.Providers;
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Requests.Surveys;
using Sabio.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using Sabio.Models.Domain.Users;
using Sabio.Models.Domain;
using Sabio.Services.Interfaces;

namespace Sabio.Services
{
    public class SurveyInstanceService : ISurveyInstanceService
    {
        private IDataProvider _data = null;
        private ILookUpService _lookup = null;

        public SurveyInstanceService(IDataProvider data, ILookUpService lookup)
        {
            _data = data;
            _lookup = lookup;
        }

        #region ---GET---

        public SurveyInstance GetSurveyInstancesById(int id)
        {
            string procName = "[dbo].[SurveysInstances_SelectById]";
            SurveyInstance surveyAnswer = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    surveyAnswer = MapSingleSurveyInstance(reader, ref startingIndex);
                });

            return surveyAnswer;
        }
        public Paged<SurveyInstance> GetSurveyInstancePaginated(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[SurveysInstances_SelectAll]";

            Paged<SurveyInstance> pagedList = null;
            List<SurveyInstance> list = null;
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
                    SurveyInstance suverAnswer = MapSingleSurveyInstance(reader, ref i);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(i++);
                    }
                    if (list == null)
                    {
                        list = new List<SurveyInstance>();

                    }
                    list.Add(suverAnswer);
                });
            if (list != null)
            {
                pagedList = new Paged<SurveyInstance>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<SurveyInstance> SurveyInstanceCreatedBy(int pageIndex, int pageSize, int query)
        {
            Paged<SurveyInstance> pagedList = null;
            List<SurveyInstance> list = null;
            int totalCount = 0;
            string procName = "[dbo].[SurveysInstances_Select_ByCreatedBy]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
                paramCollection.AddWithValue("@Query", query);
            },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    SurveyInstance suverAnswer = MapSingleSurveyInstance(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (list == null)
                    {
                        list = new List<SurveyInstance>();
                    }
                    list.Add(suverAnswer);
                });
            if (list != null)
            {
                pagedList = new Paged<SurveyInstance>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }


        #endregion

        #region ---POST&PUTT---
        public int AddSurveyInstance(SurveysInstancesAddRequest model)
        {
            int id = 0;

            string procName = "[dbo].[SurveysInstances_Insert]";

            _data.ExecuteNonQuery(procName
            , inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonSurveyInstanceParams(model, col);

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
        public void UpdateSurveyInstances(SurveysInstancesUpdateRequest model)
        {
            string procName = "[dbo].[SurveysInstances_Update]";
            _data.ExecuteNonQuery(procName
            , inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonSurveyInstanceParams(model, col);
                col.AddWithValue("@Id", model.Id);
            }
            , returnParameters: null);
        }
        public SurveyInstanceDetailed GetInstanceDetailed(int id)
        {
            string procName = "dbo.SurveyInstances_SelectById_Detail";
            SurveyInstanceDetailed instanceDetailed = new SurveyInstanceDetailed();
            int i = 0;

            _data.ExecuteCmd(procName,
                delegate (SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@Id", id);
                },
                delegate (IDataReader reader, short set)
                {
                    instanceDetailed = MapDetailedParams(reader, ref i);
                });
            return instanceDetailed;
        }
        public Paged<SurveyInstanceDetailed> GetInstancesDetailed(int pageSize, int pageIndex)
        {
            Paged<SurveyInstanceDetailed> pagedInstances = null;
            List<SurveyInstanceDetailed> instances = null;
            int totalCount = 0;

            string procName = "[dbo].[SurveyInstances_SelectAll_Detail]";

            _data.ExecuteCmd(procName,
                delegate(SqlParameterCollection paramCol)
                {
                    paramCol.AddWithValue("@PageSize", pageSize);
                    paramCol.AddWithValue("@PageIndex", pageIndex);
                },
                delegate (IDataReader reader, short set)
                {
                    int i = 0;

                     SurveyInstanceDetailed surveyInstance = MapDetailedParams(reader, ref i); 

                    if (instances == null)
                    {
                        instances = new List<SurveyInstanceDetailed>();
                    }
                    instances.Add(surveyInstance);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(i++);
                    }
                });
            if (instances != null)
            {
                pagedInstances = new Paged<SurveyInstanceDetailed>(instances, pageIndex, pageSize, totalCount);
            }
            return pagedInstances;
        }


        private static SurveyInstanceDetailed MapDetailedParams(IDataReader reader, ref int i)
        {
            SurveyInstanceDetailed instanceDetailed = new SurveyInstanceDetailed();
            BaseUserProfile user = new BaseUserProfile();

            instanceDetailed.Id = reader.GetSafeInt32(i++);
            user.Email = reader.GetSafeString(i++);
            user.FirstName = reader.GetSafeString(i++);
            user.LastName = reader.GetSafeString(i++);
            user.Id = reader.GetSafeInt32(i++);
            user.AvatarUrl = reader.GetSafeString(i++);
            instanceDetailed.CreatedBy = user;
            instanceDetailed.SurveyId = reader.GetSafeInt32(i++);
            instanceDetailed.SurveyName = reader.GetSafeString(i++);
            instanceDetailed.SurveyDescription = reader.GetSafeString(i++);
            instanceDetailed.Questions = reader.DeserializeObject<List<SurveyQuestionAnswer>>(i++);
            instanceDetailed.DateCreated = reader.GetSafeDateTime(i++);

            return instanceDetailed;
        }
        #endregion

        #region ---DELETE---
        public void DeleteSurveyInstance(int id)
        {
            string procName = "[dbo].[SurveysInstances_Delete_ById]";
            _data.ExecuteNonQuery(procName
            , inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }
            , returnParameters: null);
        }
        #endregion

        private  SurveyInstance MapSingleSurveyInstance(IDataReader reader, ref int startingIndex)
        {
            SurveyInstance surveyInstances = new SurveyInstance();
            surveyInstances.Survey = new Survey();
            surveyInstances.Survey.CreatedBy = new BaseUserProfile();
            surveyInstances.Survey.Status = new LookUp();
            surveyInstances.Survey.SurveyType = new LookUp();
            BaseUserProfile user = new BaseUserProfile();


            surveyInstances.Id = reader.GetSafeInt32(startingIndex++);
            surveyInstances.Survey.Id = reader.GetSafeInt32(startingIndex++);
            surveyInstances.Survey.Name = reader.GetSafeString(startingIndex++);
            surveyInstances.Survey.Description = reader.GetSafeString(startingIndex++);
            surveyInstances.Survey.Status = _lookup.MapSingleLookUp(reader, ref startingIndex);
            surveyInstances.Survey.SurveyType = _lookup.MapSingleLookUp(reader, ref startingIndex);
            user.Email = reader.GetSafeString(startingIndex++);
            user.FirstName = reader.GetSafeString(startingIndex++);
            user.LastName = reader.GetSafeString(startingIndex++);
            user.Id = reader.GetSafeInt32(startingIndex++);
            user.AvatarUrl = reader.GetSafeString(startingIndex++);
            surveyInstances.Survey.CreatedBy = user;
            surveyInstances.Survey.DateCreated = reader.GetSafeDateTime(startingIndex++);
            surveyInstances.Survey.DateModified = reader.GetSafeDateTime(startingIndex++);
            surveyInstances.UserId = reader.GetSafeInt32(startingIndex++);
            surveyInstances.DateCreated = reader.GetSafeDateTime(startingIndex++);
            surveyInstances.DateModified = reader.GetSafeDateTime(startingIndex++);
            return surveyInstances;
        }

        private static void AddCommonSurveyInstanceParams(SurveysInstancesAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@InstanceId", model.Survey);
        }
    }
}
