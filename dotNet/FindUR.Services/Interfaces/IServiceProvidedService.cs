using Sabio.Models;
using Sabio.Models.Domain.Services;
using Sabio.Models.Requests.Services;

namespace Sabio.Services.Interfaces
{
    public interface IServiceProvidedService
    {
        int InsertService(ServiceProvidedAddRequest model, int userId);
        void UpdateService(ServiceProvidedUpdateRequest model, int userId);
        void DeleteService(int id);
        Service SelectServiceById(int id);
        Paged<Service> SelectServicesByPage(int pageIndex, int pageSize);
        Paged<Service> SelectServicesByCreatedBy(int pageIndex, int pageSize, int userId);
        Paged<Service> SearchCreatedBy(string query, int pageIndex, int pageSize);
        Paged<Service> SelectServicesByPracticeId(int pageIndex, int pageSize, int practiceId);
    }
}

