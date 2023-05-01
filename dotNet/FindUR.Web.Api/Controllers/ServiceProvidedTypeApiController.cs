using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Services;
using Sabio.Models.Requests.Services;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/serviceTypes")]
    [ApiController]
    public class ServiceProvidedTypeApiController : BaseApiController
    {
        private IServiceProvidedTypeService _serviceTypeService = null;
        private IAuthenticationService<int> _authService = null;
        public ServiceProvidedTypeApiController(IServiceProvidedTypeService service
            , ILogger<ServiceProvidedTypeApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _serviceTypeService = service;
            _authService = authService;
        }
        [HttpGet]
        public ActionResult<ItemsResponse<ServiceType>> SelectAll()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<ServiceType> list = _serviceTypeService.SelectAll();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else
                {
                    response = new ItemsResponse<ServiceType> { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
        [HttpGet("{userId:int}")]
        public ActionResult<ItemsResponse<ServiceType>> SelectByDefault(int userId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<ServiceType> list = _serviceTypeService.SelectByDefault(userId);

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else
                {
                    response = new ItemsResponse<ServiceType> { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
        [HttpGet("createdby")]
        public ActionResult<ItemsResponse<ServiceType>> SelectByCreatedBy(int userId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int iCurrentUserId = _authService.GetCurrentUserId();
                List<ServiceType> list = _serviceTypeService.SelectByCreatedBy(iCurrentUserId);

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else
                {
                    response = new ItemsResponse<ServiceType> { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
        [HttpPost]
        public ActionResult<ItemResponse<int>> InsertServiceType(ServiceTypeAddRequest model)
        {
            ObjectResult result = null;

            int userId = _authService.GetCurrentUserId();
            IUserAuthData user = _authService.GetCurrentUser();

            try
            {
                int id = _serviceTypeService.InsertServiceType(model, user.Id);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

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

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _serviceTypeService.Delete(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
    }
}
