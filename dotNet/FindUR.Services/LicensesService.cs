using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain.Comments;
using Sabio.Models;
using Sabio.Models.Domain.faq;
using Sabio.Models.Domain.FAQ;
using Sabio.Models.Domain.Licenses;
using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain;
using System.Data.SqlClient;
using Sabio.Models.Domain.Schedules;
using Sabio.Models.Requests.comments;
using Sabio.Models.Requests.Licenses;

namespace Sabio.Services
{
    public class LicensesService : ILicensesService
    {
        IDataProvider _data = null;
        public LicensesService(IDataProvider data)
        {
            _data = data;
        }

        public Paged<License> GetAll(int pageIndex, int pageSize)
        {
            Paged<License> pagedList = null;
            List<License> list = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[License_SelectAll]",
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                (reader, recordSetIndex) =>
                {
                    int index = 0;
                    License license = MapLicenses(reader, ref index);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }

                    if (list == null)
                    {
                        list = new List<License>();
                    }

                    list.Add(license);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<License>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<License> GetCreatedBy(int pageIndex, int pageSize, int userId)
        {
            Paged<License> pagedList = null;
            List<License> list = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[License_Select_ByCreatedBy]",
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                    col.AddWithValue("@UserId", userId);
                }, delegate
                (IDataReader reader, short set)
                {
                    int index = 0;
                   License license = MapLicenses(reader, ref index);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }

                    if (list == null)
                    {
                        list = new List<License>();
                    }

                    list.Add(license);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<License>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public License Get(int id)
        {
            string procName = "[dbo].[License_Select_ById]";

            License license = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                license = MapLicenses(reader, ref startingIndex);
            });

            return license;
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[License_DeleteById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                },
                returnParameters: null);
        }

        public int Add(LicenseAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[License_Insert]";

            _data.ExecuteNonQuery(procName,
               inputParamMapper: delegate (SqlParameterCollection col)
               {
                   CommonLicenseParams(model, col, userId);
                   col.AddWithValue("@CreatedBy", userId);

                   SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                   idOut.Direction = ParameterDirection.Output;

                   col.Add(idOut);

               }, returnParameters: delegate (SqlParameterCollection returCollection)
               {
                   object oId = returCollection["@Id"].Value;

                   int.TryParse(oId.ToString(), out id);
               });

            return id;
        }

        public void Update(LicenseUpdateRequest model, int userId)
        {
            string procName = "[dbo].[License_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                CommonLicenseParams(model, col, userId);


                col.AddWithValue("@Id", model.Id);

            }, returnParameters: null);
        }

        private static License MapLicenses(IDataReader reader, ref int index)
        {
            License license = new License();
            license.CreatedBy = new User();
            license.LicenseState = new StateLookUp();


            license.Id = reader.GetSafeInt32(index++);
            license.LicenseState.Id = reader.GetSafeInt32(index++);
            license.LicenseState.Code = reader.GetSafeString(index++);
            license.LicenseState.Name = reader.GetSafeString(index++);
            license.LicenseNumber = reader.GetSafeString(index++);
            license.DateExpires = reader.GetSafeDateTime(index++);
            license.CreatedBy.Id = reader.GetSafeInt32(index++);
            license.CreatedBy.FirstName = reader.GetSafeString(index++);
            license.CreatedBy.LastName = reader.GetSafeString(index++);
            license.CreatedBy.Mi = reader.GetSafeString(index++);
            license.CreatedBy.Email = reader.GetSafeString(index++);
            license.CreatedBy.AvatarUrl = reader.GetSafeString(index++);
            license.DateCreated = reader.GetSafeDateTime(index++);


            return license;
        }

        private static void CommonLicenseParams(LicenseAddRequest model, SqlParameterCollection col, int userId)
        {
            col.AddWithValue("@LicenseStateId", model.LicenseStateId);
            col.AddWithValue("@LicenseNumber", model.LicenseNumber);
            col.AddWithValue("@DateExpires", model.DateExpires);
            col.AddWithValue("@ModifiedBy", userId);
        }




    }
}
