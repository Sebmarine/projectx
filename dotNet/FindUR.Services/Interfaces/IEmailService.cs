using Sabio.Models.Requests;
using Sabio.Models.Requests.Appointments;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface IEmailService
    {
        void ContactUs(ContactUsRequest contact);
        void SendEmailModel(EmailRequest model);
        void SendConfirmEmail(string email, string token);
        void SendResetPasswordEmail(string email, string token);
        void SendAppointmentEmail(AppointmentEmailRequest model);
    }
}
