using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Blogs;
using Sabio.Models.Requests.Blog;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Sabio.Web.Api.Controllers.Blogs
{
    [Route("blogs")]
    [ApiController]
    public class BlogApiController : BaseApiController
    {
        private IAuthenticationService<int> _authService = null;
        private IBlogService _service = null;
        private ILogger _logger;

        public BlogApiController(IBlogService service
              , IAuthenticationService<int> authService
              , ILogger<BlogApiController> logger) : base(logger)
        {
            _authService = authService;
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Vet,Admin")]
        public ActionResult<ItemResponse<int>> Add(BlogAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int currentUserId = _authService.GetCurrentUserId();

                int id = _service.Add(model, currentUserId);

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

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public ActionResult<ItemResponse<Blog>> GetBy(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Blog course = _service.GetBy(id);

                if (course == null)
                {
                    code = 404;
                    response = new ErrorResponse("Not found.");
                }
                else
                {
                    response = new ItemResponse<Blog> { Item = course };
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

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Vet,Admin")]
        public ActionResult<SuccessResponse> Update(BlogUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int currentUserId = _authService.GetCurrentUserId();

                _service.Update(model, currentUserId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Vet,Admin")]
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<ItemResponse<Paged<Blog>>> GetAll(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Blog> page = _service.GetAll(pageIndex, pageSize);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Blog>> { Item = page };
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
       
        [AllowAnonymous]
        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<Blog>>> Search(int pageIndex, int pageSize, string query)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {

                Paged<Blog> page = _service.Search(pageIndex, pageSize, query);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Blog>> { Item = page };
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

        [HttpGet("current")]
        public ActionResult<ItemResponse<Paged<Blog>>> CreatedBy(int pageIndex, int pageSize, int userId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<Blog> page = _service.CreatedBy(pageIndex, pageSize, userId);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Blog>> { Item = page };
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

        [HttpGet("blogtype/{id:int}")]
        [AllowAnonymous]
        public ActionResult<ItemsResponse<Blog>> GetBlogType(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Blog> list = _service.GetBlogType(id);

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<Blog> { Items = list };
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

        [HttpGet("recent")]
        public ActionResult<ItemsResponse<Blog>> GetRecent()
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Blog> list = _service.GetRecent();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<Blog> { Items = list };
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
