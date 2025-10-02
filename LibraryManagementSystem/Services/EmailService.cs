using LibraryManagementSystem.API.DTO;
using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using NLog;
using NLog.Web;

namespace LibraryManagementSystem.API.Services
{
    public class EmailService
    {

        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
                _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(SendEmailDTO emailSent)
        {
            var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            MailjetClient client = new MailjetClient(_configuration["Mailjet:ApiKey"], _configuration["Mailjet:SecretKey"]);
            var email= new TransactionalEmailBuilder()
                .WithFrom(new SendContact(_configuration["Email:From"], _configuration["Email:ApplicationName"]))
                .WithSubject(emailSent.Subject)
                .WithHtmlPart(emailSent.Body)
                .WithTo(new SendContact(emailSent.To))
                .Build();

            var response=await client.SendTransactionalEmailAsync(email);
            logger.Info($"Mailjet Messages: {System.Text.Json.JsonSerializer.Serialize(response.Messages)}");


            if (response.Messages != null)
            {
                if (response.Messages[0].Status == "success")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
