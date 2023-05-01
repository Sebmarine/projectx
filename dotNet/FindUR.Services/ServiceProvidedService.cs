using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Services;
using Sabio.Models.Requests.Services;
using Sabio.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Services
{
    public class ServiceProvidedService : IServiceProvidedService
    {
        #region -- IDataProvider --
        private IAuthenticationService<int> _authenticationService;
        private IDataProvider _dataProvider;
        public ServiceProvidedService(IAuthenticationService<int> authService, IDataProvider dataProvider)
        {
            _authenticationService = authService;
            _dataProvider = dataProvider;
        }
        #endregion

        #region -- Service Add -- OK
        public int InsertService(ServiceProvidedAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Services_Insert]";
            _dataProvider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col, userId);

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
        #endregion

        #region -- Service Select By Id -- OK
        public Service SelectServiceById(int id)
        {
            string procName = "[dbo].[Services_Select_ById]";
            Service service = null;

            _dataProvider.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                service = MapSingleService(reader, ref startingIndex);
            }
            );
            return service;
        }
        #endregion

        #region - Service Select Pagination -
        public Paged<Service> SelectServicesByPage(int pageIndex, int pageSize)
        {
            Paged<Service> pagedList = null;
            List<Service> list = null;
            int totalCount = 0;

            _dataProvider.ExecuteCmd(
                "dbo.Services_SelectAll",
                (param) =>
                {
                    param.AddWithValue(@"PageIndex", pageIndex);
                    param.AddWithValue(@"PageSize", pageSize);
                },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    Service service = MapSingleService(reader, ref startingIndex);
                    totalCount = reader.GetSafeInt32(startingIndex);

                    if (list == null)
                    {
                        list = new List<Service>();
                    }
                    list.Add(service);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<Service>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        #endregion

        #region -- Service Select By Created By --
        public Paged<Service> SelectServicesByCreatedBy(int pageIndex, int pageSize, int userId)
        {
            string procName = "[dbo].[Services_Select_ByCreatedBy]";
            Paged<Service> pagedList = null;
            List<Service> serviceList = null;
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

                    Service service = MapSingleService(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (serviceList == null)
                    {
                        serviceList = new List<Service>();
                    }
                    serviceList.Add(service);


                });
            if (serviceList != null)
            {
                pagedList = new Paged<Service>(serviceList, pageIndex, pageSize, totalCount);
            };
            return pagedList;
        }
        #endregion

        #region -- Search By Created By --
        public Paged<Service> SearchCreatedBy(string query, int pageIndex, int pageSize)
        {
            Paged<Service> pagedList = null;
            List<Service> serviceList = null;
            int totalCount = 0;
            _dataProvider.ExecuteCmd(
                "[dbo].[Services_Search_ByCreatedBy]",
                inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@PageIndex", pageIndex);
                    paramCollection.AddWithValue("@PageSize", pageSize);
                    paramCollection.AddWithValue("@Query", query);
                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Service service = MapSingleServiceSearch(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (serviceList == null)
                    {
                        serviceList = new List<Service>();
                    }
                    serviceList.Add(service);


                });
            if (serviceList != null)
            {
                pagedList = new Paged<Service>(serviceList, pageIndex, pageSize, totalCount);
            };
            return pagedList;
        }
        #endregion

        #region -- Service Update -- OK 
        public void UpdateService(ServiceProvidedUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Services_Update]";
            _dataProvider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col, userId);
                col.AddWithValue("@Id", model.Id);
            },
            returnParameters: null);
        }
        #endregion

        #region -- Service Delete -- OK 
        public void DeleteService(int id)
        {
            string procName = "[dbo].[Services_Delete_ById]";
            _dataProvider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            },
            returnParameters: null);
        }
        #endregion

        #region -- Service Select By Practice By --
        public Paged<Service> SelectServicesByPracticeId(int pageIndex, int pageSize, int practiceId)
        {
            string procName = "[dbo].[Services_Select_ByPracticeId]";
            Paged<Service> pagedList = null;
            List<Service> serviceList = null;
            int totalCount = 0;
            _dataProvider.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                    col.AddWithValue("@PracticeId", practiceId);

                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Service service = MapSingleService(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (serviceList == null)
                    {
                        serviceList = new List<Service>();
                    }
                    serviceList.Add(service);


                });
            if (serviceList != null)
            {
                pagedList = new Paged<Service>(serviceList, pageIndex, pageSize, totalCount);
            };
            return pagedList;
        }
        #endregion

        #region -- MapSingleService --
        private static Service MapSingleService(IDataReader reader, ref int idx)
        {
            Service service = new Service();
            service.ServiceType = new ServiceType();
            service.CreatedBy = new ServiceUser();

            service.Id = reader.GetSafeInt32(idx++);
            service.ServiceType.Name = reader.GetSafeString(idx++);
            service.Name = reader.GetSafeString(idx++);
            service.Description = reader.GetSafeString(idx++);
            service.Total = reader.GetSafeDecimal(idx++);
            service.ServiceCode = reader.GetSafeString(idx++);
            service.IsActive = reader.GetSafeBool(idx++);
            service.DateCreated = reader.GetSafeDateTime(idx++);
            service.DateModified = reader.GetSafeDateTime(idx++);
            service.CreatedBy.Id = reader.GetSafeInt32(idx++);
            service.ModifiedBy = reader.GetSafeInt32(idx++);

            return service;
        }
        #endregion

        #region -- MapSingleServiceSearch --
        private static Service MapSingleServiceSearch(IDataReader reader, ref int idx)
        {
            Service service = new Service();
            service.ServiceType = new ServiceType();
            service.CreatedBy = new ServiceUser();

            service.Id = reader.GetSafeInt32(idx++);
            service.ServiceType.Name = reader.GetSafeString(idx++);
            service.Name = reader.GetSafeString(idx++);
            service.Description = reader.GetSafeString(idx++);
            service.Total = reader.GetSafeDecimal(idx++);
            service.ServiceCode = reader.GetSafeString(idx++);
            service.IsActive = reader.GetSafeBool(idx++);
            service.DateCreated = reader.GetSafeDateTime(idx++);
            service.DateModified = reader.GetSafeDateTime(idx++);
            service.CreatedBy.Id = reader.GetSafeInt32(idx++);
            service.CreatedBy.FirstName = reader.GetSafeString(idx++);
            service.CreatedBy.LastName = reader.GetSafeString(idx++);
            service.ModifiedBy = reader.GetSafeInt32(idx++);

            return service;
        }
        #endregion

        #region -- AddCommonParams
        private static void AddCommonParams(ServiceProvidedAddRequest model, SqlParameterCollection col, int userId)
        {
            col.AddWithValue("ServiceTypeId", model.ServiceTypeId);
            col.AddWithValue("Name", model.Name);
            col.AddWithValue("Description", model.Description);
            col.AddWithValue("Total", model.Total);
            col.AddWithValue("ServiceCode", model.ServiceCode);
            col.AddWithValue("IsActive", model.isActive);
            col.AddWithValue("@UserId", userId);
        }
        #endregion
    }
}