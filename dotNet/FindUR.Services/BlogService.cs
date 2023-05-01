using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Blogs;
using Sabio.Models.Requests.Blog;
using Sabio.Services.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Services
{
    public class BlogService : IBlogService
    {
        ILookUpService _lookUpService = null;
        IDataProvider _data = null;

        public BlogService(IDataProvider data, ILookUpService lookUpService)
        {
            _data = data;
            _lookUpService = lookUpService;
        }

        public int Add(BlogAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Blogs_Insert]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection collect)
                {
                    collect.AddWithValue("@AuthorId", userId);
                    AddCommonParams(model, collect);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                    idOut.Direction = ParameterDirection.Output;

                    collect.Add(idOut);
                },
                returnParameters: delegate (SqlParameterCollection returnCollect)
                {
                    object objId = returnCollect["@Id"].Value;

                    int.TryParse(objId.ToString(), out id);
                });
            return id;
        }

        public Blog GetBy(int id)
        {
            Blog blog = null;

            string procName = "[dbo].[Blogs_Select_ById]";

            _data.ExecuteCmd(procName,
            inputParamMapper: delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;

                blog = MapSingleBlog(reader, ref startingIndex);
            }
            );
            return blog;
        }

        public void Update(BlogUpdateRequest model, int userId)
        {

            string procName = "[dbo].[Blogs_Update]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection collect)
                {
                    collect.AddWithValue("@Id", model.Id);
                    collect.AddWithValue("@AuthorId", userId);
                    AddCommonParams(model, collect);
                    

                },
                returnParameters: null);
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Blogs_Delete_ById]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection collect)
                {
                    collect.AddWithValue("@Id", id);
                },
                returnParameters: null);
        }

        public Paged<Blog> GetAll(int pageIndex, int pageSize)
        {
            Paged<Blog> pagedList = null;
            List<Blog> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Blogs_SelectAll]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
            },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    Blog blog = MapSingleBlog(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (list == null)
                    {
                        list = new List<Blog>();
                    }
                    list.Add(blog);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<Blog>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Blog> Search(int pageIndex, int pageSize, string query)
        {
            Paged<Blog> pagedList = null;
            List<Blog> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Blogs_Search]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
                paramCollection.AddWithValue("@Query", query);
            },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    Blog blog = MapSingleBlog(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (list == null)
                    {
                        list = new List<Blog>();
                    }
                    list.Add(blog);
                });
            if (list != null)
            {
                pagedList = new Paged<Blog>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Blog> CreatedBy(int pageIndex, int pageSize, int userId)
        {
            Paged<Blog> pagedList = null;
            List<Blog> list = null;
            int totalCount = 0;
            string procName = "[dbo].[Blogs_Select_ByCreatedBy]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);
                paramCollection.AddWithValue("@UserId", userId);
            },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    Blog blog = MapSingleBlog(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (list == null)
                    {
                        list = new List<Blog>();
                    }
                    list.Add(blog);
                });
            if (list != null)
            {
                pagedList = new Paged<Blog>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public List<Blog> GetBlogType(int id)
        {
            List<Blog> list = null;

            string procName = "[dbo].[Blogs_Select_BlogCategory]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@BlogTypeId", id);
            },
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    Blog blog = MapSingleBlog(reader, ref startingIndex);

                    if (list == null)
                    {
                        list = new List<Blog>();
                    }
                    list.Add(blog);
                });
            return list;
        }
        public List<Blog> GetRecent()
        {
            List<Blog> list = null;

            string procName = "[dbo].[Blogs_SelectRecent]";

            _data.ExecuteCmd(procName, null,
                (reader, recordSetIndex) =>
                {
                    int startingIndex = 0;
                    Blog blog = MapSingleBlog(reader, ref startingIndex);

                    if (list == null)
                    {
                        list = new List<Blog>();
                    }
                    list.Add(blog);
                });
            return list;
        }

        private static Blog MapSingleBlog(IDataReader reader, ref int startingIndex)
        {
            Blog blog = new Blog();

            blog.Id = reader.GetSafeInt32(startingIndex++);
            blog.BlogType = new LookUp();
            blog.BlogType.Id = reader.GetSafeInt32(startingIndex++);
            blog.BlogType.Name = reader.GetSafeString(startingIndex++);
            blog.AuthorId = reader.GetSafeInt32(startingIndex++);
            blog.Title = reader.GetSafeString(startingIndex++);
            blog.Subject = reader.GetSafeString(startingIndex++);
            blog.Content = reader.GetSafeString(startingIndex++);
            blog.IsPublished = reader.GetSafeBool(startingIndex++);
            blog.ImageUrl = reader.GetSafeString(startingIndex++);
            blog.DateCreated = reader.GetSafeDateTime(startingIndex++);
            blog.DateModified = reader.GetSafeDateTime(startingIndex++);
            blog.DatePublish = reader.GetSafeDateTime(startingIndex++);

            return blog;
        }
        private static void AddCommonParams(BlogAddRequest model,
            SqlParameterCollection collect)
        {
            collect.AddWithValue("@BlogTypeId", model.BlogTypeId);
            collect.AddWithValue("@Title", model.Title);
            collect.AddWithValue("@Subject", model.Subject);
            collect.AddWithValue("@Content", model.Content);
            collect.AddWithValue("@IsPublished", model.IsPublished);
            collect.AddWithValue("@ImageUrl", model.ImageUrl);
            collect.AddWithValue("@DatePublish", model.DatePublish);
        }
    }
}
