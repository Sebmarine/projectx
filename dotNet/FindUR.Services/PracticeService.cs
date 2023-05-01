using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data.Providers;
using Sabio.Models.Requests.Practices;
using Sabio.Services.Interfaces;
using Sabio.Models.Domain.Practices;
using Sabio.Data;
using Microsoft.AspNetCore.Mvc;
using Sabio.Models;
using EllipticCurve.Utils;
using Sabio.Models.Domain.Services;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Domain.Schedules;
using Sabio.Models.Domain.Users;
using Sabio.Models.Domain.File;
using Newtonsoft.Json;
using System.Xml.Linq;
using SendGrid.Helpers.Mail;
using Sabio.Models.Domain.Blogs;

namespace Sabio.Services
{
    public class PracticeService : IPracticeService
    {
        private IAuthenticationService<int> _authenticationService;
        private IDataProvider _dataProvider;
        private static ILocationService _locationService = null;
        public PracticeService(IAuthenticationService<int> authSerice, 
            IDataProvider dataProvider , ILocationService locationService)
        {
            _authenticationService = authSerice;
            _dataProvider = dataProvider;
            _locationService = locationService;
        }

        public int AddPractice(PracticeAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[Practices_Insert]";
            DataTable myParamValue = null;
            if(model.VetProfileIds !=null)
            {
                myParamValue = MapVetProfileIdsToTable(model.VetProfileIds);
            }
            _dataProvider.ExecuteNonQuery(procName
                , inputParamMapper: delegate (SqlParameterCollection col)
                {
                    

                    AddCommonParams(model, col, userId);
                    col.AddWithValue("@VetProfileIds", myParamValue);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);

                },
                returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;
                    int.TryParse(oId.ToString(), out id);




                });
            return id;
        }
        
        public void UpdatePractice(PracticeUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Practices_Update]";
            _dataProvider.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col, userId);
                    col.AddWithValue("@Id", model.Id);
                }, returnParameters: null);
        }

        public Practice  GetPracticeById(int id)
        {
            string procName = "dbo.Practices_Select_ById";
            Practice practice = null;
            _dataProvider.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);

                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    practice = MapSinglePractice(reader, ref startingIndex);
                    

                });
            return practice;
        }

        public Paged<Practice> GetPractices(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Practices_SelectAll]";
            Paged<Practice> pagedList = null;
            List<Practice> practiceList = null;
            int totalCount = 0;
            _dataProvider.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize); 

                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    
                    Practice practice = MapSinglePractice(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (practiceList == null)
                    {
                        practiceList = new List<Practice>();
                    }
                    practiceList.Add(practice);


                });
            if (practiceList != null)
            {
                pagedList = new Paged<Practice>(practiceList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<Practice> GetPracticesByCreatedByPage(int pageIndex, int pageSize, int userId)
        {
            string procName = "[dbo].[Practices_Select_ByCreatedBy]";
            Paged<Practice> pagedList = null;
            List<Practice> practiceList = null;
            int totalCount = 0;
            _dataProvider.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                    col.AddWithValue("@UserId", userId);

                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Practice practice = MapSinglePractice(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (practiceList == null)
                    {
                        practiceList = new List<Practice>();
                    }
                    practiceList.Add(practice);


                });
            if (practiceList != null)
            {
                pagedList = new Paged<Practice>(practiceList, pageIndex, pageSize, totalCount);
            };
            return pagedList;
        }
        public Paged<Practice> GetPracticeBySearch(int pageIndex, int pageSize, string query)
        {
            string procName = "[dbo].[Practices_Search]";
            Paged<Practice> pagedList = null;
            List<Practice> practiceList = null;
            int totalCount = 0;
            _dataProvider.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                    col.AddWithValue("@Query", query);

                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Practice practice = MapSinglePractice(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (practiceList == null)
                    {
                        practiceList = new List<Practice>();
                    }
                    practiceList.Add(practice);


                });
            if (practiceList != null)
            {
                pagedList = new Paged<Practice>(practiceList, pageIndex, pageSize, totalCount);
            };
            return pagedList;
        }
        public void Delete(int id)
        {
            string procName = "[dbo].[Practices_Delete_ById]";
            _dataProvider.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);

                },
                returnParameters: null);
        }
        public Paged<Practice> GetPracticeBySearchV2(int pageIndex, int pageSize, string query)
        {
            string procName = "[dbo].[Practices_SearchV2]";
            Paged<Practice> pagedList = null;
            List<Practice> practiceList = null;
            int totalCount = 0;
            _dataProvider.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                    col.AddWithValue("@Query", query);

                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Practice practice = MapSinglePractice(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (practiceList == null)
                    {
                        practiceList = new List<Practice>();
                    }
                    practiceList.Add(practice);


                });
            if (practiceList != null)
            {
                pagedList = new Paged<Practice>(practiceList, pageIndex, pageSize, totalCount);
            };
            return pagedList;
        }

        

        public List<Practice> GetPractiveByServiceType(int id)
        {
            List<Practice> list = null;

            string procName = "[dbo].[Practices_ByServiceTypeId]";

            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@ServiceTypeId", id);
            },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    Practice practice = MapSinglePractice(reader, ref startingIndex);

                    if (list == null)
                    {
                        list = new List<Practice>();
                    }
                    list.Add(practice);
                });
            return list;
        }

        private static DataTable MapVetProfileIdsToTable(List<int> idsToMap)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("VetProfileId", typeof(int));
            foreach (int id in idsToMap)
            {
                DataRow dr = dt.NewRow();
                int startingIndex = 0;
                dr.SetField(startingIndex++, id);
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public Practice MapSinglePractice(IDataReader reader, ref int startingIndex)
        {
            Practice practice = new Practice();
            
            practice.Id = reader.GetSafeInt32(startingIndex++);
            practice.Name = reader.GetSafeString(startingIndex++);
            practice.Description = reader.GetSafeString(startingIndex++);
            string primaryImageString = reader.GetString(startingIndex++);
            if (!string.IsNullOrEmpty(primaryImageString))
            {
                practice.PrimaryImage = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileModel>>(primaryImageString);
                
            };
            practice.Location = _locationService.MapSingleLocation(reader,ref startingIndex);
            practice.Phone = reader.GetSafeString(startingIndex++);
            practice.Fax = reader.GetSafeString(startingIndex++);
            practice.BusinessEmail = reader.GetSafeString(startingIndex++);
            practice.SiteUrl = reader.GetSafeString(startingIndex++);
            string serviceString = reader.GetSafeString(startingIndex++);
            if (!string.IsNullOrEmpty(serviceString))
            {
                practice.Services = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Service>>(serviceString);
            };
            string scheduleString = reader.GetSafeString(startingIndex++);
                if (!string.IsNullOrEmpty(scheduleString))
            {
                practice.Schedule = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Schedule>>(scheduleString);
            };
            
            practice.DateCreated = reader.GetSafeDateTime(startingIndex++);
            practice.DateModified = reader.GetSafeDateTime(startingIndex++);
            string createdByString = reader.GetSafeString(startingIndex++);
            if (!string.IsNullOrEmpty(createdByString))
            {
                practice.CreatedBy = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(createdByString);
            };
            string ModifiedByString = reader.GetSafeString(startingIndex++);
            if (!string.IsNullOrEmpty(ModifiedByString))
            {
                practice.ModifiedBy = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(ModifiedByString);
            };
            practice.IsActive = reader.GetSafeBool(startingIndex++);
            
            return practice;
        }
        private static void AddCommonParams(PracticeAddRequest model, SqlParameterCollection col, int userId)
        {
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@PrimaryImageId", model.PrimaryImageId);
            col.AddWithValue("@LineOne", model.LineOne);
            col.AddWithValue("@LineTwo", model.LineTwo);
            col.AddWithValue("@City", model.City);
            col.AddWithValue("@Zip", model.Zip);
            col.AddWithValue("@StateId", model.StateId);
            col.AddWithValue("@Latitude", model.Latitude);
            col.AddWithValue("@Longitude", model.Longitude);
            col.AddWithValue("@LocationTypeId", model.LocationTypeId);
            col.AddWithValue("@Phone", model.Phone);
            col.AddWithValue("@Fax", model.Fax);
            col.AddWithValue("@BusinessEmail", model.BusinessEmail);
            col.AddWithValue("@SiteUrl", model.SiteUrl);
            col.AddWithValue("@ScheduleId", model.ScheduleId);
            col.AddWithValue("@UserId", userId);
        }
    }
}
