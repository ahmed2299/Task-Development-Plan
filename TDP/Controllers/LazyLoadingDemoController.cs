using Microsoft.AspNetCore.Mvc;
using TDP.BLL.Services;
using TDP.BLL.Repository;
using TDP.BLL.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace TDP.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LazyLoadingDemoController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUsersRepo _userRepo;
        private readonly ILogger<LazyLoadingDemoController> _logger;

        public LazyLoadingDemoController(IUserService userService, IUsersRepo userRepo, ILogger<LazyLoadingDemoController> logger)
        {
            _userService = userService;
            _userRepo = userRepo;
            _logger = logger;
        }

        /// <summary>
        /// Demo: Compare Eager Loading vs Lazy Loading performance
        /// </summary>
        /// <returns>Performance comparison results</returns>
        [HttpGet("performance-comparison")]
        public async Task<IActionResult> PerformanceComparison()
        {
            var stopwatch = new Stopwatch();
            var results = new List<object>();

            // Test 1: Eager Loading (Include)
            stopwatch.Start();
            var eagerUsers = await _userRepo.GetAllUsers();
            var eagerCount = eagerUsers.Count();
            stopwatch.Stop();
            
            results.Add(new
            {
                Method = "Eager Loading (Include)",
                UserCount = eagerCount,
                ExecutionTime = stopwatch.ElapsedMilliseconds,
                DepartmentDataLoaded = eagerUsers.All(u => u.Department != null),
                MemoryUsage = "Higher (all data loaded)"
            });

            // Test 2: Lazy Loading (No Include)
            stopwatch.Restart();
            var lazyUsers = await _userRepo.GetAllUsersLazy();
            var lazyCount = lazyUsers.Count();
            
            // Access department data to trigger lazy loading
            var departments = lazyUsers.Select(u => u.Department?.NameEN).ToList();
            stopwatch.Stop();
            
            results.Add(new
            {
                Method = "Lazy Loading (No Include)",
                UserCount = lazyCount,
                ExecutionTime = stopwatch.ElapsedMilliseconds,
                DepartmentDataLoaded = departments.All(d => d != null),
                MemoryUsage = "Lower (data loaded on demand)"
            });

            return Ok(new
            {
                Comparison = results,
                Summary = "Lazy loading loads related data only when accessed, while eager loading loads everything upfront."
            });
        }

        /// <summary>
        /// Demo: Show lazy loading in action
        /// </summary>
        /// <param name="userId">User ID to demonstrate lazy loading</param>
        /// <returns>Step-by-step lazy loading demonstration</returns>
        [HttpGet("lazy-loading-demo/{userId}")]
        public async Task<IActionResult> LazyLoadingDemo(int userId)
        {
            var demo = new List<object>();

            // Step 1: Load user without related data
            var user = await _userRepo.GetUserByIdLazyAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            demo.Add(new
            {
                Step = 1,
                Action = "Load user without related data",
                UserData = new { user.ID, user.Name, user.Email, user.DepartmentID },
                DepartmentData = user.Department?.NameEN ?? "Not loaded yet",
                DatabaseQueries = "1 query (user only)"
            });

            // Step 2: Access department (triggers lazy loading)
            var departmentName = user.Department?.NameEN ?? "Unknown";
            
            demo.Add(new
            {
                Step = 2,
                Action = "Access user.Department (triggers lazy loading)",
                UserData = new { user.ID, user.Name, user.Email, user.DepartmentID },
                DepartmentData = departmentName,
                DatabaseQueries = "2 queries (user + department)"
            });

            // Step 3: Access department again (no additional query)
            var departmentNameAgain = user.Department?.NameEN ?? "Unknown";
            
            demo.Add(new
            {
                Step = 3,
                Action = "Access user.Department again (cached)",
                UserData = new { user.ID, user.Name, user.Email, user.DepartmentID },
                DepartmentData = departmentNameAgain,
                DatabaseQueries = "2 queries (department data cached)"
            });

            return Ok(new
            {
                UserId = userId,
                DemoSteps = demo,
                Explanation = "Lazy loading automatically loads related data when first accessed and caches it for subsequent access."
            });
        }

        /// <summary>
        /// Demo: Pagination with lazy loading
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Items per page</param>
        /// <returns>Paginated results with lazy loading</returns>
        [HttpGet("pagination-demo")]
        public async Task<IActionResult> PaginationDemo([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
        {
            var result = await _userService.GetUsersPaginatedAsync(page, pageSize);
            
            return Ok(new
            {
                PaginationInfo = new
                {
                    result.Page,
                    result.PageSize,
                    result.TotalCount,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                },
                Data = result.Data,
                LazyLoadingBenefits = new
                {
                    InitialLoad = "Fast (only basic user data)",
                    RelatedData = "Loaded on demand when accessed",
                    MemoryEfficiency = "Only loads what's needed",
                    Scalability = "Better for large datasets"
                }
            });
        }

        /// <summary>
        /// Demo: Selective data loading strategies
        /// </summary>
        /// <returns>Different data loading strategies comparison</returns>
        [HttpGet("loading-strategies")]
        public async Task<IActionResult> LoadingStrategies()
        {
            var strategies = new List<object>();

            // Strategy 1: Lightweight summary
            var summaryUsers = await _userService.GetUsersPaginatedAsync(1, 3);
            strategies.Add(new
            {
                Strategy = "Lightweight Summary",
                Description = "Load only essential user data",
                Data = summaryUsers.Data,
                UseCase = "User lists, search results, dashboards"
            });

            // Strategy 2: Lazy loading with on-demand access
            if (summaryUsers.Data.Any())
            {
                var firstUser = summaryUsers.Data.First();
                var detailedUser = await _userService.GetUserDetailsLazyAsync(firstUser.ID);
                
                strategies.Add(new
                {
                    Strategy = "Lazy Loading with On-Demand Access",
                    Description = "Load basic data first, related data when needed",
                    BasicData = new { firstUser.ID, firstUser.Name, firstUser.Email },
                    RelatedData = detailedUser != null ? "Department data loaded on demand" : "No related data",
                    UseCase = "User profiles, detailed views, progressive loading"
                });
            }

            // Strategy 3: Explicit loading
            if (summaryUsers.Data.Any())
            {
                var firstUser = summaryUsers.Data.First();
                var userWithDept = await _userService.LoadUserWithDepartmentAsync(firstUser.ID);
                
                strategies.Add(new
                {
                    Strategy = "Explicit Loading",
                    Description = "Load specific related data when explicitly requested",
                    Data = userWithDept != null ? "User + Department data loaded" : "No data",
                    UseCase = "Specific data requirements, performance-critical operations"
                });
            }

            return Ok(new
            {
                Strategies = strategies,
                Recommendation = "Choose strategy based on your specific use case and performance requirements."
            });
        }
    }
}



