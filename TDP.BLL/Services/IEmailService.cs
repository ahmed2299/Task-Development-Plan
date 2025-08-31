using TDP.BLL.DTOs;

namespace TDP.BLL.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = false);
        Task<bool> SendWelcomeEmailAsync(string to, string userName);
        Task<bool> SendPasswordResetEmailAsync(string to, string resetLink);
        Task<bool> SendNotificationEmailAsync(string to, string subject, string message);
    }
} 