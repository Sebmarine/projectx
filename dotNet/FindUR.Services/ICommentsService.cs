using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Comments;
using Sabio.Models.Requests.comments;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface ICommentsService
    {
        int Add(CommentsAddRequest model, int userId);
        void Delete(int Id);
        Paged<Comment> GetCreatedBy(int pageIndex, int pageSize, int userId);
        List<Comment> GetByEntity(int entityId, int entityTypeId);
        Paged<Comment> Pagination(int pageIndex, int pageSize);
        void Update(CommentsUpdateRequest model);

        List<Comment> GetWithReplies(int entityId, int entityTypeId);


    }
}