using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sabio.Models.AppSettings;
using Sabio.Models.Requests;
using Sabio.Services.Interfaces;
using Sabio.Services;
using Sabio.Web.Models.Responses;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using Sabio.Web.Controllers;
using Sabio.Web.Core.Configs;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/emails")]
    [ApiController]
    public class ContactUsApiController : BaseApiController
    {
        private AppKeys _appKeys;
        private readonly IEmailService _emailService = null;
        private readonly IAuthenticationService<int> _authService = null;

        public ContactUsApiController(IOptions<AppKeys> appKeys
            , IEmailService service
            , IAuthenticationService<int> authService
            , ILogger<ContactUsApiController> logger) : base(logger)
        {
            _appKeys = appKeys.Value;
            _emailService = service;
            _authService = authService;
        }

        [HttpPost("contact")]
        public ActionResult<SuccessResponse> SendContactForm(ContactUsRequest contact)
        {            
            int code = 200;
            BaseResponse response = null;

            try
            {
                _emailService.ContactUs(contact);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(code, response);
        }

        [HttpPost("reply")]
        public ActionResult<SuccessResponse> SendEmailModel(EmailRequest model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _emailService.SendEmailModel(model);
                response = new SuccessResponse();
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
