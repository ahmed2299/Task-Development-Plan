using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace TDP.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpServer = _configuration["Smtp:Server"] ?? "smtp.gmail.com";
            _smtpPort = int.Parse(_configuration["Smtp:Port"] ?? "587");
            _smtpUsername = _configuration["Smtp:Username"];
            _smtpPassword = _configuration["Smtp:Password"];
            _fromEmail = _configuration["Smtp:FromEmail"];
            _fromName = _configuration["Smtp:FromName"];
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            try
            {
                // Validate configuration
                if (string.IsNullOrEmpty(_smtpUsername) || string.IsNullOrEmpty(_smtpPassword))
                {
                    Console.WriteLine("❌ SMTP credentials are missing!");
                    Console.WriteLine("   Username: " + (_smtpUsername ?? "NULL"));
                    Console.WriteLine("   Password: " + (string.IsNullOrEmpty(_smtpPassword) ? "NULL" : "SET"));
                    return false;
                }

                Console.WriteLine($"📧 Sending SMTP email to: {to}");
                Console.WriteLine($"📧 From: {_fromEmail}");
                Console.WriteLine($"📧 SMTP Server: {_smtpServer}:{_smtpPort}");
                Console.WriteLine($"📧 Username: {_smtpUsername}");
                Console.WriteLine($"📧 Password Length: {_smtpPassword?.Length ?? 0} characters");
                
                // Check if password looks like Gmail App Password (16 chars)
                if (_smtpPassword?.Length != 16)
                {
                    Console.WriteLine("⚠️  Warning: Gmail App Passwords are exactly 16 characters!");
                    Console.WriteLine("   Current password length: " + (_smtpPassword?.Length ?? 0));
                }

                using var client = new SmtpClient(_smtpServer, _smtpPort);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                client.Timeout = 30000; // 30 seconds timeout

                using var message = new MailMessage();
                message.From = new MailAddress(_fromEmail, _fromName);
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = isHtml;

                Console.WriteLine("📧 Attempting to send email...");
                await client.SendMailAsync(message);
                Console.WriteLine("✅ SMTP Email sent successfully!");
                return true;
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"❌ SMTP Error: {smtpEx.Message}");
                Console.WriteLine($"❌ Status Code: {smtpEx.StatusCode}");
                
                if (smtpEx.Message.Contains("Authentication Required") || smtpEx.Message.Contains("5.7.0"))
                {
                    Console.WriteLine("💡 Solution: Generate Gmail App Password:");
                    Console.WriteLine("   1. Enable 2-Step Verification in Google Account");
                    Console.WriteLine("   2. Go to Security → App passwords");
                    Console.WriteLine("   3. Generate password for 'Mail' app");
                    Console.WriteLine("   4. Use the 16-character password in config");
                }
                
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ SMTP Email sending failed: {ex.Message}");
                Console.WriteLine($"❌ Exception Type: {ex.GetType().Name}");
                return false;
            }
        }

        public async Task<bool> SendWelcomeEmailAsync(string to, string userName)
        {
            var subject = "Welcome to TDP Application!";
            var htmlBody = $@"
                <html>
                <body>
                    <h2>Welcome {userName}!</h2>
                    <p>Thank you for registering with our application.</p>
                    <p>We're excited to have you on board!</p>
                    <br>
                    <p>Best regards,<br>TDP Team</p>
                </body>
                </html>";

            return await SendEmailAsync(to, subject, htmlBody, true);
        }

        public async Task<bool> SendPasswordResetEmailAsync(string to, string resetLink)
        {
            var subject = "Password Reset Request";
            var htmlBody = $@"
                <html>
                <body>
                    <h2>Password Reset Request</h2>
                    <p>You have requested to reset your password.</p>
                    <p>Click the link below to reset your password:</p>
                    <a href='{resetLink}'>Reset Password</a>
                    <p>If you didn't request this, please ignore this email.</p>
                    <br>
                    <p>Best regards,<br>TDP Team</p>
                </body>
                </html>";

            return await SendEmailAsync(to, subject, htmlBody, true);
        }

        public async Task<bool> SendNotificationEmailAsync(string to, string subject, string message)
        {
            var htmlBody = $@"
                <html>
                <body>
                    <h2>{subject}</h2>
                    <p>{message}</p>
                    <br>
                    <p>Best regards,<br>TDP Team</p>
                </body>
                </html>";

            return await SendEmailAsync(to, subject, htmlBody, true);
        }
    }
}