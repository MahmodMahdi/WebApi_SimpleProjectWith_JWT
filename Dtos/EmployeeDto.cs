using System.ComponentModel.DataAnnotations;

namespace WebApi_Demo.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string? Name { get; set; }
        [Range(2000, 10000)]
        public decimal Salary { get; set; }
        public string? Address { get; set; }
        public int Age { get; set; }
    }
}
