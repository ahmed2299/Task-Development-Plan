using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDP.BLL.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public Guid IDGuid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public DateTime BirthDate { get; set; }
        public int DepartmentID { get; set; }
        public bool IsActive { get; set; }

        // Navigation property - MUST be virtual for lazy loading proxies
        public virtual Department Department { get; set; }
    }
}
