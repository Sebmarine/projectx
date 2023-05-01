using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Resources;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class ResourceService : IResourceService
    {
        IDataProvider _data = null;

        public ResourceService(IDataProvider data)
        {
            _data = data;
        }

        #region --- GETS ---

        public Resource GetResourceById(int id)
        {
            string procName = "[dbo].[Resources_Select_ById]";
            Resource resource = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    resource = MapSingleResource(reader, ref startingIndex);
                });
            return resource;
        }

        public Paged<Resource> GetResources(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Resources_SelectAll]";
            Paged<Resource> pagedList = null;
            List<Resource> resourceList = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);

                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Resource resource = MapSingleResource(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (resourceList == null)
                    {
                        resourceList = new List<Resource>();
                    }
                    resourceList.Add(resource);


                });

            if (resourceList != null)
            {
                pagedList = new Paged<Resource>(resourceList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Resource> GetResourcesByCreatedBy(int userId, int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Resources_Select_ByCreatedBy]";
            Paged<Resource> pagedList = null;
            List<Resource> resourceList = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@UserId", userId);
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);

                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Resource resource = MapSingleResource(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (resourceList == null)
                    {
                        resourceList = new List<Resource>();
                    }
                    resourceList.Add(resource);


                });

            if (resourceList != null)
            {
                pagedList = new Paged<Resource>(resourceList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }


        #endregion

        #region --- POST & PUT ---

        public int AddResource(ResourceAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Resources_Insert]";

            _data.ExecuteNonQuery(procName
           , inputParamMapper: delegate (SqlParameterCollection col)
           {
               AddCommonParams(model,col);
               col.AddWithValue("@UserId", userId);

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

        public void UpdateResource(ResourceUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Resources_Update]";
            _data.ExecuteNonQuery(procName
            , inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@UserId", userId);
                col.AddWithValue("@Id", model.Id);
            }
            , returnParameters: null);
        }

        #endregion

        #region --- DELETE ---
        public void DeleteResource(int id, int userId)
        {
            string procName = "[dbo].[Resources_Delete_ById]";
            _data.ExecuteNonQuery(procName
            , inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }
            , returnParameters: null);
        }

        #endregion

        private static Resource MapSingleResource(IDataReader reader, ref int startingIndex)
        {
            Resource resource = new Resource();
            resource.ResourceType = new LookUp();

            resource.Id = reader.GetSafeInt32(startingIndex++);
            resource.Title = reader.GetSafeString(startingIndex++);
            resource.Subject = reader.GetSafeString(startingIndex++);
            resource.Description = reader.GetSafeString(startingIndex++);
            resource.Url = reader.GetSafeString(startingIndex++);
            resource.Duration = reader.GetDataTypeName(startingIndex++);
            resource.ResourceType.Id = reader.GetSafeInt32(startingIndex++);
            resource.ResourceType.Name = reader.GetSafeString(startingIndex++);
            resource.CoverImageUrl = reader.GetSafeString(startingIndex++);
            resource.DateCreated = reader.GetSafeDateTime(startingIndex++);
            resource.DateModified = reader.GetSafeDateTime(startingIndex++);
            resource.CreatedBy = reader.GetSafeInt32(startingIndex++);
            resource.ModifiedBy = reader.GetSafeInt32(startingIndex++);

            return resource;
        }
        private static void AddCommonParams(ResourceAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Title", model.Title);
            col.AddWithValue("@Subject", model.Subject);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@Url", model.Url);
            col.AddWithValue("@Duration", model.Duration);
            col.AddWithValue("@ResourceTypeId", model.ResourceTypeId);
            col.AddWithValue("@CoverImageUrl", model.CoverImageUrl);            
        }

    }
}
