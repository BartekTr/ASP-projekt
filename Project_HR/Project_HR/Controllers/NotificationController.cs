using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Project_HR.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly IConfiguration _configuration;

        public NotificationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PostMessage(string email)
        {
            var apiKey = _configuration.GetSection("SENDGRID_API_KEY").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("truszkowski.bartlomiej@wp.pl", "Admin");
            var to = new EmailAddress(email, "HR");

            var subject = "App notify";
            var htmlContent = "<strong>You have new application for your offer</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}