using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Microsoft.Extensions.Logging;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using Sabio.Models.Domain.CharitableFunds;
using System;
using System.Collections.Generic;
using Sabio.Models.Requests.CharitableFunds;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/charitablefunds")]
    [ApiController]
    public class CharitableFundApiController : BaseApiController
    {
        private ICharitableFundService _service = null;
        private IAuthenticationService<int> _authService = null;

        public CharitableFundApiController(ICharitableFundService service
            , ILogger<CharitableFundApiController> logger
            , IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpGet]
        public ActionResult<ItemResponse<CharitableFund>> GetAll() 
        { 
            int code = 200; 
             
            BaseResponse response = null; 
             
            try 
            {  
                List<CharitableFund> list = _service.GetAll(); 
                  
                if (list == null) 
                { 
                    code = 404; 
                     
                    response = new ErrorResponse("Application Resource not found."); 
                } 
                else 
                { 
                    response = new ItemsResponse<CharitableFund> { Items = list }; 
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
        public ActionResult<ItemResponse<CharitableFund>> Get(int id)
        {
            int iCode = 200;

            BaseResponse response = null;

            try
            {
                CharitableFund charitableFund = _service.Get(id);

                if(charitableFund == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemResponse<CharitableFund> { Item = charitableFund };
                }
            }
            catch(Exception ex)
            {
                iCode = 500;

                response = new ErrorResponse($"Generic Error: {ex.Message})");
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
            catch(Exception ex)
            {
                code = 500;

                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(CharitableFundAddRequest model)
        {
            int code = 201;

            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                int id = _service.Add(model, userId);

                response = new ItemResponse<int> { Item = id };       
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
        public ActionResult<ItemResponse<int>> Update(CharitableFundUpdateRequest model)
        {
            int code = 200;

            int userId = _authService.GetCurrentUserId();

            BaseResponse response = null;

            try
            {
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
