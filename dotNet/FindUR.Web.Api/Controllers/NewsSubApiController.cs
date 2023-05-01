using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.NewsletterSubscriptions;
using Sabio.Models.Requests.NewsletterSubscriptions;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/newsletter/")]
    [ApiController]
    public class NewsSubApiController : BaseApiController
    {
        private INewsletterSubService _newsletterSubService = null;
        private IAuthenticationService<int> _authService = null;

        public NewsSubApiController(INewsletterSubService service,
            IAuthenticationService<int> authService,
            ILogger<NewsSubApiController> logger) : base(logger)
        {
            _newsletterSubService = service;
            _authService = authService;
        }

        [HttpGet("subscriptions")]
        public ActionResult<ItemResponse<Paged<NewsSub>>> GetPagination(int pageIndex, int pageSize)
        {
            ActionResult result = null;

            try
            {
                Paged<NewsSub> paged = _newsletterSubService.GetAllPagination(pageIndex, pageSize);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found."));
                }
                else
                {
                    ItemResponse<Paged<NewsSub>> response = new ItemResponse<Paged<NewsSub>>();
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

        [HttpGet("subscriptions/subscribed")]
        public ActionResult<ItemResponse<List<NewsSub>>> GetSubsribed()
        {
            ActionResult result = null;

            try
            {
                List<NewsSub> paged = _newsletterSubService.GetAllSubscribed();
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found."));
                }
                else
                {
                    ItemResponse<List<NewsSub>> response = new ItemResponse<List<NewsSub>>();
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

        [HttpGet("subscriptions/date")]
        public ActionResult<ItemResponse<Paged<NewsSub>>> SearchDate(int pageIndex, int pageSize, string date)
        {
            ActionResult result = null;

            try
            {
                Paged<NewsSub> paged = _newsletterSubService.SelectByDate(pageIndex, pageSize, date);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found."));
                }
                else
                {
                    ItemResponse<Paged<NewsSub>> response = new ItemResponse<Paged<NewsSub>>();
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
        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<ItemResponse<string>> Create(NewsSubAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                // Newsletter subscriptions requires email address
                string id = _newsletterSubService.Add(model);
                ItemResponse<string> response = new ItemResponse<string>() { Item = id};

                result = Created201(response);
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());

                result = StatusCode(500, response);
            }

            return result;
        }

        [HttpPut]
        public ActionResult Create(NewsSubUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _newsletterSubService.Update(model);
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
