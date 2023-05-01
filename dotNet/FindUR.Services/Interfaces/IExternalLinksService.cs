using Sabio.Models.Domain.ExternalLinks;
using Sabio.Models.Requests.ExternalLinks;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface IExternalLinksService
    {
        public int Add(ExternalLinksAddRequest request, int userId);
        public void Update(ExternalLinksUpdateRequest request, int userId);
        public List<ExternalLink> Get(int userId);
        public void Delete(int id, int userId);

    }
}
