using Sabio.Models;
using Sabio.Models.Domain.HorseProfiles;
using Sabio.Models.Requests.HorseProfiles;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces.HorseProfiles
{
    public interface IHorseService
    {
        int Create(HorseAddRequest model, int userId);
        void Delete(int id);
        void VetRemove(int id);
        Paged<HorseProfile> GetAll(int pageIndex, int pageSize);
        HorseProfile GetById(int id);
        Paged<HorseProfile> Search(string query, int pageIndex, int pageSize);
        Paged<HorseProfile> SearchByCreatedBy(int userId, int pageIndex, int pageSize);
        Paged<HorseProfile> SearchVetPatients(int userId, int pageIndex, int pageSize);
        void Update(HorseUpdateRequest model, int userId);
        Paged<HorseProfile> GetHorsesByOwnerId(int pageIndex, int pageSize, int ownerId);
    }
}