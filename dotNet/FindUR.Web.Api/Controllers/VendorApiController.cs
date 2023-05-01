using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Vendors;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using Sabio.Services.Interfaces;
using Sabio.Models.Requests.Vendors;
using Sabio.Models;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/vendors")]
    [ApiController]
    public class VendorApiController : BaseApiController
    {
        private IVendorService _service = null;
        private IAuthenticationService<int> _authService = null;
        private ILogger _logger;
        public VendorApiController(IVendorService service
            , ILogger<VendorApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _logger = logger;
            _service = service;
            _authService = authService;
        }

        #region ADD
        [HttpPost("")]
        public ActionResult<ItemResponse<int>> Add(VendorAddRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                iCode = 201;
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);
                response = new ItemResponse<int>() { Item = id };
            }
            catch (Exception ex)
            {
                iCode = 500;
                _logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: ${ex.Message}");
            }

            return StatusCode(iCode, response);
        }
        #endregion

        #region UPDATE
        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(VendorUpdateRequest model, int userId)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.Update(model, userId);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic error: {ex.Message}");
            }
            return StatusCode(iCode, response);
        }
        #endregion

        #region SELECTALL
        [HttpGet]
        public ActionResult<ItemsResponse<SimpleVendor>> GetAll()
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<SimpleVendor> list = _service.SelectAllV2();

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<SimpleVendor> { Items = list };
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


        #region SELECT ALL PAGED
        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Vendor>>> SelectAll(int pageSize, int pageIndex)
        {
            int iCode = 200;
            BaseResponse response = null;

            try 
            {
                Paged<Vendor> vendors = _service.SelectAll(pageSize, pageIndex);

                if (vendors == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<Vendor>> { Item = vendors };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.ToString());
                base.Logger.LogError(ex.Message);
            }

            return StatusCode(iCode, response);
        }
        #endregion

        #region SELECT ALL ACTIVE PAGED
        [HttpGet("paginate/active")]
        public ActionResult<ItemResponse<Paged<Vendor>>> SelectAllActive(int pageSize, int pageIndex)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<Vendor> vendors = _service.SelectAllActive(pageSize, pageIndex);

                if (vendors == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<Vendor>> { Item = vendors };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.ToString());
                base.Logger.LogError(ex.Message);
            }

            return StatusCode(iCode, response);
        }
        #endregion

        #region SEARCH
        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Vendor>>> Query(string query, int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null; 

            try
            {
                Paged<Vendor> page = _service.Query(pageIndex, pageSize, query);

                if (page == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Vendor>> { Item = page };
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

        #region SELECTBYID
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Vendor>> SelectById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Vendor vendor = _service.SelectById(id);

                if (vendor == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource Not Found");
                }
                else
                {
                    response = new ItemResponse<Vendor> { Item = vendor };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.ToString());
                base.Logger.LogError(ex.Message);
            }

            return StatusCode(iCode, response);
        }
        #endregion

        #region CREATEDBY PAGED
        [HttpGet("createdby/{createdBy:int}")]
        public ActionResult<ItemResponse<Paged<Vendor>>> SelectByCreatedBy(int pageIndex, int pageSize, int createdBy)
        {
   
            ActionResult result = null;

            try
            {
                Paged<Vendor> paged = _service.SelectByCreatedBy(pageIndex, pageSize, createdBy);
                if (paged == null)
                {
                    result = StatusCode(404, new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Vendor>> response = new ItemResponse<Paged<Vendor>>();
                    response.Item = paged;
                    result = StatusCode(200, response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result = StatusCode(500, new ErrorResponse(ex.Message.ToString()));
            }
            return result;
        }
        #endregion
        
        #region DELETE
        [HttpPut("deactivate/{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id, int userId)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id, userId);
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

        #region SELECTALLFORM
        [HttpGet("form")]
        public ActionResult<ItemsResponse<FormVendor>> GetAllForm()
        {
            int iCode = 200;
            BaseResponse response = null;

            try 
            {
                List<FormVendor> list = _service.SelectAllForm();

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemsResponse<FormVendor> { Items = list };
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

    }
}
