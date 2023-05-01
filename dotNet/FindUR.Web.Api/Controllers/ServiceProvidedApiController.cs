using Microsoft.AspNetCore.Authorization;
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

namespace Sabio.Web.Api.Controllers
{
    [Route("api/services")]
    [ApiController]
    public class ServiceProvidedApiController : BaseApiController
    {
        #region -- Controller, Logger, Authentication --
        private IServiceProvidedService _serviceService = null;
        private IAuthenticationService<int> _authService = null;
        public ServiceProvidedApiController(IServiceProvidedService service
            , ILogger<ServiceProvidedApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _serviceService = service;
            _authService = authService;
        }
        #endregion

        #region -- Service Add -- OK
        [HttpPost]
        public ActionResult<ItemResponse<int>> InsertService(ServiceProvidedAddRequest model)
        {
            ObjectResult result = null;

            int userId = _authService.GetCurrentUserId();
            IUserAuthData user = _authService.GetCurrentUser();

            try
            {
                int id = _serviceService.InsertService(model, user.Id);
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
        #endregion

        #region -- Service Update -- OK
        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> UpdateService(ServiceProvidedUpdateRequest model)
        {
            int userId = _authService.GetCurrentUserId();
            IUserAuthData user = _authService.GetCurrentUser();

            int code = 200;
            BaseResponse response = null;
            try
            {

                _serviceService.UpdateService(model, userId);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
        #endregion

        #region -- Service Delete -- OK 
        [HttpDelete("{id:int}")]
        public ActionResult<ItemResponse<int>> DeleteService(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _serviceService.DeleteService(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
        #endregion

        #region -- Service Select By Id -- OK
        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Service>> SelectServiceById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Service service = _serviceService.SelectServiceById(id);

                if (service == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resource not found");
                }
                else
                {
                    response = new ItemResponse<Service> { Item = service };
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
        #endregion

        #region -- Service Select Paginated --
        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Service>>> SelectServicesByPage(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Service> page = _serviceService.SelectServicesByPage(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Service>> { Item = page };
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
        #endregion

        #region -- Services Select By Created By --
        [HttpGet("createdby/paginate")]
        public ActionResult<ItemResponse<Paged<Service>>> SelectServicesByCreatedBy(int pageIndex, int pageSize, int userId)
        {
            ActionResult result = null;
            try
            {
                Paged<Service> paged = _serviceService.SelectServicesByCreatedBy(pageIndex, pageSize, userId);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Record Not Found"));

                }
                else
                {
                    ItemResponse<Paged<Service>> response = new ItemResponse<Paged<Service>>();
                    response.Item = paged;
                    result = Ok200(response);
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

        #region -- Services Search By Created By --
        [HttpGet("createdby/search")]
        public ActionResult<ItemResponse<Service>> SearchCreatedBy(string query, int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int iCurrentUserId = _authService.GetCurrentUserId();
                Paged<Service> results = _serviceService.SearchCreatedBy(query, pageIndex, pageSize);
                if (results == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Service>> { Item = results };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }
        #endregion

        #region -- Services Select By Practice By --
        [AllowAnonymous]
        [HttpGet("practice/{practiceId:int}")]
        public ActionResult<ItemResponse<Paged<Service>>> SelectServicesByPracticeId(int pageIndex, int pageSize, int practiceId)
        {
            ActionResult result = null;
            try
            {
                Paged<Service> paged = _serviceService.SelectServicesByPracticeId(pageIndex, pageSize, practiceId);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Record Not Found"));

                }
                else
                {
                    ItemResponse<Paged<Service>> response = new ItemResponse<Paged<Service>>();
                    response.Item = paged;
                    result = Ok200(response);
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
    }
}

