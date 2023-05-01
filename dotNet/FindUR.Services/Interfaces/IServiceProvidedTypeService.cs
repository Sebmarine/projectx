using Sabio.Models.Domain.Services;
using Sabio.Models.Requests.Services;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IServiceProvidedTypeService
    {
        List<ServiceType> SelectAll();
        List<ServiceType> SelectByDefault(int userId);
        List<ServiceType> SelectByCreatedBy(int userId);
        int InsertServiceType(ServiceTypeAddRequest model, int userId);
        void Delete(int id);
    }
}

