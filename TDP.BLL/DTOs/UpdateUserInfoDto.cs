using System;

namespace TDP.BLL.DTOs
{
    public class UpdateUserInfoDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int DepartmentID { get; set; }
    }
} 