using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TDP.BLL.DTOs;
using TDP.BLL.Models;

namespace TDP.BLL.Repository
{
    public interface IUsersRepo
    {
        Task<IEnumerable<Employee>> GetAllUsers();
        Task<IEnumerable<Employee>> GetAllUsersLazy(); // New: Lazy loading version
        Task<Employee> CreateUser(Employee employee);
        Task<bool> EmailExistsAsync(string email);
        Task<Department?> GetDepartmentByIdAsync(int departmentId);
        Task<Employee?> GetUserByEmailAsync(string email);
        Task<Employee?> GetUserByIdAsync(int id);
        Task<Employee?> GetUserByIdLazyAsync(int id); // New: Lazy loading version
        Task<Employee> UpdateUser(Employee employee);
        
        // New: Load related data on demand
        Task<Employee> LoadUserWithDepartmentAsync(int id);
        Task<Employee> LoadUserWithAllDataAsync(int id);
    }
    public class UsersRepo : IUsersRepo
    {
        private readonly TdpDbContext _context;
        public UsersRepo(TdpDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllUsers()
        {
            return await _context.Employees.
                Include(e => e.Department)
                .ToListAsync();
        }

        // New: Lazy loading version - only loads basic user data
        public async Task<IEnumerable<Employee>> GetAllUsersLazy()
        {
            return await _context.Employees.ToListAsync(); // No Include - lazy loading will handle related data
        }

        public async Task<Employee> CreateUser(Employee user)
        {
            _context.Employees.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Employee?> GetUserByIdAsync(int id)
        {
            return await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.ID == id);
        }

        // New: Lazy loading version - only loads basic user data
        public async Task<Employee?> GetUserByIdLazyAsync(int id)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.ID == id); // No Include - lazy loading will handle related data
        }

        // New: Load department data on demand
        public async Task<Employee> LoadUserWithDepartmentAsync(int id)
        {
            var user = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.ID == id);
            
            if (user == null)
                throw new InvalidOperationException($"User with ID {id} not found");
                
            return user;
        }

        // New: Load all related data on demand
        public async Task<Employee> LoadUserWithAllDataAsync(int id)
        {
            var user = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.ID == id);
            
            if (user == null)
                throw new InvalidOperationException($"User with ID {id} not found");
                
            return user;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Employees.AnyAsync(e=> e.Email.ToLower()==email.ToLower());
        }

        public async Task<Department?> GetDepartmentByIdAsync(int departmentId)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.ID == departmentId);
        }

        public async Task<Employee?> GetUserByEmailAsync(string email)
        {
            return await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());
        }

        public async Task<Employee> UpdateUser(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return employee;
        }
    }
}
