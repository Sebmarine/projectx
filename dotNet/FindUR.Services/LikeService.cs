using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain.Likes;
using Sabio.Models.Requests.Likes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class LikeService : ILikeService
    {
        IDataProvider _data = null;

        public LikeService(IDataProvider data)        {
            _data = data;
        }

        public void DeleteLike(int entityId, int entityTypeId, int userId)
        {
            string procName = "[dbo].[Likes_Delete]";

            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection col)
            {
                AddCommonParams(entityId, entityTypeId, userId, col);

            });
        }


        public void AddLike(LikeAddRequest model, int userId)
        {
            string procName = "[dbo].[Likes_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model.EntityId, model.EntityTypeId, userId, col);
                col.AddWithValue("@IsLiked", model.IsLiked);

            },
            returnParameters: null
            );

        }

        public List<Like> GetByEntityId_EntityTypeId(int entityId, int entityTypeId)
        {
            string procName = "[dbo].[Likes_SelectByEntityId_EntityTypeId]";

            List<Like> list = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@EntityId", entityId);
                paramCollection.AddWithValue("@EntityTypeId", entityTypeId);

            }, delegate (IDataReader reader, short set)
            {
                Like like = new Like();

                int start = 0;

                start = SingleLikeMapper(reader, like, start);

                if (list == null)
                {
                    list = new List<Like>();
                }
                list.Add(like);
            });
            return list;
        }

        public void UpdateLikeSatus(LikeUpdateRequest model, int entityTypeId, int entityId, int userId)
        {
            string procName = "[dbo].[Likes_UpdateStatus]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(entityId, entityTypeId, userId, col);

                col.AddWithValue("@IsLiked", model.IsLiked);

            },
            returnParameters: null);
        }

        private static int SingleLikeMapper(IDataReader reader, Like like, int start)
        {
            like.EntityId = reader.GetSafeInt32(start++);
            like.EntityTypeId = reader.GetSafeInt32(start++);
            like.UserId = reader.GetSafeInt32(start++);
            like.DateCreated = reader.GetSafeDateTime(start++);
            like.IsLiked = reader.GetSafeBool(start++);
            return start;
        }

        private static void AddCommonParams(int entityId, int entityTypeId, int userId, SqlParameterCollection col)
        {
            col.AddWithValue("@EntityId", entityId);
            col.AddWithValue("@EntityTypeId", entityTypeId);
            col.AddWithValue("@UserId", userId);
        }
    }
}
