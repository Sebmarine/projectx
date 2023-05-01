using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain;
using Sabio.Models.Requests.SiteReference;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/sitereference")]
    [ApiController]
    public class SiteReferenceApiController : BaseApiController
    {
        private ISiteReferenceService _service = null;
        private ILookUpService _lookUpService = null;

        public SiteReferenceApiController(ISiteReferenceService service, ILookUpService lookup, ILogger<SiteReferenceApiController> logger) : base(logger)
        {
            _service = service;
            _lookUpService = lookup;
        }

        [AllowAnonymous]
        [HttpPost("{tableName}")]
        public ActionResult<ItemsResponse<LookUp>> getLookUp(string tableName)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<LookUp> list = _lookUpService.GetLookUp(tableName);

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource Not Found");
                }
                else
                {
                    response = new ItemsResponse<LookUp> { Items = list };
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

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<SuccessResponse> Add(SiteReferenceAddRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.Add(model);
                response = new SuccessResponse();

            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(iCode, response);
        }
    }
}
