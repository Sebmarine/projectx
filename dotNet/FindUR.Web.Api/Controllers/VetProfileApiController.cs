using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Appointments;
using Sabio.Models.Domain.VetProfiles;
using Sabio.Models.Requests.VetProfiles;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/vetprofiles")]
    [ApiController]
    public class VetProfileApiController : BaseApiController
    {
        private IVetProfileService _service = null;
        private IAuthenticationService<int> _authSerivce = null;

        public VetProfileApiController(IVetProfileService service,
            ILogger<VetProfileApiController> logger,
            IAuthenticationService<int> authService) : base(logger)
        {
            _service = service;
            _authSerivce = authService;
        }


        [HttpGet("initial")]
        public ActionResult<ItemResponse<Boolean>> InitialLogIn()
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                int id = _authSerivce.GetCurrentUserId();
                Boolean result = _service.CheckIfVetHasProfile(id);
                response = new ItemResponse<Boolean> { Item = result};

            }catch(Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
                
            }

            return StatusCode(iCode, response);
        }


        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<VetProfileV2>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                VetProfileV2 aVetProfile = _service.GetVetProfileById(id);

                if (aVetProfile == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Vet Profile not found");
                }
                else
                {
                    response = new ItemResponse<VetProfileV2> { Item = aVetProfile };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }

            return StatusCode(iCode, response);
        }


        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(VetProfileAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authSerivce.GetCurrentUserId();
                int id = _service.AddVetProfile(model, userId);

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


        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(VetProfileUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int userId = _authSerivce.GetCurrentUserId();
                _service.UpdateVetProfile(model, userId);
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
        public ActionResult Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.DeleteVetProfile(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }


        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<VetProfileV2>>> GetCreatedByPaginated(int pageIndex, int pageSize, int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<VetProfileV2> page = _service.GetVetProfileByCreated(pageIndex, pageSize, id);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<VetProfileV2>> { Item = page };
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


        [HttpGet("paginateall")]
        public ActionResult<ItemResponse<Paged<VetProfileV2>>> GetAllPaginated(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<VetProfileV2> page = _service.GetAll(pageIndex, pageSize);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<VetProfileV2>> { Item = page };
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
        public ActionResult<ItemsResponse<Appointment>> GetRecent()
        {

            int code = 200;
            BaseResponse response = null;

            try
            {
                List<VetProfileV2> list = _service.GetRecent();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<VetProfileV2> { Items = list };
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


        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<VetProfileV2>>> SearchPaginated(int pageIndex, int pageSize, string query)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<VetProfileV2> page = _service.SearchPaginated(pageIndex, pageSize, query);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<VetProfileV2>> { Item = page };
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


        [HttpGet("searchbycategory")]
        public ActionResult<ItemResponse<Paged<VetProfileV2>>> SearchByCategoryPaginated(int pageIndex, int pageSize, string query, int categoryId)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<VetProfileV2> page = _service.SearchByServiceTypeIdPaginated(pageIndex, pageSize, query, categoryId);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<VetProfileV2>> { Item = page };
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
        [HttpGet("practice/{id:int}")]
        public ActionResult<ItemResponse<Paged<VetProfileV2>>> GetByPracticeId(int pageIndex, int pageSize , int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<VetProfileV2> page = _service.GetVetProfileByPractice(pageIndex, pageSize, id);
                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<VetProfileV2>> { Item = page };
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
