using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Inventory;
using Sabio.Models.Domain.Products;
using Sabio.Models.Domain.Vendors;
using Sabio.Models.Requests.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class InventoryService : IInventoryService
    {
        IDataProvider _data = null;

        public InventoryService(IDataProvider data)
        {
            _data = data;
        }

        #region --- GETS ---
        public Inventory GetById(int id)
        {
            string procName = "dbo.Inventory_Select_ById";
            Inventory inventory = null;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);
                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    inventory = MapSingleInventory(reader, ref startingIndex);
                });

            return inventory;
        }

        public Paged<Inventory> GetAllPaginated(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[Inventory_Select_All]";
            Paged<Inventory> pagedList = null;
            List<Inventory> inventoryList = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);

                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Inventory inventory = MapSingleInventory(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (inventoryList == null)
                    {
                        inventoryList = new List<Inventory>();
                    }
                    inventoryList.Add(inventory);


                });

            if (inventoryList != null)
            {
                pagedList = new Paged<Inventory>(inventoryList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Inventory> GetByCreatedBy(int pageIndex, int pageSize, int userId)
        {
            string procName = "[dbo].[Inventory_Select_ByCreatedBy]";
            Paged<Inventory> pagedList = null;
            List<Inventory> inventoryList = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                    col.AddWithValue("@UserId", userId);

                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Inventory inventory = MapSingleInventory(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (inventoryList == null)
                    {
                        inventoryList = new List<Inventory>();
                    }
                    inventoryList.Add(inventory);
                });

            if (inventoryList != null)
            {
                pagedList = new Paged<Inventory>(inventoryList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Inventory> GetBySearch(int pageIndex, int pageSize, string searchTerm)
        {
            string procName = "[dbo].[Inventory_Select_BySearch]";
            Paged<Inventory> pagedList = null;
            List<Inventory> inventoryList = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                    col.AddWithValue("@SearchTerm", searchTerm);

                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Inventory inventory = MapSingleInventory(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (inventoryList == null)
                    {
                        inventoryList = new List<Inventory>();
                    }
                    inventoryList.Add(inventory);
                });

            if (inventoryList != null)
            {
                pagedList = new Paged<Inventory>(inventoryList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Inventory> GetByProductId(int pageIndex, int pageSize, int productId)
        {
            string procName = "[dbo].[Inventory_Select_ByProductId]";
            Paged<Inventory> pagedList = null;
            List<Inventory> inventoryList = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                    col.AddWithValue("@ProductId", productId);

                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Inventory inventory = MapSingleInventory(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (inventoryList == null)
                    {
                        inventoryList = new List<Inventory>();
                    }
                    inventoryList.Add(inventory);


                });

            if (inventoryList != null)
            {
                pagedList = new Paged<Inventory>(inventoryList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public Paged<Inventory> GetByVendorId(int pageIndex, int pageSize, int vendorId)
        {
            string procName = "[dbo].[Inventory_Select_ByVendorId]";
            Paged<Inventory> pagedList = null;
            List<Inventory> inventoryList = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@PageIndex", pageIndex);
                    col.AddWithValue("@PageSize", pageSize);
                    col.AddWithValue("@VendorId", vendorId);

                }, delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Inventory inventory = MapSingleInventory(reader, ref startingIndex);
                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (inventoryList == null)
                    {
                        inventoryList = new List<Inventory>();
                    }
                    inventoryList.Add(inventory);


                });

            if (inventoryList != null)
            {
                pagedList = new Paged<Inventory>(inventoryList, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        #endregion

        #region --- POST & PUT ---
        public int Add(InventoryAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[Inventory_Insert]";

            _data.ExecuteNonQuery(procName
            , inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@UserId", userId);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }
            , returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;

                int.TryParse(oId.ToString(), out id);

            });

            return id;
        }

        public void Update(InventoryUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Inventory_Update]";
            _data.ExecuteNonQuery(procName
            , inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@UserId", userId);
                col.AddWithValue("@Id", model.Id);
            }
            , returnParameters: null);
        }

        public void UpdateQuantity(InventoryUpdateQuantityRequest model, int userId)
        {
            string procName = "[dbo].[Inventory_Update_Quantity]";
            _data.ExecuteNonQuery(procName
            , inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@UserId", userId);
                col.AddWithValue("@Quantity", model.Quantity);
                col.AddWithValue("@Id", model.Id);
            }
            , returnParameters: null);
        }
        #endregion

        #region --- DELETE ---
        public void DeleteById(int id, int userId)
        {
            string procName = "[dbo].[Inventory_Delete_ById]";
            _data.ExecuteNonQuery(procName
            , inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
                col.AddWithValue("@UserId", userId);
            }
            , returnParameters: null);
        }
        #endregion

        private static Inventory MapSingleInventory(IDataReader reader, ref int startingIndex)
        {
            Inventory inventory = new Inventory();
            inventory.Product = new SimpleProduct();
            inventory.ProductType = new LookUp();
            inventory.Vendor = new SimpleVendor();

            inventory.Id = reader.GetSafeInt32(startingIndex++);
            inventory.Quantity = reader.GetSafeInt32(startingIndex++);
            inventory.BasePrice = reader.GetSafeDecimal(startingIndex++);
            inventory.Product.Id = reader.GetSafeInt32(startingIndex++);
            inventory.Product.Name = reader.GetSafeString(startingIndex++);
            inventory.Product.SKU = reader.GetSafeString(startingIndex++);
            inventory.Product.Year = reader.GetSafeInt32(startingIndex++);
            inventory.Product.Manufacturer = reader.GetSafeString(startingIndex++);
            inventory.Product.Description = reader.GetSafeString(startingIndex++);
            inventory.Product.PrimaryImage = reader.GetSafeString(startingIndex++);
            inventory.ProductType.Id = reader.GetSafeInt32(startingIndex++);
            inventory.ProductType.Name = reader.GetSafeString(startingIndex++);
            inventory.Vendor.Id = reader.GetSafeInt32(startingIndex++);
            inventory.Vendor.Name = reader.GetSafeString(startingIndex++);
            inventory.Vendor.Description = reader.GetSafeString(startingIndex++);
            inventory.Vendor.Headline = reader.GetSafeString(startingIndex++);
            inventory.VetId = reader.GetSafeInt32(startingIndex++);
            inventory.IsActive = reader.GetSafeBool(startingIndex++);
            inventory.DateCreated = reader.GetSafeDateTime(startingIndex++);
            inventory.DateModified = reader.GetSafeDateTime(startingIndex++);
            inventory.CreatedBy = reader.GetSafeInt32(startingIndex++);
            inventory.ModifiedBy = reader.GetSafeInt32(startingIndex++);

            return inventory;
        }

        private static void AddCommonParams(InventoryAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@ProductId", model.ProductId);
            col.AddWithValue("@Quantity", model.Quantity);
            col.AddWithValue("@IsActive", model.IsActive);
            col.AddWithValue("@BasePrice", model.BasePrice);
        }
    }
}
