using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain.Surveys;
using Sabio.Models.Requests.Surveys;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/instances")]
    [ApiController]
    public class SurveysInstancesApiController : BaseApiController
    {
        private IAuthenticationService<int> _authService = null;
        private ISurveyInstanceService _service = null;
        private ILogger _logger;

        public SurveysInstancesApiController(ISurveyInstanceService service
           , IAuthenticationService<int> authService
           , ILogger<SurveysInstancesApiController> logger) : base(logger)
        {
            _authService = authService;
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Add(SurveysInstancesAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int currentUserId = _authService.GetCurrentUserId();

                int id = _service.AddSurveyInstance(model);

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
        public ActionResult<SuccessResponse> Update(SurveysInstancesUpdateRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                int currentUserId = _authService.GetCurrentUserId();

                _service.UpdateSurveyInstances(model);

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
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.DeleteSurveyInstance(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;

                response = new ErrorResponse(ex.Message);

            }

            return StatusCode(code, response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<SurveyInstance>> GetBy(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                SurveyInstance course = _service.GetSurveyInstancesById(id);

                if (course == null)
                {
                    code = 404;
                    response = new ErrorResponse("Not found.");
                }
                else
                {
                    response = new ItemResponse<SurveyInstance> { Item = course };
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

        [HttpGet]
        public ActionResult<ItemResponse<Paged<SurveyInstance>>> GetAll(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<SurveyInstance> pages = _service.GetSurveyInstancePaginated(pageIndex, pageSize);

                if (pages == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<SurveyInstance>> { Item = pages };
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
        public ActionResult<ItemResponse<Paged<SurveyInstance>>> Search(int pageIndex, int pageSize, int search)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {

                Paged<SurveyInstance> page = _service.SurveyInstanceCreatedBy(pageIndex, pageSize, search);

                if (page == null)
                {
                    code = 404;
                    response = new ErrorResponse("App Resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<SurveyInstance>> { Item = page };
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
        [HttpGet("detailed/{id:int}")]
        public ActionResult GetById(int id)
        {
            int code = 200;
            BaseResponse response = null;
            SurveyInstanceDetailed surveyInstance = null;

            try
            {
                surveyInstance = _service.GetInstanceDetailed(id);
                if (surveyInstance == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource not found");
                }
                else
                {
                    response = new ItemResponse<SurveyInstanceDetailed>() { Item = surveyInstance };
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
        [HttpGet("detailed/paginate")]
        public ActionResult GetPaginated(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                Paged<SurveyInstanceDetailed> surveyInstance = _service.GetInstancesDetailed(pageSize, pageIndex);
                if (surveyInstance == null)
                {
                    code = 404;
                    response = new ErrorResponse("Resource not found");
                }
                else
                {
                    response = new ItemResponse<Paged<SurveyInstanceDetailed>>() { Item = surveyInstance };
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

