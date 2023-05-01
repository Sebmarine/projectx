using Microsoft.AspNetCore.Mvc;
using Sabio.Services;
using Sabio.Web.Controllers;
using Microsoft.Extensions.Logging;
using Sabio.Services.Interfaces;
using Sabio.Web.Models.Responses;
using System;
using Sabio.Models.Requests.ShoppingCarts;
using Sabio.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using Sabio.Models.Domain.ShoppingCart;
using Sabio.Models.Domain.HorseProfiles;
using Microsoft.AspNetCore.Authorization;
using Sabio.Models.Requests.HorseProfiles;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/shoppingcarts")]
    [ApiController]

    public class ShopingCartApiController : BaseApiController
    {
        private ILogger _logger;
        private IAuthenticationService<int> _authService;
        private IShoppingCartService _service;
        public ShopingCartApiController(ILogger<ShopingCartApiController> logger, IAuthenticationService<int> authService, IShoppingCartService service) : base(logger)
        {
            _logger = logger;
            _authService = authService;
            _service = service;
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<ShoppingCart>> GetAll(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<ShoppingCart> results = _service.GetAll(pageIndex, pageSize);
                if (results == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<ShoppingCart>> { Item = results };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                _logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<ShoppingCart>> GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                ShoppingCart result = _service.GetById(id);
                if (result == null)
                {
                    code = 401;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<ShoppingCart> { Item = result };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                _logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(ShoppingCartAddRequest model)
        {
            ObjectResult result = null;
            try
            {
                int currentUserId = _authService.GetCurrentUserId();
                int id = _service.Add(model, currentUserId);
                ItemResponse<int> response = new ItemResponse<int> { Item = id };
                result = Created201(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(ShoppingCartUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                int iCurrentUserId = _authService.GetCurrentUserId();
                _service.Update(model, iCurrentUserId);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                _logger.LogError(ex.ToString());
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
                _logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("byuser")]
        public ActionResult<ItemResponse<Paged<ShoppingCart>>> GetCreatedBy(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                List<ShoppingCart> list = _service.GetCreatedBy(id);

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource not found .");
                }
                else
                {
                    response = new ItemsResponse<ShoppingCart> { Items = list };
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

    }
}

