using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [AllowAnonymous]
    [Route("api/lookups")]
    [ApiController]
    public class LookUpApiController : BaseApiController
    {

        private ILookUpService _lookUpService = null;
        private IAuthenticationService<int> _authenticationService = null;
        public LookUpApiController(ILookUpService service
            , ILogger<LookUpApiController> logger
            , IAuthenticationService<int> authenticationService) : base(logger)
        {
            _lookUpService = service;
            _authenticationService = authenticationService;
        }

        [HttpPost()]
        public ActionResult<ItemResponse<Dictionary<string, List<LookUp>>>> GetType(string[] tableNames)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Dictionary<string, List<LookUp>> lookup = _lookUpService.GetMany(tableNames);

                if (lookup == null)
                {
                    code = 404;
                    response = new ErrorResponse("Not Found");
                }
                else
                {
                    response = new ItemResponse<Dictionary<string, List<LookUp>>> { Item = lookup };
                }
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