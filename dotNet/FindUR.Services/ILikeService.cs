using Sabio.Models.Domain.Likes;
using Sabio.Models.Requests.Likes;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface ILikeService
    {
        void DeleteLike(int entityId, int entityTypeId, int userId);
        public void AddLike(LikeAddRequest model, int userId);
        public List<Like> GetByEntityId_EntityTypeId(int entityId, int entityTypeId);
        public void UpdateLikeSatus(LikeUpdateRequest model, int entityTypeId, int entityId, int userId);

    }
}