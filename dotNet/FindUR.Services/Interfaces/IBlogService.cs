using Sabio.Models;
using Sabio.Models.Domain.Blogs;
using Sabio.Models.Requests.Blog;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IBlogService
    {
        public int Add(BlogAddRequest model, int userId);
        public Blog GetBy(int id);
        public void Update(BlogUpdateRequest model, int userId);
        public void Delete(int id);
        Paged<Blog> GetAll(int pageIndex, int pageSize);
        Paged<Blog> CreatedBy(int pageIndex, int pageSize, int userId);
        Paged<Blog> Search(int pageIndex, int pageSize, string query);
        List<Blog> GetRecent();
        public List<Blog> GetBlogType(int id);
    }
}
