using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi_Demo.Dtos;
using WebApi_Demo.Repository;

namespace WebApi_Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentRepository _departmentRepository;
    public DepartmentsController(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }
    [HttpGet]
    public async Task<ActionResult<List<DepartmentDto>>> GetAll()
    {
        return Ok(await _departmentRepository.GetAll());
    }
    [HttpGet("DepartmentWithEmployees/{id}")]
    public async Task<ActionResult<DepartmentWithEmployeesDto>> GetDepartmentWithEmployeesById(int id)
    {
        var department = await _departmentRepository.GetDepartmentWithEmployeesById(id);
        var departmentWithEmployees = new DepartmentWithEmployeesDto();
        departmentWithEmployees.DepartmentId = department!.Id;
        departmentWithEmployees.DepartmentName = department.Name;
        foreach (var item in department.Employees!)
        {
            departmentWithEmployees.Employees.Add(new AllEmployeeDto() { EmployeeId = item.Id, EmployeeName = item.Name });
        }
        return Ok(departmentWithEmployees);
    }

    [HttpGet("GetById/{id}", Name = "DepartmentDetailsRoute")]
    public async Task<ActionResult<DepartmentDto>> GetDepartmentById(int id)
    {
        var department = await _departmentRepository.GetDepartmentById(id);
        var departmentDto = new DepartmentDto();
        departmentDto.Id = department!.Id;
        departmentDto.DepartmentName = department.Name;
        departmentDto.DepartmentManager = department.DepartmentManager;
        return Ok(departmentDto);
    }
    [HttpPost("Create")]
    public async Task<ActionResult<DepartmentDto>> PostDepartment(DepartmentDto newDepartment)
    {
        if (ModelState.IsValid)
        {
            await _departmentRepository.AddDepartment(newDepartment);

            string url = Url.Link("DepartmentDetailsRoute", new { id = newDepartment.Id })!;
            return Created(url, newDepartment);
        }
        return BadRequest(ModelState);
    }
    [HttpPut("Update/{id}")]
    public async Task<ActionResult<DepartmentDto>> PutDepartment([FromRoute] int id, [FromBody] DepartmentDto department)
    {
        if (ModelState.IsValid)
        {
            await _departmentRepository.UpdateDepartment(id, department);
            return StatusCode(StatusCodes.Status204NoContent);
        }
        return BadRequest(ModelState);
    }
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult<DepartmentDto>> DeleteDepartment([FromRoute] int id)
    {
        try
        {
            await _departmentRepository.DeleteDepartment(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }

    }
}


