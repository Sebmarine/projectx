using Sabio.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Sabio.Models.Domain.Schedules;
using Sabio.Data.Providers;
using Sabio.Data;
using Sabio.Models.Domain;
using Sabio.Services.Interfaces;
using Sabio.Models.Requests.Schedules;

namespace Sabio.Services
{
    public class ScheduleService : IScheduleService
    {
        IDataProvider _data = null;
        public ScheduleService(IDataProvider data)
        {
            _data = data;
        }

        #region -- Schedule
        public Schedule Get(int id)
        {
            string procName = "[dbo].[Schedules_Select_ById]";

            Schedule schedule = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                schedule = MapSingleSchedule(reader, ref startingIndex);
            });

            return schedule;
        }

        public int Add(ScheduleAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Schedules_Insert]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col, userId);
                    col.AddWithValue("@CreatedBy", userId);

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
        public void Update(ScheduleUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Schedules_Update]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col, userId);
                    col.AddWithValue("@Id", model.Id);
                },
                returnParameters: null);
        }
        public Paged<Schedule> Pagination(int pageIndex, int pageSize, int userId)
        {
            string procName = "[dbo].[Schedules_Select_ByCreatedBy]";
            Paged<Schedule> pages = null;
            List<Schedule> result = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
                paramCollection.AddWithValue("@UserId", userId);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;

                Schedule aSchedule = MapSingleSchedule(reader, ref index);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }
                if (result == null)
                {
                    result = new List<Schedule>();
                }
                result.Add(aSchedule);
            }
            );
            if (result != null)
            {
                pages = new Paged<Schedule>(result, pageIndex, pageSize, totalCount);
            }
            return pages;
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Schedules_Delete_ById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                },
                returnParameters: null);
        }

        private static Schedule MapSingleSchedule(IDataReader reader, ref int startingIndex)
        {
            Schedule aSchedule = new Schedule();
            aSchedule.Id = reader.GetSafeInt32(startingIndex++);
            aSchedule.Name = reader.GetSafeString(startingIndex++);
            aSchedule.VetProfileId = reader.GetSafeInt32(startingIndex++);
            aSchedule.CreatedBy = reader.GetSafeInt32(startingIndex++);
            aSchedule.ModifiedBy = reader.GetSafeInt32(startingIndex++);
            aSchedule.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aSchedule.DateModified = reader.GetSafeDateTime(startingIndex++);
            return aSchedule;
        }
        private static void AddCommonParams(ScheduleAddRequest model, SqlParameterCollection col, int userId)
        {
            col.AddWithValue("@VetProfileId", model.VetProfileId);
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@ModifiedBy", userId);
        }
        #endregion

        #region -- Availability

        public ScheduleAvailability GetAvailabilityById(int id)
        {
            string procName = "[dbo].[ScheduleAvailability_Select_ById]";

            ScheduleAvailability scheduleAvailability = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);
                }, singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    scheduleAvailability = MapSingleAvailability(reader, ref startingIndex);
                }
                );
            return scheduleAvailability;
        }

        public ScheduleAvailabilityV2 GetAvailabilityByIdV2(int id)
        {
            string procName = "[dbo].[ScheduleAvailability_Select_ByIdV2]";

            ScheduleAvailabilityV2 scheduleAvailability = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);
                }, singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    scheduleAvailability = MapSingleAvailabilityV2(reader, ref startingIndex);
                }
                );
            return scheduleAvailability;
        }

        public List<ScheduleAvailability> GetAvailabilityByCreatedBy(int createdBy)
        {
            List<ScheduleAvailability> list = null;

            string procName = "[dbo].[ScheduleAvailability_Select_ByCreatedBy]";
            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@CreatedBy", createdBy);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    ScheduleAvailability sched = MapSingleAvailability(reader, ref startingIndex);

                    if (list == null)
                    {
                        list = new List<ScheduleAvailability>();
                    }
                    list.Add(sched);
                });
            return list;
        }

        public List<ScheduleAvailabilityV2> GetAllAvailability()
        {
            List<ScheduleAvailabilityV2> list = null;
            string procName = "[dbo].[ScheduleAvailability_SelectAll]";
            _data.ExecuteCmd(procName,
                inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    ScheduleAvailabilityV2 sched = MapSingleAvailabilityV2(reader, ref startingIndex);

                    if (list == null)
                    {
                        list = new List<ScheduleAvailabilityV2>();
                    }
                    list.Add(sched);
                });
            return list;
        }

        public List<ScheduleAvailabilityV2> GetAllAvailabilityByVet(int vetProfileId)
        {
            List<ScheduleAvailabilityV2> list = null;
            string procName = "[dbo].[ScheduleAvailability_SelectByVetProfileId]";
            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@VetProfileId", vetProfileId);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    ScheduleAvailabilityV2 sched = MapSingleAvailabilityV2(reader, ref startingIndex);

                    if (list == null)
                    {
                        list = new List<ScheduleAvailabilityV2>();
                    }
                    list.Add(sched);
                });
            return list;
        }

        public List<ScheduleAvailability> GetAvailabilityByScheduleId(int scheduleId)
        {
            string procName = "[dbo].[ScheduleAvailability_Select_By_ScheduleId]";
            List<ScheduleAvailability> list = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@ScheduleId", scheduleId);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;

                ScheduleAvailability availability = MapSingleAvailability(reader, ref startingIndex);

                if (list == null)
                {
                    list = new List<ScheduleAvailability>();
                }
                list.Add(availability);
            });
            return list;
        }

        public int AddAvailability(ScheduleAvailabilityAddRequest model, int createdBy)
        {
            int id = 0;

            string procName = "[dbo].[ScheduleAvailability_Insert]";
            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParamsAvailability(model, col, createdBy);
                col.AddWithValue("@CreatedBy", createdBy);

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

        public int AddAvailabilityV2(ScheduleAvailabilityAddRequestV2 model, int createdBy)
        {
            int id = 0;

            string procName = "[dbo].[ScheduleAvailability_InsertV2]";
            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParamsAvailabilityV2(model, col, createdBy);
                col.AddWithValue("@CreatedBy", createdBy);

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

        public void DeleteAvailabilityById(int id)
        {
            string procName = "[dbo].[ScheduleAvailability_Delete_ById]";

            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, returnParameters: null);
        }

        public void DeleteAvailabilityByIdV2(int id)
        {
            string procName = "[dbo].[ScheduleAvailability_Delete_ByIdV2]";

            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, returnParameters: null);
        }

        public void UpdateAvailability(ScheduleAvailabilityUpdateRequest model, int modifiedBy)
        {
            string procName = "[dbo].[ScheduleAvailability_Update]";
            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParamsAvailability(model, col, modifiedBy);

                col.AddWithValue("@Id", model.Id);

            }, returnParameters: null);
        }

        public void UpdateAvailabilityV2(ScheduleAvailabilityUpdateRequestV2 model, int modifiedBy)
        {

            string procName = "[dbo].[ScheduleAvailability_UpdateV2]";
            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParamsAvailabilityV2(model, col, modifiedBy);

                col.AddWithValue("@Id", model.Id);

            }, returnParameters: null);
        }

        private static void AddCommonParamsAvailability(ScheduleAvailabilityAddRequest model, SqlParameterCollection col, int userId)
        {
            col.AddWithValue("@ScheduleId", model.ScheduleId);
            col.AddWithValue("@DayOfWeek", model.DayOfWeek);
            col.AddWithValue("@StartTime", model.StartTime);
            col.AddWithValue("@EndTime", model.EndTime);
            col.AddWithValue("@ModifiedBy", userId);
        }

        private static void AddCommonParamsAvailabilityV2(ScheduleAvailabilityAddRequestV2 model, SqlParameterCollection col, int userId)
        {
            col.AddWithValue("@ScheduleId", model.ScheduleId);
            col.AddWithValue("@DayOfWeek", model.DayOfWeek);
            col.AddWithValue("@StartTime", model.StartTime);
            col.AddWithValue("@EndTime", model.EndTime);
            col.AddWithValue("@ModifiedBy", userId);
            col.AddWithValue("@IsBooked", model.IsBooked);
        }

        private static ScheduleAvailability MapSingleAvailability(IDataReader reader, ref int startingIndex)
        {
            ScheduleAvailability aSched = new ScheduleAvailability();

            aSched.Id = reader.GetSafeInt32(startingIndex++);
            aSched.ScheduleId = reader.GetSafeInt32(startingIndex++);
            aSched.DayOfWeek = new LookUp();
            aSched.DayOfWeek.Id = reader.GetSafeInt32(startingIndex++);
            aSched.DayOfWeek.Name = reader.GetSafeString(startingIndex++);
            aSched.StartTime = (TimeSpan)reader[startingIndex++];
            aSched.EndTime = (TimeSpan)reader[startingIndex++];
            aSched.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aSched.DateModified = reader.GetSafeDateTime(startingIndex++);

            return aSched;
        }

        private static ScheduleAvailabilityV2 MapSingleAvailabilityV2(IDataReader reader, ref int startingIndex)
        {
            ScheduleAvailabilityV2 aSched = new ScheduleAvailabilityV2();

            aSched.Id = reader.GetSafeInt32(startingIndex++);
            aSched.ScheduleId = reader.GetSafeInt32(startingIndex++);
            aSched.DayOfWeek = new LookUp();
            aSched.DayOfWeek.Id = reader.GetSafeInt32(startingIndex++);
            aSched.DayOfWeek.Name = reader.GetSafeString(startingIndex++);
            aSched.StartTime = reader.GetSafeDateTime(startingIndex++);
            aSched.EndTime = reader.GetSafeDateTime(startingIndex++);
            aSched.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aSched.DateModified = reader.GetSafeDateTime(startingIndex++);
            aSched.IsBooked = reader.GetSafeBool(startingIndex++);

            return aSched;
        }

        #endregion
    }
}
