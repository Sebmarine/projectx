using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.Comments;
using Sabio.Data;
using Sabio.Models;
using Sabio.Models.Requests.comments;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Users;

namespace Sabio.Services
{
    public class CommentsService : ICommentsService
    {
        IDataProvider _data = null;
        public CommentsService(IDataProvider data)
        {
            _data = data;
        }
        public Paged<Comment> GetCreatedBy(int pageIndex, int pageSize, int userId)
        {
            Paged<Comment> pagedList = null;
            List<Comment> list = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[Comments_Select_ByCreatedBy]",
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                    col.AddWithValue("@UserId", userId);
                }, delegate
                (IDataReader reader, short set)
                {
                    int index = 0;
                    Comment comment = CommentMapper(reader, ref index);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }

                    if (list == null)
                    {
                        list = new List<Comment>();
                    }

                    list.Add(comment);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<Comment>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<Comment> Pagination(int pageIndex, int pageSize)
        {
            Paged<Comment> pagedList = null;
            List<Comment> list = null;

            int totalCount = 0;

            _data.ExecuteCmd(
                "[dbo].[Comments_SelectAll]",
                (param) =>
                {
                    param.AddWithValue("@PageIndex", pageIndex);
                    param.AddWithValue("@PageSize", pageSize);
                },
                (reader, recordSetIndex) =>
                {
                    int index = 0;
                    Comment friend = CommentMapper(reader, ref index);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(index++);
                    }

                    if (list == null)
                    {
                        list = new List<Comment>();
                    }

                    list.Add(friend);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<Comment>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public int Add(CommentsAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Comments_Insert]";

            _data.ExecuteNonQuery(procName,
               inputParamMapper: delegate (SqlParameterCollection col)
               {
                   CommonCommentParams(model, col);
                   col.AddWithValue("CreatedBy", userId);

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
        public void Update(CommentsUpdateRequest model)
        {
            string procName = "[dbo].[Comments_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                CommonCommentParams(model, col);
                col.AddWithValue("@CreatedBy", model.CreatedBy);


                col.AddWithValue("@Id", model.Id);

            }, returnParameters: null);
        }
        public void Delete(int Id)
        {
            string procName = "[dbo].[Comments_DeleteById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", Id);
                }, returnParameters: null);

        }

        public List<Comment> GetByEntity(int entityId, int entityTypeId)
        {
            string procName = "[dbo].[Comments_Select_ByEntity_V2]";
            List<Comment> list = null;


            _data.ExecuteCmd(procName, delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@EntityTypeId", entityTypeId);
                parameterCollection.AddWithValue("@EntityId", entityId);


            }, delegate (IDataReader reader, short set)
            {
                int index = 0;

                Comment comment = CommentMapper(reader, ref index);


                if (list == null)
                {
                    list = new List<Comment>();
                }
                list.Add(comment);
            });

            return list;
        }


        public List<Comment> GetWithReplies(int entityId, int entityTypeId)
        {

            List<Comment> list = GetByEntity(entityId, entityTypeId);

            List<Comment>  newList = new List<Comment>();

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (i != j)
                    {
                        if (list[i].Id == list[j].ParentId)
                        {

                            if (list[i].Replies == null)
                            {
                                list[i].Replies = new List<Comment>();
                            }

                            list[i].Replies.Add(list[j]);

                        }
                    }

                }
                if (list[i].ParentId == 0)
                {
                    newList.Add(list[i]);
                }

            }

            return newList;
        }

        private static Comment CommentMapper(IDataReader reader, ref int index)
        {
            Comment comment = new Comment();
            comment.EntityType = new LookUp();
            comment.CreatedBy = new User();

            comment.Id = reader.GetSafeInt32(index++);
            comment.EntityType.Id = reader.GetSafeInt32(index++);
            comment.EntityType.Name = reader.GetSafeString(index++);
            comment.EntityId = reader.GetSafeInt32(index++);
            comment.Subject = reader.GetSafeString(index++);
            comment.Text = reader.GetSafeString(index++);
            comment.ParentId = reader.GetSafeInt32(index++);
            comment.DateCreated = reader.GetSafeDateTime(index++);
            comment.DateModified = reader.GetSafeDateTime(index++);
            comment.IsDeleted = reader.GetSafeBool(index++);
            comment.CreatedBy.Id = reader.GetSafeInt32(index++);
            comment.CreatedBy.FirstName = reader.GetSafeString(index++);
            comment.CreatedBy.LastName = reader.GetSafeString(index++);
            comment.CreatedBy.Mi = reader.GetSafeString(index++);
            comment.CreatedBy.Email = reader.GetSafeString(index++);
            comment.CreatedBy.AvatarUrl = reader.GetSafeString(index++);

            return comment;
        }


        private static void CommonCommentParams(CommentsAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Subject", model.Subject);
            col.AddWithValue("@Text", model.Text);
            col.AddWithValue("@ParentId", model.ParentId);
            col.AddWithValue("@EntityTypeId", model.EntityTypeId);
            col.AddWithValue("@EntityId", model.EntityId);
            col.AddWithValue("@IsDeleted", model.IsDeleted);
        }






    }
}
