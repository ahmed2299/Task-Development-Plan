using TDP.BLL.Models;
using System;
using System.Collections.Generic;

namespace TDP.BLL
{
    public static class InitialSeed
    {
        public static List<Department> GetDepartments()
        {
            return new List<Department>
            {
                new Department { ID = 1, IDGuid = Guid.Parse("f7a9e8b0-3c1d-4a9f-8b2a-1e6c9d7f8a3b"), NameEN = "Human Resources", NameAR = "الموارد البشرية" },
                new Department { ID = 2, IDGuid = Guid.Parse("a3b8d4c1-9e7f-4b0d-9c3a-2e8d7f6a5b4c"), NameEN = "Information Technology", NameAR = "تقنية المعلومات" },
                new Department { ID = 3, IDGuid = Guid.Parse("c9d8e7f6-5a4b-4c3d-8b2a-1e6d9c7f8a3b"), NameEN = "Finance", NameAR = "المالية" }
            };
        }

        public static List<Employee> GetEmployees()
        {
            return new List<Employee>
            {
                new Employee
                {
                    ID = 1,
                    IDGuid = Guid.Parse("d1e2f3a4-b5c6-4d7e-8f9a-0b1c2d3e4f5a"),
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    HashedPassword = "hashed_password_placeholder_1", // Replace with a real hash
                    BirthDate = new DateTime(1990, 5, 15),
                    DepartmentID = 2, // Belongs to IT
                    IsActive = true
                },
                new Employee
                {
                    ID = 2,
                    IDGuid = Guid.Parse("f5a4e3d2-c1b0-4a9f-8e7d-6c5b4a3f2e1d"),
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    HashedPassword = "hashed_password_placeholder_2",
                    BirthDate = new DateTime(1988, 8, 22),
                    DepartmentID = 1, // Belongs to HR
                    IsActive = true
                },
                new Employee
                {
                    ID = 3,
                    IDGuid = Guid.Parse("a9b8c7d6-e5f4-4a3b-8c2d-1e9f8a7b6c5d"),
                    Name = "Peter Jones",
                    Email = "peter.jones@example.com",
                    HashedPassword = "hashed_password_placeholder_3",
                    BirthDate = new DateTime(1992, 1, 30),
                    DepartmentID = 2, // Belongs to IT
                    IsActive = false
                },
                new Employee
                {
                    ID = 4,
                    IDGuid = Guid.Parse("b3c2d1e0-f9a8-4e7d-6c5b-4a3f2e1d0c9b"),
                    Name = "Sara Miller",
                    Email = "sara.miller@example.com",
                    HashedPassword = "hashed_password_placeholder_4",
                    BirthDate = new DateTime(1995, 11, 10),
                    DepartmentID = 3, // Belongs to Finance
                    IsActive = true
                }
            };
        }
    }
}
