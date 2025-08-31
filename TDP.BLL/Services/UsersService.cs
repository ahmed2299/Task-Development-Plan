using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TDP.BLL.DTOs;
using TDP.BLL.Models;
using TDP.BLL.Repository;
using AutoMapper;

namespace TDP.BLL.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<RegisterResponseDto> RegisterUserAsync(RegisterDto registerDto);
        Task<LoginResponseDto> LoginUserAsync(LoginDto loginDto);
        Task<UpdateResponseDto> UpdateUserInfoAsync(int userId, UpdateUserInfoDto updateDto);
        Task<UpdateResponseDto> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<UserDto> GetUserDetailsAsync(int userId);
        
        // New: Lazy loading methods
        Task<PaginationDto<UserSummaryDto>> GetUsersPaginatedAsync(int page, int pageSize);
        Task<UserSummaryDto> GetUserSummaryAsync(int userId);
        Task<UserDto> GetUserDetailsLazyAsync(int userId);
        Task<UserDto> LoadUserWithDepartmentAsync(int userId);
    }

    public class UserService : IUserService
    {
        private readonly IUsersRepo _userRepo;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobService _backgroundJobService;

        public UserService(IUsersRepo userRepo, IJwtService jwtService, IMapper mapper, IBackgroundJobService backgroundJobService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
            _mapper = mapper;
            _backgroundJobService = backgroundJobService;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllUsers();

            // Use AutoMapper if available, otherwise manual mapping
            try
            {
                return _mapper.Map<IEnumerable<UserDto>>(users);
            }
            catch
            {
                // Fallback to manual mapping
                return users.Select(e => new UserDto
            {
                IDGuid = e.IDGuid,
                Name = e.Name,
                Email = e.Email,
                BirthDate = e.BirthDate,
                DepartmentID = e.DepartmentID,
                DepartmentNameEN = e.Department.NameEN,
                DepartmentNameAR = e.Department.NameAR,
                IsActive = e.IsActive
            });
            }
        }

        // New: Paginated users with lazy loading
        public async Task<PaginationDto<UserSummaryDto>> GetUsersPaginatedAsync(int page, int pageSize)
        {
            var totalCount = await _userRepo.GetAllUsers().ContinueWith(t => t.Result.Count());
            var users = await _userRepo.GetAllUsersLazy(); // Use lazy loading version
            
            var paginatedUsers = users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new UserSummaryDto
                {
                    ID = e.ID,
                    Name = e.Name,
                    Email = e.Email,
                    IsActive = e.IsActive,
                    DepartmentID = e.DepartmentID,
                    DepartmentName = e.Department?.NameEN ?? "Unknown" // Lazy loading will populate this
                });

            return new PaginationDto<UserSummaryDto>(paginatedUsers, page, pageSize, totalCount);
        }

        // New: Get user summary (lightweight)
        public async Task<UserSummaryDto> GetUserSummaryAsync(int userId)
        {
            var user = await _userRepo.GetUserByIdLazyAsync(userId);
            if (user == null) return null;

            return new UserSummaryDto
            {
                ID = user.ID,
                Name = user.Name,
                Email = user.Email,
                IsActive = user.IsActive,
                DepartmentID = user.DepartmentID,
                DepartmentName = user.Department?.NameEN ?? "Unknown" // Lazy loading will populate this
            };
        }

        // New: Get user details with lazy loading
        public async Task<UserDto> GetUserDetailsLazyAsync(int userId)
        {
            var user = await _userRepo.GetUserByIdLazyAsync(userId);
            if (user == null) return null;

            // Lazy loading will automatically load Department when accessed
            return _mapper.Map<UserDto>(user);
        }

        // New: Load user with department on demand
        public async Task<UserDto> LoadUserWithDepartmentAsync(int userId)
        {
            var user = await _userRepo.LoadUserWithDepartmentAsync(userId);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<LoginResponseDto> LoginUserAsync(LoginDto loginDto)
        {
            try
            {
                // Find user by email
                var user = await _userRepo.GetUserByEmailAsync(loginDto.Email.ToLower());

                if (user == null)
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                // Check if account is active
                if (!user.IsActive)
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Account is deactivated. Please contact administrator."
                    };
                }

                // Verify password
                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.HashedPassword))
                {
                    return new LoginResponseDto
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                // Generate JWT token
                var token = _jwtService.GenerateToken(user);
                var tokenExpiration = _jwtService.GetTokenExpiration();

                return new LoginResponseDto
                {
                    Success = true,
                    Message = "Login successful",
                    Token = token,
                    TokenExpiration = tokenExpiration,
                    User = _mapper.Map<UserDto>(user)
                };
            }
            catch (Exception ex)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "An error occurred during login"
                };
            }
        }

        public async Task<RegisterResponseDto> RegisterUserAsync(RegisterDto registerDto)
        {
            if (await _userRepo.EmailExistsAsync(registerDto.Email))
            {
                return new RegisterResponseDto
                {
                    Success = false,
                    Message = "Email address is already registered"
                };
            }

            var department = await _userRepo.GetDepartmentByIdAsync(registerDto.DepartmentID);

            if (department == null)
            {
                return new RegisterResponseDto
                {
                    Success = false,
                    Message = "Invalid department selected"
                };
            }

            var newEmployee = _mapper.Map<Employee>(registerDto);
            newEmployee.IDGuid = Guid.NewGuid();
            newEmployee.HashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            newEmployee.IsActive = true;

            // Save to database
            var createUser = await _userRepo.CreateUser(newEmployee);

            // Get the user with department info for response
            var userWithDepartment = await _userRepo.GetUserByIdAsync(createUser.ID);

            // Send welcome email in background
            _backgroundJobService.EnqueueWelcomeEmailJob(registerDto.Email, registerDto.Name);

            return new RegisterResponseDto
            {
                Success = true,
                Message = "User registered successfully",
                User = _mapper.Map<UserDto>(userWithDepartment)
            };
        }

        public async Task<UpdateResponseDto> UpdateUserInfoAsync(int userId, UpdateUserInfoDto updateDto)
        {
            try
            {
                // Get the current user
                var currentUser = await _userRepo.GetUserByIdAsync(userId);
                if (currentUser == null)
                {
                    return new UpdateResponseDto
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                // Check if email is already taken by another user
                if (await _userRepo.EmailExistsAsync(updateDto.Email) && 
                    currentUser.Email.ToLower() != updateDto.Email.ToLower())
                {
                    return new UpdateResponseDto
                    {
                        Success = false,
                        Message = "Email address is already registered by another user"
                    };
                }

                // Verify department exists
                var department = await _userRepo.GetDepartmentByIdAsync(updateDto.DepartmentID);
                if (department == null)
                {
                    return new UpdateResponseDto
                    {
                        Success = false,
                        Message = "Invalid department selected"
                    };
                }

                // Update user information using AutoMapper
                _mapper.Map(updateDto, currentUser);

                // Save changes
                await _userRepo.UpdateUser(currentUser);

                // Get updated user with department info
                var updatedUser = await _userRepo.GetUserByIdAsync(userId);

                return new UpdateResponseDto
                {
                    Success = true,
                    Message = "User information updated successfully",
                    User = _mapper.Map<UserDto>(updatedUser)
                };
            }
            catch (Exception ex)
            {
                return new UpdateResponseDto
                {
                    Success = false,
                    Message = "An error occurred while updating user information"
                };
            }
        }

        public async Task<UpdateResponseDto> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            try
            {
                // Get the current user
                var currentUser = await _userRepo.GetUserByIdAsync(userId);
                if (currentUser == null)
                {
                    return new UpdateResponseDto
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                // Verify current password
                if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, currentUser.HashedPassword))
                {
                    return new UpdateResponseDto
                    {
                        Success = false,
                        Message = "Current password is incorrect"
                    };
                }

                // Hash new password
                currentUser.HashedPassword = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);

                // Save changes
                await _userRepo.UpdateUser(currentUser);

                // Get updated user with department info
                var updatedUser = await _userRepo.GetUserByIdAsync(userId);

                return new UpdateResponseDto
                {
                    Success = true,
                    Message = "Password changed successfully",
                    User = _mapper.Map<UserDto>(updatedUser)
                };
            }
            catch (Exception ex)
            {
                return new UpdateResponseDto
                {
                    Success = false,
                    Message = "An error occurred while changing password"
                };
            }
        }

        public async Task<UserDto> GetUserDetailsAsync(int userId)
        {
            try
            {
                var user = await _userRepo.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return null;
                }

                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
