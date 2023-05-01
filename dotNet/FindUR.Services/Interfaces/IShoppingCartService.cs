using Sabio.Models;
using Sabio.Models.Domain.ShoppingCart;
using Sabio.Models.Requests.ShoppingCarts;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Paged<ShoppingCart> GetAll(int pageIndex, int pageSize);

        ShoppingCart GetById(int id);

        public List<ShoppingCart> GetCreatedBy(int userId);

        int Add(ShoppingCartAddRequest model, int userId);

        void Update(ShoppingCartUpdateRequest model, int userId);

        void Delete(int id);
    }
}