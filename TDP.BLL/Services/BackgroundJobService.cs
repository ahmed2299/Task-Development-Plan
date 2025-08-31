using Hangfire;

namespace TDP.BLL.Services
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly IEmailService _emailService;

        public BackgroundJobService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public string EnqueueEmailJob(string to, string subject, string body)
        {
            return BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(to, subject, body, false));
        }

        public string EnqueueWelcomeEmailJob(string to, string userName)
        {
            return BackgroundJob.Enqueue(() => _emailService.SendWelcomeEmailAsync(to, userName));
        }

        public string EnqueuePasswordResetEmailJob(string to, string resetLink)
        {
            return BackgroundJob.Enqueue(() => _emailService.SendPasswordResetEmailAsync(to, resetLink));
        }

        public string ScheduleEmailJob(string to, string subject, string body, DateTime scheduledTime)
        {
            return BackgroundJob.Schedule(() => _emailService.SendEmailAsync(to, subject, body, false), scheduledTime);
        }

        public string EnqueueRecurringJob(string jobId, string cronExpression)
        {
            // Create a recurring job that sends a daily report
            RecurringJob.AddOrUpdate(jobId, () => SendDailyReport(), cronExpression);
            return jobId;
        }

        public string EnqueueCustomRecurringJob(string jobId, string cronExpression, string to, string subject, string body)
        {
            // Create a recurring job with custom email parameters
            RecurringJob.AddOrUpdate(jobId, () => SendCustomRecurringEmail(to, subject, body), cronExpression);
            return jobId;
        }

        public async Task SendDailyReport()
        {
            // This is an example recurring job
            // You can implement your own recurring job logic here
            await _emailService.SendNotificationEmailAsync(
                "a7mad.hossam2299@gmail.com", 
                "Daily Report", 
                "This is your daily report from TDP application.");
        }

        public async Task SendCustomRecurringEmail(string to, string subject, string body)
        {
            // Send a custom recurring email
            await _emailService.SendEmailAsync(to, subject, body, true);
        }

        public bool DeleteJob(string jobId)
        {
            try
            {
                BackgroundJob.Delete(jobId);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 