using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi_Demo.Models;
public class Employee
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Salary { get; set; }
    public string? Address { get; set; }
    public int Age { get; set; }
    public int? departmentId { get; set; }
    public Department? Department { get; set; }
}
