using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface IResourceService
    {
        int AddResource(ResourceAddRequest model, int userId);
        void DeleteResource(int id, int userId);
        Resource GetResourceById(int id);
        Paged<Resource> GetResources(int pageIndex,int pageSize);
        Paged<Resource> GetResourcesByCreatedBy(int id, int pageIndex, int pageSize);
        void UpdateResource(ResourceUpdateRequest model, int userId);        

    }
}
