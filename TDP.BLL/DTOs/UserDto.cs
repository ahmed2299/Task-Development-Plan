using System;

namespace TDP.BLL.DTOs
{
    public class UserDto
    {
        public Guid IDGuid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentNameEN { get; set; }
        public string DepartmentNameAR { get; set; }
        public bool IsActive { get; set; }
    }
}