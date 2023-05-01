using Microsoft.AspNetCore.Mvc;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.Inventory;
using Sabio.Models.Domain.Products;
using Sabio.Models.Domain.ShoppingCart;
using Sabio.Models.Domain.Vendors;
using Sabio.Models.Requests.ShoppingCarts;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sabio.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        IDataProvider _data = null;
        public ShoppingCartService(IDataProvider data)
        {
            _data = data;
        }

        public int Add(ShoppingCartAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[ShoppingCart_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@UserId", userId);
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                col.Add(idOut);
            },
            returnParameters: delegate (SqlParameterCollection returnCol)
            {
                object oId = returnCol["@Id"].Value;
                int.TryParse(oId.ToString(), out id);
            });
            return id;
        }
        
        public Paged<ShoppingCart> GetAll(int pageIndex, int pageSize)
        {
            Paged<ShoppingCart> pagedList = null;
            List<ShoppingCart> list = null;
            int totalCount = 0;
            string procName = "[dbo].[ShoppingCart_Select_All]";

            _data.ExecuteCmd(
                procName
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@PageIndex", pageIndex);
                    paramCollection.AddWithValue("@PageSize", pageSize);
                }
                , singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    ShoppingCart shoppingCart = MapSingleShoppingCart(reader, ref startingIndex);
                    totalCount = reader.GetSafeInt32(startingIndex++);

                    if (list == null)
                    {
                        list = new List<ShoppingCart>();
                    }
                    list.Add(shoppingCart);
                }
                );
            if (list != null)
            {
                pagedList = new Paged<ShoppingCart>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public ShoppingCart GetById(int id)
        {
            string procName = "[dbo].[ShoppingCart_Select_ById]";
            ShoppingCart shoppingCart = null;
            _data.ExecuteCmd
                (
                procName
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);
                }
                , singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    shoppingCart = MapSingleShoppingCart(reader, ref startingIndex);
                }
              );

            return shoppingCart;
        }

        public List<ShoppingCart> GetCreatedBy(int userId )
        {
            string procName = "[dbo].[ShoppingCart_Select_CreatedBy]";
            List<ShoppingCart> shoppingCartList = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@UserId", userId);

                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    ShoppingCart shoppingCart = MapSingleShoppingCart(reader, ref startingIndex);

                    if (shoppingCartList == null)
                    {
                        shoppingCartList = new List<ShoppingCart>();
                    }
                    shoppingCartList.Add(shoppingCart);
                });

            return shoppingCartList;
        }

        public void Update(ShoppingCartUpdateRequest model, int userId)
        {
            string procName = "[dbo].[ShoppingCart_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@UserId", userId);
                col.AddWithValue("@Id", model.Id);

            },
            returnParameters: null);

        }

        public void Delete(int id)
        {
            string procName = "[ShoppingCart_Delete_ById]";
            _data.ExecuteNonQuery
                (
                procName
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);
                }
                ,
                    returnParameters: null
              );

        }

        private static void AddCommonParams(ShoppingCartAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@InventoryId", model.InventoryId);
            col.AddWithValue("@Quantity", model.Quantity);

        }
        private static ShoppingCart MapSingleShoppingCart(IDataReader reader, ref int startingIndex)
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            Inventory inventory = new Inventory();
            SimpleProduct simpleProduct = new SimpleProduct();
            SimpleVendor simpleVendor = new SimpleVendor();

            shoppingCart.Id = reader.GetSafeInt32(startingIndex++);
            inventory.Id = reader.GetSafeInt32(startingIndex++);
            shoppingCart.Quantity = reader.GetSafeInt32(startingIndex++);
            shoppingCart.DateAdded = reader.GetSafeDateTime(startingIndex++);
            shoppingCart.DateModified = reader.GetSafeDateTime(startingIndex++);
            shoppingCart.CreatedBy = reader.GetSafeInt32(startingIndex++);
            shoppingCart.ModifiedBy = reader.GetSafeInt32(startingIndex++);
            simpleProduct.Id = reader.GetSafeInt32(startingIndex++);
            inventory.Quantity = reader.GetSafeInt32(startingIndex++);
            inventory.BasePrice = reader.GetSafeDecimal(startingIndex++);
            inventory.VetId = reader.GetSafeInt32(startingIndex++);
            inventory.IsActive = reader.GetSafeBool(startingIndex++);
            simpleProduct.SKU = reader.GetSafeString(startingIndex++);
            simpleProduct.Name = reader.GetSafeString(startingIndex++);
            simpleProduct.Manufacturer = reader.GetSafeString(startingIndex++);
            simpleProduct.Year = reader.GetSafeInt32(startingIndex++);
            simpleProduct.Description = reader.GetSafeString(startingIndex++);
            shoppingCart.PrimaryImage = reader.GetSafeString(startingIndex++);
            simpleVendor.Id = reader.GetSafeInt32(startingIndex++);
            simpleVendor.Name = reader.GetSafeString(startingIndex++);
            simpleVendor.Description = reader.GetSafeString(startingIndex++);
            simpleVendor.Headline = reader.GetSafeString(startingIndex++);

            inventory.Product = simpleProduct;
            inventory.Vendor = simpleVendor;
            shoppingCart.Inventory = inventory;
            
            return shoppingCart;
        }


    }
}