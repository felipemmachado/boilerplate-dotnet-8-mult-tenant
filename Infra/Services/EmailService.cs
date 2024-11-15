using Application.Common.Configs;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
namespace Infra.Services
{
    public class EmailService(
        IOptions<EmailConfig> emailConfig,
        IOptions<ApiConfig> apiConfig,
        IApplicationDbContext context,
        ICurrentTenantService currentTenantService) : IEmailService
    {
        private readonly ApiConfig _apiConfig = apiConfig.Value ?? throw new ArgumentNullException(nameof(apiConfig));
        private readonly EmailConfig _emailConfig = emailConfig.Value ?? throw new ArgumentNullException(nameof(emailConfig));

        public async Task<bool> ForgotPassword(ForgotPasswordDto forgotPassword)
        {
            var data = new
            {
                email = forgotPassword.Email,
                link = await Url($"reset-password/?token={forgotPassword.Token}"),
                name = forgotPassword.Name
            };

            var result = await SendEmail(_emailConfig.Template.ResetPasswordEmail, forgotPassword.Email, data);
            return result.IsSuccessStatusCode;
        }

        private async Task<string> Url(string uri)
        {
            var urlTenant = await context
                    .Companies
                    .Where(p => p.Id == currentTenantService.TenantId)
                    .Select(p => p.Url).FirstAsync()
                ?? throw new ValidationException(ApiResponseMessages.TenantIdRequired);

            return $"{_apiConfig.AppUrl}/{urlTenant}/{uri}";
        }

        private async Task<Response> SendEmail(string templateId, string email, object data)
        {
            var fromEmail = new EmailAddress(_emailConfig.FromEmail, _emailConfig.FromName);
            var client = new SendGridClient(_emailConfig.ApiKey);
            var to = new EmailAddress(email);
            var message = new SendGridMessage();
            message.SetFrom(fromEmail);
            message.AddTo(to);
            message.SetTemplateId(templateId);
            message.SetTemplateData(data);

            return await client.SendEmailAsync(message);
        }
    }
}
