namespace TDP.BLL.Services
{
    public interface IBackgroundJobService
    {
        string EnqueueEmailJob(string to, string subject, string body);
        string EnqueueWelcomeEmailJob(string to, string userName);
        string EnqueuePasswordResetEmailJob(string to, string resetLink);
        string ScheduleEmailJob(string to, string subject, string body, DateTime scheduledTime);
        string EnqueueRecurringJob(string jobId, string cronExpression);
        string EnqueueCustomRecurringJob(string jobId, string cronExpression, string to, string subject, string body);
        bool DeleteJob(string jobId);
    }
} 