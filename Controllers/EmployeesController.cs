using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_Demo.Dtos;
using WebApi_Demo.Repository;

namespace WebApi_Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository employeeRepository;
    public EmployeesController(IEmployeeRepository employeeRepository)
    {
        this.employeeRepository = employeeRepository;
    }
    [HttpGet] 
    public async Task<ActionResult<List<EmployeesDto>>> GetAll()
    {
        var employees = await employeeRepository.GetAll();
        return (Ok(employees));
    }
    [HttpGet("GetById/{id}", Name = "EmployeeDetailsRoute")]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeById([FromRoute] int id)
    {
        var employee = await employeeRepository.GetEmployeeById(id);
        var EmployeeDto = new EmployeeDto();
        EmployeeDto.Id = employee.Id;
        EmployeeDto.Name = employee.Name;
        EmployeeDto.Address = employee.Address;
        EmployeeDto.Age = employee.Age;
        return Ok(EmployeeDto);
    }
    [HttpGet("EmployeeWithDepartment/{id}")]
    public async Task<ActionResult<EmployeesDto>> GetEmployeeWithDepartmentName(int id)
    {
        var employee = await employeeRepository.GetEmployeeById(id);
        var EmployeeWithNameDto = new EmployeeWithDepartmentNameDto();
        EmployeeWithNameDto.EmployeeId = employee.Id;
        EmployeeWithNameDto.EmployeeName = employee.Name;
        EmployeeWithNameDto.DepartmentName = employee.Department?.Name;
        return Ok(EmployeeWithNameDto);
    }
    [HttpPost("Create")]
    public async Task<ActionResult<EmployeesDto>> PostEmployee(EmployeesDto employee)
    {
        if (ModelState.IsValid)
        {
            await employeeRepository.AddEmployee(employee);
            string url = Url.Link("EmployeeDetailsRoute", new { id = employee.Id })!;
            return Created(url, employee);
        }
        return BadRequest(ModelState);
    }
    [HttpPut("Update/{id}")] 
    public async Task<ActionResult<EmployeesDto>> PutEmployee([FromRoute] int id, [FromBody] EmployeesDto employee)
    {
        if (ModelState.IsValid)
        {
            await employeeRepository.UpdateEmployee(id, employee);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        return BadRequest(ModelState);
    }
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult<EmployeesDto>> DeleteEmployee([FromRoute] int id)
    {
        try
        {
            await employeeRepository.DeleteEmployee(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }

    }
}
