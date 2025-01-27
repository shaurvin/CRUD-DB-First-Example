using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebFinalCRDUFK.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int DeptId { get; set; }
        public string? DepartmentName { get; set; }
        public List<SelectListItem>? Departments { get; set; }
    }
}
