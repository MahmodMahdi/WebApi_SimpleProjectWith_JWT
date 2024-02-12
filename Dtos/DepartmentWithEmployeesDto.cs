namespace WebApi_Demo.Dtos;
public class DepartmentWithEmployeesDto
{
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public List<AllEmployeeDto> Employees { get; set; } = new List<AllEmployeeDto>();
}
public class AllEmployeeDto
{
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
}
