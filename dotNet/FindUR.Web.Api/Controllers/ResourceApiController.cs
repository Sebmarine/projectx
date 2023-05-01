using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Resources;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/resources")]
    [ApiController]
    public class ResourceApiController : BaseApiController
    {
        private IResourceService _service = null;
        private IAuthenticationService<int> _authService = null;

        public ResourceApiController(IResourceService service,
            IAuthenticationService<int> authService,
            ILogger<ResourceApiController> logger) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        #region --- GETS ---

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Resource>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Resource resource = _service.GetResourceById(id);

                if (resource == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Resource>() { Item = resource };
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
        public ActionResult<ItemResponse<Paged<Resource>>> GetAllPaginated(int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<Resource> page = _service.GetResources(pageIndex, pageSize);

                if (page == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Resource>> { Item = page };
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

        [HttpGet("{id:int}/paginate")]
        public ActionResult<ItemResponse<Paged<Resource>>> GetByCreatedBy(int pageIndex, int pageSize, int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<Resource> page = _service.GetResourcesByCreatedBy(id, pageIndex, pageSize);

                if (page == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Resource>> { Item = page };
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
        public ActionResult<ItemResponse<int>> Create(ResourceAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                int newId = _service.AddResource(model, userId);

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
        public ActionResult<SuccessResponse> Update(ResourceUpdateRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;
            

            try
            {
                int userId = _authService.GetCurrentUserId();

                _service.UpdateResource(model, userId);

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
                _service.DeleteResource(id, userId);

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
