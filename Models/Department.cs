using System.ComponentModel.DataAnnotations;

namespace WebApi_Demo.Models;
public class Department
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? DepartmentManager { get; set; }
    public virtual List<Employee>? Employees { get; set; }
}
