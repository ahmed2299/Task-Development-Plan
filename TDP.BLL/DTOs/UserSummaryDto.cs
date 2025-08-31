namespace TDP.BLL.DTOs
{
    public class UserSummaryDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName {  get; set; } // Only basic department info
    }
}

