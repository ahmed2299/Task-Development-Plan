using Microsoft.AspNetCore.Mvc;
using TDP.BLL.Services;
using log4net;

namespace TDP.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IUserService _userService;
        private static readonly ILog _log4netLogger = LogManager.GetLogger(typeof(TestController));

        public TestController(ILogger<TestController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Test controller is working!", timestamp = DateTime.UtcNow });
        }

        [HttpGet("service-test")]
        public IActionResult TestServiceRegistration()
        {
            try
            {
                // This will test if IUserService is properly registered
                var serviceType = _userService.GetType().Name;
                return Ok(new { 
                    message = "Service registration is working!", 
                    serviceType = serviceType,
                    timestamp = DateTime.UtcNow 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                    message = "Service registration failed!", 
                    error = ex.Message,
                    timestamp = DateTime.UtcNow 
                });
            }
        }

        [HttpGet("test-logging")]
        public IActionResult TestLogging()
        {
            try
            {
                // Test different log levels
                _log4netLogger.Debug("This is a DEBUG message from TestController");
                _log4netLogger.Info("This is an INFO message from TestController");
                _log4netLogger.Warn("This is a WARNING message from TestController");
                _log4netLogger.Error("This is an ERROR message from TestController");
                
                // Test with structured data
                _log4netLogger.Info($"Logging test executed at {DateTime.UtcNow} by {User?.Identity?.Name ?? "Anonymous"}");
                
                // Test exception logging
                try
                {
                    throw new InvalidOperationException("This is a test exception for logging");
                }
                catch (Exception ex)
                {
                    _log4netLogger.Error("Caught test exception", ex);
                }
                
                return Ok(new { 
                    message = "Logging test completed successfully!", 
                    instructions = "Check console output and Logs/TDP.log file for log entries",
                    timestamp = DateTime.UtcNow 
                });
            }
            catch (Exception ex)
            {
                _log4netLogger.Error("Error occurred during logging test", ex);
                return BadRequest(new { 
                    message = "Logging test failed!", 
                    error = ex.Message,
                    timestamp = DateTime.UtcNow 
                });
            }
        }
    }
} 