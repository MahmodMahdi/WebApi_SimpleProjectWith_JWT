using Microsoft.AspNetCore.Mvc;
using WebApi_Demo.Dtos;
using WebApi_Demo.Models;

namespace WebApi_Demo.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAll();
        Task<Employee> GetEmployeeById(int id);
        Task AddEmployee(EmployeesDto employee);
        Task UpdateEmployee(int id ,EmployeesDto employee);
        Task DeleteEmployee(int id);
    }
}
