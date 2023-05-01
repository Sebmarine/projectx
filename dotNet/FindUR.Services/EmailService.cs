using EllipticCurve;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Sabio.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Sabio.Models.Requests;
using Microsoft.AspNetCore.Hosting;
using Sabio.Models.AppSettings;
using System.IO;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Appointments;
using Microsoft.VisualBasic;
using System;

namespace Sabio.Services
{
    public class EmailService : IEmailService
    {
        private AppKeys _appKeys;
        private IWebHostEnvironment _hostEnvironment;
        private Domain _domain;
        private ContactInfo _contactInfo;
        public EmailService(IOptions<AppKeys> appKeys, IOptions<Domain> domain, IOptions<ContactInfo> contactInfo
            , IWebHostEnvironment env)
        {
            _appKeys = appKeys.Value;
            _hostEnvironment = env;
            _domain = domain.Value;
            _contactInfo = contactInfo.Value;
        }
        public void SendResetPasswordEmail(string email, string token)
        {
            var path = Path.Combine(_hostEnvironment.WebRootPath, "EmailTemplates", "ResetPassword.html");
            var msg = new SendGridMessage()
            {
                From = new EmailAddress()
                {
                    Email = _contactInfo.AdminEmail,
                    Name = "MiVet"
                },
                Subject = "MiVet Reset Password Confirmation",
                HtmlContent = File.ReadAllText(path).Replace("{{token}}", token).Replace("{{email}}", email).Replace("{{domain}}", _domain.Url)
            };
            msg.AddTo(email, "MiVet");
            SendEmail(msg);
        }

        private async void SendEmail(SendGridMessage message)
        {
            var client = new SendGridClient(_appKeys.SendGridAppKey);
            await client.SendEmailAsync(message);
        }

        public void ContactUs(ContactUsRequest contact)
        {
            var contactForm = new SendGridMessage()
            {
                From = new EmailAddress(contact.From),
                Subject = contact.Subject,
                PlainTextContent = contact.Message
            };
            contactForm.AddTo(new EmailAddress("pijejev269@ploneix.com", "MiVet"));
            SendEmail(contactForm);
        }
        public void SendEmailModel(EmailRequest model)
        {
            var body = Path.Combine(_hostEnvironment.WebRootPath, "EmailTemplates", "TestTemplate.html");
            model.HtmlContent = File.ReadAllText(body);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(model.From),
                Subject = model.Subject,
                PlainTextContent = model.PlainTextContent,
                HtmlContent = model.HtmlContent,
            };
            msg.AddTo(new EmailAddress ("pijejev269@ploneix.com", "MiVet"));
            SendEmail(msg);
        }
        public void SendConfirmEmail(string email, string token)
        {
            var path = Path.Combine(_hostEnvironment.WebRootPath, "EmailTemplates", "AccountConfirm.html");
            var msg = new SendGridMessage()
            {
                From = new EmailAddress()
                {
                    Email = _contactInfo.AdminEmail,
                    Name = "MiVet"
                },
                Subject = "MiVet Account Confirmation",
                HtmlContent = File.ReadAllText(path).Replace("{{token}}", token).Replace("{{email}}", email).Replace("{{domain}}", _domain.Url)
            };
            msg.AddTo(email, "MiVet");
            SendEmail(msg);
        }
        public void SendAppointmentEmail(AppointmentEmailRequest request)
        {
            var path = Path.Combine(_hostEnvironment.WebRootPath, "EmailTemplates", "AppointmentRequest.html");
            var msg = new SendGridMessage()
            {
                From = new EmailAddress()
                {
                    Email = "jaredays@gmail.com", //replace with MiVet automated email when established
                    Name = "MiVet"
                },
                Subject = "MiVet Appointment Request",
                HtmlContent = File.ReadAllText(path).Replace("{{date}}", request.AppointmentStart.ToUniversalTime().ToString()).Replace("{{clientEmail}}", request.ClientEmail)
            };
            msg.AddTo(request.VetEmail, "MiVet"); 
            SendEmail(msg);
        }
    }
}
