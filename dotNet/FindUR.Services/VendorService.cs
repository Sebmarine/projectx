using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.File;
using Sabio.Models.Domain.Newsfeed;
using Sabio.Models.Domain.Vendors;
using Sabio.Models.Requests.Newsfeed;
using Sabio.Models.Requests.Vendors;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sabio.Services
{
    public class VendorService : IVendorService
    {
        private IDataProvider _data = null;

        public VendorService(IAuthenticationService<int> authSerice, IDataProvider data)
        {
            _data = data;
        }

        public int Add(VendorAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Vendors_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddVendorParams(model, col);
                col.AddWithValue("@CreatedBy", userId); // is this right?

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

        public void Update(VendorUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Vendors_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddVendorParams(model, col);
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@ModifiedBy", userId);
            },
            returnParameters: null);
        }

        private static void AddVendorParams(VendorAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Name", model.Name);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@Headline", model.Headline);
            col.AddWithValue("@PrimaryImageId", model.PrimaryImageId);
        }
        public void Delete(int id, int userId)
        {
            string procName = "[dbo].[Vendors_Delete]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
                col.AddWithValue("@ModifiedBy", userId);
            },
            returnParameters: null);
        }

        public Paged<Vendor> SelectAll(int pageIndex, int pageSize)
        {
            Paged<Vendor> pagedResult = null;

            List<Vendor> result = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[Vendors_SelectAll]",
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);

                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int index = 0;
                    Vendor vendor = MapSingleVendor(reader, ref index);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }
                    if (result == null)
                    {
                        result = new List<Vendor>();
                    }
                    result.Add(vendor);
                }
              );
            if (result != null)
            {
                pagedResult = new Paged<Vendor>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;

        }


        public Paged<Vendor> SelectAllActive(int pageIndex, int pageSize)
        {
            Paged<Vendor> pagedResult = null;

            List<Vendor> result = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[Vendors_SelectAllActive]",
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);

                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int index = 0;
                    Vendor vendor = MapSingleVendor(reader, ref index);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }
                    if (result == null)
                    {
                        result = new List<Vendor>();
                    }
                    result.Add(vendor);
                }
              );
            if (result != null)
            {
                pagedResult = new Paged<Vendor>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;

        }
        public List<SimpleVendor> SelectAllV2()
        {
            string procName = "[dbo].[Vendors_SelectAllV2]";

            List<SimpleVendor> list = null;

            _data.ExecuteCmd(procName, null
             , delegate (IDataReader reader, short set)
             {
                 int index = 0;
                 SimpleVendor aVendor = MapSingleVendorV2(reader, ref index);

                 if (list == null)
                 {
                     list = new List<SimpleVendor>();
                 }

                 list.Add(aVendor);

             }
            );
            return list;
        }

        public Paged<Vendor> Query(int pageIndex, int pageSize, string query)
        {

            Paged<Vendor> pagedResult = null;

            List<Vendor> result = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[Vendors_Query]",
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Query", query);
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);

                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int index = 0;
                    Vendor vendor = MapSingleVendor(reader, ref index);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }
                    if (result == null)
                    {
                        result = new List<Vendor>();
                    }
                    result.Add(vendor);
                }
              );
            if (result != null)
            {
                pagedResult = new Paged<Vendor>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;

        }

        public Vendor SelectById(int id)
        {
            string procName = "[dbo].[Vendors_SelectById]";
            Vendor vendor = null;
            _data.ExecuteCmd(procName, delegate
            (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int index = 0;
                vendor = MapSingleVendor(reader, ref index);
            }
            );
            return vendor;
        }
        public Paged<Vendor> SelectByCreatedBy(int pageIndex, int pageSize, int createdBy)
        {
            Paged<Vendor> pagedResult = null;

            List<Vendor> result = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[Vendors_SelectByCreatedBy]",
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                    col.AddWithValue("@CreatedBy", createdBy);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int index = 0;
                    Vendor vendor = MapSingleVendor(reader, ref index);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }
                    if (result == null)
                    {
                        result = new List<Vendor>();
                    }
                    result.Add(vendor);
                }
              );
            if (result != null)
            {
                pagedResult = new Paged<Vendor>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;

        }
        private static Vendor MapSingleVendor(IDataReader reader, ref int index)
        {


            Vendor vendor = new Vendor();

            vendor.File = new FileModel();

            vendor.Id = reader.GetSafeInt32(index++);
            vendor.Name = reader.GetSafeString(index++);
            vendor.Description = reader.GetSafeString(index++);
            vendor.Headline = reader.GetSafeString(index++);
            vendor.PrimaryImageId = reader.GetSafeInt32(index++);
            vendor.File.Url = reader.GetSafeString(index++);
            vendor.IsActive = reader.GetSafeBool(index++);
            vendor.DateCreated = reader.GetSafeUtcDateTime(index++);
            vendor.DateModified = reader.GetSafeUtcDateTime(index++);
            vendor.CreatedBy = reader.GetSafeInt32(index++);
            vendor.ModifiedBy = reader.GetSafeInt32(index++);

            return vendor;
        }

        private static SimpleVendor MapSingleVendorV2(IDataReader reader, ref int index)
        {
            SimpleVendor vendor = new SimpleVendor();

            vendor.Id = reader.GetSafeInt32(index++);
            vendor.Name = reader.GetSafeString(index++);
            vendor.Description = reader.GetSafeString(index++);
            vendor.Headline = reader.GetSafeString(index++);

            return vendor;
        }
        public List<FormVendor> SelectAllForm()
        {
            string procName = "[dbo].[Vendors_SelectAllForm]";

            List<FormVendor> list = null;

            _data.ExecuteCmd(procName, null
             , delegate (IDataReader reader, short set)
             {
                 int index = 0;
                 FormVendor aVendor = MapFormVendor(reader, ref index);

                 if (list == null)
                 {
                     list = new List<FormVendor>();
                 }

                 list.Add(aVendor);

             }
            );
            return list;
        }

        private static FormVendor MapFormVendor(IDataReader reader, ref int index)
        {
            FormVendor vendor = new FormVendor();
            vendor.File = new FileModel();

            vendor.Id = reader.GetSafeInt32(index++);
            vendor.Name = reader.GetSafeString(index++);
            vendor.Description = reader.GetSafeString(index++);
            vendor.Headline = reader.GetSafeString(index++);
            vendor.PrimaryImageId = reader.GetSafeInt32(index++);
            vendor.File.Url = reader.GetSafeString(index++);

            return vendor;
        }
    }
}
