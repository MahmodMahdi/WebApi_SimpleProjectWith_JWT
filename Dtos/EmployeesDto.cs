using System.ComponentModel.DataAnnotations;
using WebApi_Demo.Models;

namespace WebApi_Demo.Dtos;
public class EmployeesDto
{
    public int Id { get; set; }
    [Required]
    [MinLength(3)]
    public string? Name { get; set; }
    [Range(2000, 10000)]
    public decimal Salary { get; set; }
    public string? Address { get; set; }
    public int Age { get; set; }
    public int? departmentId { get; set; }
}