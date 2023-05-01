using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Models.Domain.FAQ;
using Sabio.Models.Requests.FAQ;
using Sabio.Web.Models.Responses;
using System.Collections.Generic;
using System;
using Sabio.Models.Requests.ExternalLinks;
using Microsoft.AspNetCore.Authentication;
using Sabio.Models;
using Sabio.Models.Domain.ExternalLinks;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/ExternalLinks")]
    [ApiController]
    public class ExternalLinksController : BaseApiController
    {
        private IExternalLinksService _service = null;
        private IAuthenticationService<int> _authService = null;
        
        public ExternalLinksController(IExternalLinksService service
            , ILogger<ExternalLinksController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

       
        
        [HttpPost]
        public ActionResult Create(ExternalLinksAddRequest model)
        {
            ObjectResult result = null;
            int userId = 0;
            try
            {
                userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);
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
        public ActionResult Delete(int id)
        {
            int code = 200;
            int userId = 0;
            BaseResponse response = null;
            try
            {
                userId = _authService.GetCurrentUserId();
                _service.Delete(id, userId);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpPut("{id:int}")] 
        public ActionResult<ItemResponse<int>> Update(ExternalLinksUpdateRequest model, int userId)
        {
            int code = 200;
            //int userId = 0;
            BaseResponse response = null;
            try
            {
                userId = _authService.GetCurrentUserId();
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
        
        [HttpGet]
        public ActionResult<ItemsResponse<ExternalLink>> Get(int userId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                userId = _authService.GetCurrentUserId();
                List<ExternalLink> list  = _service.Get(userId);
                response = new SuccessResponse();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Request not found");
                }
                else
                {
                    response = new ItemsResponse<ExternalLink> { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(code, response);
        }
    }
}
//app://resources/notifications.html#