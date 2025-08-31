using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDP.BLL.DTOs;
using TDP.BLL.Services;
using System.ComponentModel.DataAnnotations;

namespace TDP.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Get all registered users
        /// </summary>
        /// <returns>List of all users</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get users with pagination and lazy loading
        /// </summary>
        /// <param name="page">Page number (default: 1)</param>
        /// <param name="pageSize">Items per page (default: 10)</param>
        /// <returns>Paginated list of users with lazy loading</returns>
        [HttpGet("paginated")]
        public async Task<IActionResult> GetUsersPaginatedAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var result = await _userService.GetUsersPaginatedAsync(page, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Get user summary (lightweight) with lazy loading
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User summary with basic information</returns>
        [HttpGet("{id}/summary")]
        public async Task<IActionResult> GetUserSummaryAsync(int id)
        {
            var userSummary = await _userService.GetUserSummaryAsync(id);
            if (userSummary == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(userSummary);
        }

        /// <summary>
        /// Get user details with lazy loading (loads related data on demand)
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User details with lazy-loaded related data</returns>
        [HttpGet("{id}/details-lazy")]
        public async Task<IActionResult> GetUserDetailsLazyAsync(int id)
        {
            var userDetails = await _userService.GetUserDetailsLazyAsync(id);
            if (userDetails == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(userDetails);
        }

        /// <summary>
        /// Load user with department data on demand
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User with department information</returns>
        [HttpGet("{id}/with-department")]
        public async Task<IActionResult> LoadUserWithDepartmentAsync(int id)
        {
            var userDetails = await _userService.LoadUserWithDepartmentAsync(id);
            if (userDetails == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(userDetails);
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registerDto">User registration information</param>
        /// <returns>Registration result with user details</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _userService.RegisterUserAsync(registerDto);
            return Ok(result);
        }

        /// <summary>
        /// Login user and get JWT token
        /// </summary>
        /// <param name="loginDto">User login credentials</param>
        /// <returns>Login result with JWT token</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _userService.LoginUserAsync(loginDto);
            return Ok(result);
        }

        /// <summary>
        /// Update user information
        /// </summary>
        /// <param name="updateDto">User information to update</param>
        /// <returns>Update result with user details</returns>
        [HttpPut("update-info")]
        [Authorize]
        public async Task<IActionResult> UpdateInfo([FromBody] UpdateUserInfoDto updateDto)
        {
            // Get user ID from JWT token
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            var result = await _userService.UpdateUserInfoAsync(userId, updateDto);
            return Ok(result);
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="changePasswordDto">Password change information</param>
        /// <returns>Password change result</returns>
        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            // Get user ID from JWT token
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            var result = await _userService.ChangePasswordAsync(userId, changePasswordDto);
            return Ok(result);
        }

        /// <summary>
        /// Get current user details
        /// </summary>
        /// <returns>Current user details</returns>
        [HttpGet("details")]
        [Authorize]
        public async Task<IActionResult> UserDetails()
        {
            // Get user ID from JWT token
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            var userDetails = await _userService.GetUserDetailsAsync(userId);
            if (userDetails == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(userDetails);
        }

    }
}
