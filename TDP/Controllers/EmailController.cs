using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDP.BLL.Services;

namespace TDP.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IBackgroundJobService _backgroundJobService;

        public EmailController(IEmailService emailService, IBackgroundJobService backgroundJobService)
        {
            _emailService = emailService;
            _backgroundJobService = backgroundJobService;
        }

        /// <summary>
        /// Send a test email immediately
        /// </summary>
        [HttpPost("send-test")]
        [AllowAnonymous]
        public async Task<IActionResult> SendTestEmail([FromBody] TestEmailDto request)
        {
            var result = await _emailService.SendEmailAsync(request.To, request.Subject, request.Body);
            
            return Ok(new { 
                success = result, 
                message = result ? "Email sent successfully" : "Email sending failed",
                timestamp = DateTime.UtcNow 
            });
        }

        /// <summary>
        /// Send a test email as background job
        /// </summary>
        [HttpPost("send-test-background")]
        [AllowAnonymous]
        public IActionResult SendTestEmailBackground([FromBody] TestEmailDto request)
        {
            var jobId = _backgroundJobService.EnqueueEmailJob(request.To, request.Subject, request.Body);
            
            return Ok(new { 
                success = true, 
                message = "Email job queued successfully",
                jobId = jobId,
                timestamp = DateTime.UtcNow 
            });
        }

        /// <summary>
        /// Schedule an email to be sent later
        /// </summary>
        [HttpPost("schedule-email")]
        [AllowAnonymous]
        public IActionResult ScheduleEmail([FromBody] ScheduleEmailDto request)
        {
            var scheduledTime = DateTime.UtcNow.AddMinutes(request.DelayMinutes);
            var jobId = _backgroundJobService.ScheduleEmailJob(request.To, request.Subject, request.Body, scheduledTime);
            
            return Ok(new { 
                success = true, 
                message = "Email scheduled successfully",
                jobId = jobId,
                scheduledTime = scheduledTime,
                timestamp = DateTime.UtcNow 
            });
        }

        /// <summary>
        /// Create a recurring job (example: daily report)
        /// </summary>
        [HttpPost("create-recurring-job")]
        [AllowAnonymous]
        public IActionResult CreateRecurringJob([FromBody] RecurringJobDto request)
        {
            var jobId = _backgroundJobService.EnqueueRecurringJob(request.JobId, request.CronExpression);
            
            return Ok(new { 
                success = true, 
                message = "Recurring job created successfully",
                jobId = jobId,
                cronExpression = request.CronExpression,
                description = GetCronDescription(request.CronExpression),
                timestamp = DateTime.UtcNow 
            });
        }

        /// <summary>
        /// Get Hangfire dashboard URL
        /// </summary>
        [HttpGet("hangfire-url")]
        [AllowAnonymous]
        public IActionResult GetHangfireUrl()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            return Ok(new { 
                hangfireUrl = $"{baseUrl}/hangfire",
                message = "Access Hangfire dashboard to monitor background jobs"
            });
        }

        private string GetCronDescription(string cronExpression)
        {
            return cronExpression switch
            {
                "0 9 * * *" => "Daily at 9:00 AM",
                "0 0 * * 0" => "Weekly on Sunday at midnight",
                "0 0 1 * *" => "Monthly on the 1st at midnight",
                _ => "Custom schedule"
            };
        }
    }

    public class TestEmailDto
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class ScheduleEmailDto
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int DelayMinutes { get; set; }
    }

    public class RecurringJobDto
    {
        public string JobId { get; set; }
        public string CronExpression { get; set; }
    }
} 