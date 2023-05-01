using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain.Services;
using Sabio.Models.Requests.Services;
using Sabio.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Services
{
    public class ServiceProvidedTypeService : IServiceProvidedTypeService
    {
        IDataProvider _data = null;
        public ServiceProvidedTypeService(IDataProvider data)
        {
            _data = data;
        }
        public List<ServiceType> SelectAll()
        {
            List<ServiceType> list = null;
            string procName = "[dbo].[ServiceTypes_SelectAll]";

            _data.ExecuteCmd(procName, inputParamMapper: null,
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                ServiceType serviceType = Mapper(reader, ref startingIndex);

                if (list == null)
                {
                    list = new List<ServiceType>();
                }

                list.Add(serviceType);
            });
            return list;
        }
        public List<ServiceType> SelectByDefault(int userId)
        {
            List<ServiceType> list = null;
            string procName = "[dbo].[ServiceTypes_SelectByUserId_SelectAllDefault]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
                           {
                               col.AddWithValue("@UserId", userId);

                           }, delegate (IDataReader reader, short set)
                           {
                               int startingIndex = 0;
                               ServiceType serviceType = Mapper(reader, ref startingIndex);

                               if (list == null)
                               {
                                   list = new List<ServiceType>();
                               }

                               list.Add(serviceType);
                           });
            return list;
        }
        public List<ServiceType> SelectByCreatedBy(int userId)
        {
            List<ServiceType> list = null;
            string procName = "[dbo].[ServiceTypes_SelectByUserId]";

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@UserId", userId);

            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                ServiceType serviceType = Mapper(reader, ref startingIndex);

                if (list == null)
                {
                    list = new List<ServiceType>();
                }

                list.Add(serviceType);
            });
            return list;
        }
        public int InsertServiceType(ServiceTypeAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[ServiceTypes_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
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
        public void Delete(int id)
        {
            string procName = "[dbo].[ServiceTypes_Delete_ById]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            },
            returnParameters: null);
        }
        private static ServiceType Mapper(IDataReader reader, ref int idx)
        {
            ServiceType serviceType = new ServiceType();

            serviceType.Id = reader.GetSafeInt32(idx++);
            serviceType.Name = reader.GetSafeString(idx++);

            return serviceType;
        }
        private static void AddCommonParams(ServiceTypeAddRequest model, SqlParameterCollection col, int userId)
        {
            col.AddWithValue("Name", model.Name);
            col.AddWithValue("@UserId", userId);
        }
    }
}


