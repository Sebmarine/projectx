using Sabio.Models;
using Sabio.Models.Domain.Vendors;
using Sabio.Models.Requests.Vendors;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IVendorService
    {
        int Add(VendorAddRequest model, int userId);
        void Update(VendorUpdateRequest model, int userId);
        void Delete(int id, int userId);
        Paged<Vendor> SelectAll(int pageIndex, int pageSize);
        Paged<Vendor> SelectAllActive(int pageIndex, int pageSize);
        Paged<Vendor> Query(int pageIndex, int pageSize, string query);
        Vendor SelectById(int id);
        Paged<Vendor> SelectByCreatedBy(int pageIndex, int pageSize, int createdBy);
        List<SimpleVendor> SelectAllV2();
        List<FormVendor> SelectAllForm();
    }
}
