using Microsoft.AspNetCore.Mvc;
using WebApi_Demo.Dtos;
using WebApi_Demo.Models;

namespace WebApi_Demo.Repository
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAll();
        Task<Department> GetDepartmentWithEmployeesById(int id);
        Task<Department> GetDepartmentById(int id);
        Task AddDepartment(DepartmentDto department);
        Task UpdateDepartment(int id, DepartmentDto department);
        Task DeleteDepartment(int id);
    }
}
