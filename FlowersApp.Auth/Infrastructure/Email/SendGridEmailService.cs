using FlowerApp.Auth.Domain.Interfaces;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace FlowerApp.Auth.Infrastructure.Email
{
    public class SendGridEmailService : IEmailSender
    {
        private readonly ISendGridClient _client;
        private readonly EmailSettings _settings;
        private readonly ILogger<SendGridEmailService> _logger;

        public SendGridEmailService(IOptions<EmailSettings> settings, ILogger<SendGridEmailService> logger)
        {
            _settings = settings.Value;
            _client = new SendGridClient(_settings.ApiKey);
            _logger = logger;
        }

        public async Task SendAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default)
        {
            var fromName = string.IsNullOrWhiteSpace(_settings.FromName) ? "FlowersApp" : _settings.FromName;
            var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(
                from, to, subject,
                plainTextContent: body,
                htmlContent: $"<p>{body}</p>");

            const int maxRetries = 3;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    var response = await _client.SendEmailAsync(msg);

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Email sent to {Email}, subject: {Subject}", toEmail, subject);
                        return;
                    }

                    var errorBody = await response.Body.ReadAsStringAsync();
                    _logger.LogError("SendGrid Failed! StatusCode: {StatusCode}, Details: {ErrorBody}", response.StatusCode, errorBody);

                    throw new Exception($"Failed to send email via SendGrid. Status: {response.StatusCode}, Response: {errorBody}");

                    //var responseBody = await response.Body.ReadAsStringAsync();
                    //_logger.LogWarning("SendGrid returned {StatusCode} on attempt {Attempt}: {Body}",
                    //    response.StatusCode, attempt, responseBody);
                    //Console.WriteLine($"SendGrid Status Code: {response.StatusCode}");
                    //Console.WriteLine($"SendGrid Response Body: {responseBody}");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "SendGrid send failed on attempt {Attempt}", attempt);
                }

                if (attempt < maxRetries)
                    await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt)));
            }

            _logger.LogError("Failed to send email to {Email} after {MaxRetries} attempts", toEmail, maxRetries);
            throw new InvalidOperationException("Failed to send email after multiple attempts.");

        }

       
    }
}
