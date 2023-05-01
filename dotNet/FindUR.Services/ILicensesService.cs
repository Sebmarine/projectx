using Microsoft.AspNetCore.Mvc;
using Sabio.Models;
using Sabio.Models.Domain.Licenses;
using Sabio.Models.Requests.Licenses;
using System.Collections.Generic;

namespace Sabio.Services
{
    public interface ILicensesService
    {
        Paged<License> GetAll(int pageIndex, int pageSize);

        Paged<License> GetCreatedBy(int pageIndex, int pageSize, int userId);

        License Get(int id);

        void Delete(int id);

        int Add(LicenseAddRequest model, int userId);

        void Update(LicenseUpdateRequest model, int userId);
    }
}