using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using Core.Services.Interface;
using Core.Services.DTO.Generic;

namespace Core.Services.Service
{
    public class EmailService : IEmail
    {
        public EmailService()
        {
        }

        public Task SendFormattedHTMLEmailAsync(HTMLEmailViewModel model)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("passiveincome.shawn@gmail.com");

            msg.Subject = model.Subject;
            msg.Body = model.Body;
            msg.IsBodyHtml = true;
            msg.To.Add(new MailAddress(model.Destination));

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.EnableSsl = true;
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("passiveincome.shawn@gmail.com", "yoj829IE*");
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = credentials;
            smtpClient.Port = 587;

            return smtpClient.SendMailAsync(msg);

        }
    }
}
