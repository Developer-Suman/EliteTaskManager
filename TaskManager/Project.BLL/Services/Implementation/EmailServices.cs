using Microsoft.Extensions.Configuration;
using Project.BLL.Services.Interface;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Services.Implementation
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;

        public EmailServices(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }
        public Task SendEmailAsync(string email, string subject, string body)
        {
            var apiKey = _configuration["SendGridApiKey:EmailApi"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("officeproj111@gmail.com", "Web Application");
            var to = new EmailAddress(email);
            var plainTextContent = body;
            var htmlContent = $"<p>{body}</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            return client.SendEmailAsync(msg);
        }
    }
}
