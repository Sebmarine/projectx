using Sabio.Models;
using Sabio.Models.Domain.Inventory;
using Sabio.Models.Requests.Inventory;

namespace Sabio.Services
{
    public interface IInventoryService
    {
        int Add(InventoryAddRequest model, int userId);
        void DeleteById(int id, int userId);
        Paged<Inventory> GetAllPaginated(int pageIndex, int pageSize);
        Paged<Inventory> GetByCreatedBy(int pageIndex, int pageSize, int userId);
        Inventory GetById(int id);
        Paged<Inventory> GetByProductId(int pageIndex, int pageSize, int productId);
        Paged<Inventory> GetBySearch(int pageIndex, int pageSize, string searchTerm);
        Paged<Inventory> GetByVendorId(int pageIndex, int pageSize, int vendorId);
        void Update(InventoryUpdateRequest model, int userId);
        void UpdateQuantity(InventoryUpdateQuantityRequest model, int userId);
    }
}