using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Blogs;
using Sabio.Models.Domain.Practices;
using Sabio.Models.Domain.Services;
using Sabio.Models.Requests.Practices;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using Sabio.Web.StartUp;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/practices")]
    [ApiController]
    public class PracticeApiController : BaseApiController
    {
        private IPracticeService _service = null;
        private IAuthenticationService<int> _authService = null;
        public PracticeApiController(IPracticeService service
            , ILogger<PracticeApiController> logger
            , IAuthenticationService<int> authService)
            : base(logger)
        {
            _service = service;
            _authService = authService;
        }
        
        [HttpPost]
        public ActionResult Create(PracticeAddRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            int userId = _authService.GetCurrentUserId();
            IUserAuthData user = _authService.GetCurrentUser();

            try
            {

                int id = _service.AddPractice(model, user.Id);
                response = new ItemResponse<int>() { Item = id };


            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse($"Generic Error: ${ex.Message}");
                base.Logger.LogError(ex.ToString());

            }

            return StatusCode(code, response);
        }
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public ActionResult<ItemResponse<Practice>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                Practice practice = _service.GetPracticeById(id);
                if (practice == null)
                {
                    iCode = 404;

                    response = new ErrorResponse("Application resource not Found");

                }
                else
                {
                    response = new ItemResponse<Practice> { Item = practice };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse($"Generic Error: ${ex.Message}");
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(iCode, response);
        }
        [HttpPut("{id:int}")]
        public ActionResult Update(PracticeUpdateRequest model)
        {
            int userId = _authService.GetCurrentUserId();
            
            int code = 200;
            BaseResponse response = null;
            try
            {

                _service.UpdatePractice(model, userId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {              
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }
        [HttpGet("paginate")]
        [AllowAnonymous]
        public ActionResult<ItemResponse<Paged<Practice>>> GetByPage(int pageIndex, int pageSize)
        {
            ActionResult result = null;
            try
            {
                Paged<Practice> paged = _service.GetPractices(pageIndex, pageSize);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Record Not Found"));

                }
                else
                {
                    ItemResponse<Paged<Practice>> response = new ItemResponse<Paged<Practice>>();
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
        [HttpGet("paginate/{userId:int}")]
        [AllowAnonymous]
        public ActionResult<ItemResponse<Paged<Practice>>> GetByCreatedByPage(int pageIndex, int pageSize, int userId)
        {
            ActionResult result = null;
            try
            {
                Paged<Practice> paged = _service.GetPracticesByCreatedByPage(pageIndex, pageSize, userId);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Record Not Found"));

                }
                else
                {
                    ItemResponse<Paged<Practice>> response = new ItemResponse<Paged<Practice>>();
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
        [HttpGet("paginate/search")]
        [AllowAnonymous]
        public ActionResult<ItemResponse<Paged<Practice>>> GetPagedBySearch(int pageIndex, int pageSize , string query)
        {
            ActionResult result = null;
            try
            {
                Paged<Practice> paged = _service.GetPracticeBySearch(pageIndex, pageSize, query);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Record Not Found"));

                }
                else
                {
                    ItemResponse<Paged<Practice>> response = new ItemResponse<Paged<Practice>>();
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
        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;


            try
            {
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {

                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpGet("paginate/searchv2")]
        [AllowAnonymous]
        public ActionResult<ItemResponse<Paged<Practice>>> GetPaginatedBySearch(int pageIndex, int pageSize, string query)
        {
            ActionResult result = null;
            try
            {
                Paged<Practice> paged = _service.GetPracticeBySearchV2(pageIndex, pageSize, query);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Record Not Found"));

                }
                else
                {
                    ItemResponse<Paged<Practice>> response = new ItemResponse<Paged<Practice>>();
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


        [HttpGet("practiceservicetype/{id:int}")]
        public ActionResult<ItemsResponse<Practice>> GetPracticeByServiceTypeId(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int currentUserId = _authService.GetCurrentUserId();
                List<Practice> list = _service.GetPractiveByServiceType(id);

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<Practice> { Items = list };
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

    }

}
