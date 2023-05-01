using EllipticCurve;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Comments;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.comments;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsApiController : BaseApiController
    {
        private ICommentsService _service = null;
        private IAuthenticationService<int> _authService = null;
        private IUserService _userService = null;
        public CommentsApiController(ICommentsService service
            , IUserService userService
            , ILogger<PingApiController> logger
            , IAuthenticationService<int> authService) : base(logger)


        {
            _service = service;
            _authService = authService;
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpGet("createdby/paginate")]
        public ActionResult<ItemResponse<Paged<Comment>>> GetCreatedBy(int pageIndex, int pageSize, int userId)
        {
            ActionResult result = null;

            try
            {


                Paged<Comment> paged = _service.GetCreatedBy(pageIndex, pageSize, userId);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Comment>> response = new ItemResponse<Paged<Comment>>();
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

        [AllowAnonymous]
        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Comment>>> Pagination(int pageIndex, int pageSize)
        {
            ActionResult result = null;

            try
            {
                Paged<Comment> paged = _service.Pagination(pageIndex, pageSize);
                if (paged == null)
                {
                    result = NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Comment>> response = new ItemResponse<Paged<Comment>>();
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

        [HttpPost("")]
        public ActionResult<ItemResponse<CommentBase>> Create(CommentsAddRequest model)
        {
            int userId = _authService.GetCurrentUserId();
            CommentBase comment = null;

            int code = 201;
            BaseResponse response = null;

            try
            {
                int id = _service.Add(model, userId);

                User user = _userService.GetById(userId);


                comment = new CommentBase()
                {
                    Id = id,
                    Text = model.Text,
                    Subject = model.Subject,
                    ParentId = model.ParentId,
                    EntityId = model.EntityId,
                    EntityType = new LookUp()
                    {
                        Id = model.EntityTypeId
                    },
                    DateCreated = model.DateCreated,
                    CreatedBy = user
                };


              

                response = new ItemResponse<CommentBase>() { Item = comment };

            }
            catch (Exception ex)
            {

                code = 500;
                response = new ErrorResponse(ex.Message);
            }


            return StatusCode(code, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<CommentBase>> Update(CommentsUpdateRequest model)
        {
            int userId = _authService.GetCurrentUserId();
            CommentBase comment = null;


            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.Update(model);

                User user = _userService.GetById(userId);


                comment = new CommentBase()
                {
                    Id = model.Id,
                    Text = model.Text,
                    Subject = model.Subject,
                    ParentId = model.ParentId,
                    EntityId = model.EntityId,
                    EntityType = new LookUp()
                    {
                        Id = model.EntityTypeId
                    },
                    DateCreated = model.DateCreated,
                    CreatedBy = user
                };

                response = new ItemResponse<CommentBase>() { Item = comment };
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
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

        [AllowAnonymous]
        [HttpGet("entitytype/{entityTypeId:int}/entity/{entityId:int}")]
        public ActionResult<ItemsResponse<Comment>> GetByEntity(int entityTypeId, int entityId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Comment> list = _service.GetWithReplies(entityTypeId, entityId);


                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<Comment> { Items = list };
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
    }
}
