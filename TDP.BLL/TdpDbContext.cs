using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TDP.BLL.Migrations;
using TDP.BLL.Models;

namespace TDP.BLL
{
    public class TdpDbContext : DbContext
    {
        public TdpDbContext(DbContextOptions<TdpDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
            // Enable lazy loading with proxies
            optionsBuilder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            //Seed data in the two tables
            modelBuilder.Entity<Department>().HasData(InitialSeed.GetDepartments());
            modelBuilder.Entity<Employee>().HasData(InitialSeed.GetEmployees());
        }
    }
}
