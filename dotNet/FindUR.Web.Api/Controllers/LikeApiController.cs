using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Likes;
using Sabio.Models.Requests.Likes;
using Sabio.Services;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/likes")]
    [ApiController]
    public class LikeApiController : BaseApiController
    {
        ILikeService _service = null;
        private IAuthenticationService<int> _authService;

        public LikeApiController(ILikeService service
            , IAuthenticationService<int> authService
            , ILogger<LikeApiController> logger) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpDelete("entitytype/{entityTypeId:int}/entityid/{entityId:int}")]
        public ActionResult<SuccessResponse> Delete(int entityId, int entityTypeId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.DeleteLike(entityId, entityTypeId, userId);

                response = new SuccessResponse();
            }

            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }


        [HttpPost]
        public ActionResult<SuccessResponse> Create(LikeAddRequest model)
        {
            int code = 201;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.AddLike(model, userId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);

        }

        [HttpGet("entitytype/{entityTypeId:int}/entityid/{entityId:int}")]
        public ActionResult<ItemsResponse<Like>> GetAllLikesByEntityId_EntityTypeId(int entityId, int entityTypeId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                List<Like> list = _service.GetByEntityId_EntityTypeId(entityId, entityTypeId);

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("Application resource not found");
                }
                else
                {
                    response = new ItemsResponse<Like> { Items = list };
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

        [HttpPut("entitytype/{entityTypeId:int}/entityid/{entityId:int}")]
        public ActionResult<ItemResponse<int>> UpdateLike(LikeUpdateRequest model, int entityTypeId, int entityId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                _service.UpdateLikeSatus(model, entityTypeId, entityId, userId);

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