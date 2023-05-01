using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Inventory;
using Sabio.Models.Requests.Inventory;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/inventory")]
    [ApiController]
    public class InventoryApiController : BaseApiController
    {
        private IInventoryService _service = null;
        private IAuthenticationService<int> _authService = null;

        public InventoryApiController(IInventoryService service, 
            IAuthenticationService<int> authService,
            ILogger<InventoryApiController> logger) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        #region --- GETS ---
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Inventory>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Inventory inventory = _service.GetById(id);

                if (inventory == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Inventory>() { Item = inventory };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Inventory>>> GetAllPaginated(int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<Inventory> page = _service.GetAllPaginated(pageIndex, pageSize);

                if (page == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Inventory>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);
        }

        [HttpGet("paginate/byuser")]
        public ActionResult<ItemResponse<Paged<Inventory>>> GetByCreatedBy(int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                Paged<Inventory> page = _service.GetByCreatedBy(pageIndex, pageSize, userId);

                if (page == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Inventory>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);
        }

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Inventory>>> GetSearchPaginated(int pageIndex, int pageSize, string searchTerm)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<Inventory> page = _service.GetBySearch(pageIndex, pageSize, searchTerm);

                if (page == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Inventory>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);
        }

        [HttpGet("vendor/{vendorId:int}")]
        public ActionResult<ItemResponse<Paged<Inventory>>> GetByVendor(int pageIndex, int pageSize, int vendorId)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<Inventory> page = _service.GetByVendorId(pageIndex, pageSize, vendorId);

                if (page == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Inventory>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);
        }

        [HttpGet("product/{productId:int}")]
        public ActionResult<ItemResponse<Paged<Inventory>>> GetByProduct(int pageIndex, int pageSize, int productId)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<Inventory> page = _service.GetByProductId(pageIndex, pageSize, productId);

                if (page == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Inventory>> { Item = page };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);
        }
        #endregion

        #region --- POST & PUT ---
        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(InventoryAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                int newId = _service.Add(model, userId);

                ItemResponse<int> response = new ItemResponse<int>() { Item = newId };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                base.Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(InventoryUpdateRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                _service.Update(model, userId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(iCode, response);
        }

        [HttpPut("quantity/{id:int}")]
        public ActionResult<ItemResponse<int>> UpdateQuantity(InventoryUpdateQuantityRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                _service.UpdateQuantity(model, userId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(iCode, response);
        }
        #endregion

        #region --- DELETE ---
        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.DeleteById(id, userId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(iCode, response);
        }
        #endregion
    }
}
