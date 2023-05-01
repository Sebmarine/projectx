using Sabio.Models;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Requests.Location;
using System.Collections.Generic;
using System.Data;

namespace Sabio.Services.Interfaces
{
    public interface ILocationService
    {
        int Create(LocationAddRequest model, int currentUser);
        Paged<Location> GetByCreatedBy(int id, int pageIndex, int pageSize);
        void Delete(int id);
        void Update(LocationUpdateRequest model, int currentUser);
        List<Location> GetByGeo(int radius, double lat, double lng);
        Location MapSingleLocation(IDataReader reader, ref int startingIndex);

    }
}