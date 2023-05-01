using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.AdminData;
using Sabio.Models.Domain.Appointments;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/adminData")]
    [ApiController]
    public class AdminApiController : BaseApiController
    {
        private IAdminService _service = null;
        
        public AdminApiController(IAdminService service, ILogger<AdminApiController> logger) :base(logger)
        {
            _service = service;
        }

        [HttpGet("{dateRange:int}")]
        public ActionResult<ItemResponse<AdminData>> GetAllByDateRange(int dateRange)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                AdminData aData = _service.GetAllByDateRange(dateRange);

                if (aData == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found");
                }
                else
                {
                    response = new ItemResponse<AdminData> { Item = aData};
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

        [HttpGet("dates")]
        public ActionResult<ItemResponse<AdminData>> GetAllByDates(DateTime startDate, DateTime endDate)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                AdminData aData = _service.GetAllByDates(startDate, endDate);

                if (aData == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found");
                }
                else
                {
                    response = new ItemResponse<AdminData> { Item = aData };
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
