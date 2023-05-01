using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.FAQ;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;
using System;
using Sabio.Models.Domain.Licenses;
using Sabio.Models.Domain.Comments;
using Sabio.Models;
using Sabio.Models.Domain.Schedules;
using Sabio.Models.Requests.HorseProfiles;
using Sabio.Models.Requests.Licenses;
using Sabio.Models.Requests.Schedules;
using Sabio.Models.Requests.FAQ;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/licenses")]
    [ApiController]
    public class LicensesApiController : BaseApiController
    {
        private ILicensesService _service = null;
        private IAuthenticationService<int> _authService = null;
        public LicensesApiController(ILicensesService service
            , ILogger<PingApiController> logger
            , IAuthenticationService<int> authService) : base(logger)


        {
            _service = service;
            _authService = authService;
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<License>>> GetAll(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<License> page = _service.GetAll(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<License>> { Item = page };
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



        [HttpGet("createdby/paginate")]
        public ActionResult<ItemResponse<Paged<License>>> GetCreatedBy(int pageIndex, int pageSize, int userId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<License> page = _service.GetCreatedBy(pageIndex, pageSize, userId);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<License>> { Item = page };
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



        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<License>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                License license = _service.Get(id);

                if (license == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Schedule not found.");
                }
                else
                {
                    response = new ItemResponse<License> { Item = license };
                }
            }

            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);
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

        [HttpPost()]
        public ActionResult<ItemResponse<int>> Add(LicenseAddRequest model)
        {
 

            int code = 201;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);

                response = new ItemResponse<int>() { Item = id };

            }
            catch (Exception ex)
            {

                code = 500;
                response = new ErrorResponse(ex.Message);
            }


            return StatusCode(code, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(LicenseUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                 int userId = _authService.GetCurrentUserId();
                _service.Update(model, userId);
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
