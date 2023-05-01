using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Requests.Location;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/locations")]
    [ApiController]
    public class LocationApiContoller : BaseApiController
    {
        private ILocationService _service = null;
        private IAuthenticationService<int> _authService = null;
        private ILookUpService _lookup = null;

        public LocationApiContoller(ILookUpService lookup, ILocationService locationService, IAuthenticationService<int> authenticationService, ILogger<LocationApiContoller> logger) : base(logger)
        {
            _service = locationService;
            _authService = authenticationService;
            _lookup = lookup;
        }

        [HttpPost("{tableName}")]
        public ActionResult<ItemsResponse<LookUp>> GetFileType(string tableName)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<LookUp> list = _lookup.GetLookUp(tableName);

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource Not Found");
                }
                else
                {
                    response = new ItemsResponse<LookUp> { Items = list };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.ToString());
                base.Logger.LogError(ex.Message);
            }

            return StatusCode(iCode, response);
        }

        [HttpGet("geo")]
        public ActionResult<ItemsResponse<Location>> GetByGeo(int radius, double lat, double lng)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<Location> list = _service.GetByGeo(radius, lat, lng);

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("App Resource Not Found");
                }
                else
                {
                    response = new ItemsResponse<Location> { Items = list };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.ToString());
                base.Logger.LogError(ex.Message);
            }

            return StatusCode(iCode, response);
        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Location>>> GetByCreatedBy(int pageSize, int pageIndex, int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            Paged<Location> pagedLocation = null;

            try
            {
                pagedLocation = _service.GetByCreatedBy(id, pageIndex, pageSize);

                if (pagedLocation == null)
                {
                    iCode = 400;
                    response = new ErrorResponse("App Resource Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<Location>> { Item = pagedLocation };
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

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(LocationUpdateRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                int currentUser = _authService.GetCurrentUserId();
                _service.Update(model, currentUser);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(LocationAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int currentUser = _authService.GetCurrentUserId();
                int id = _service.Create(model, currentUser);

                ItemResponse<int> response = new ItemResponse<int> { Item = id };

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
    }
}
