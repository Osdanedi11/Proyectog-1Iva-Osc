using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MovieSeriesManagement.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // En un entorno de producción, aquí implementarías el envío real de correos
            // Por ahora, solo registramos la información
            _logger.LogInformation($"Email: {email}, Subject: {subject}, Message: {htmlMessage}");

            return Task.CompletedTask;
        }
    }
}

