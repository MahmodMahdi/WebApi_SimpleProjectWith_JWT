using Microsoft.EntityFrameworkCore;
using WebApi_Demo.Dtos;
using WebApi_Demo.Models;

namespace WebApi_Demo.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationEntity context;
        public EmployeeRepository(ApplicationEntity context)
        {
            this.context = context;
        }
        public async Task<List<Employee>> GetAll()
        {
            var employees = await context.Employees.OrderBy(emp => emp.Name).ToListAsync();
            return employees;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var employee = await context.Employees.Include(x=>x.Department).FirstOrDefaultAsync(emp => emp.Id == id);
            return employee!;

        }
        public async Task AddEmployee(EmployeesDto employeeDto)
        {
            var employee = new Employee();
            employee.Name = employeeDto.Name;
            employee.Address = employeeDto.Address;
            employee.Age = employeeDto.Age;
            employee.Salary = employeeDto.Salary;
            employee.departmentId = employeeDto.departmentId;
            await context.Employees.AddAsync(employee);
            await context.SaveChangesAsync();
        }
        public async Task UpdateEmployee(int id ,EmployeesDto employee)
        {
            var oldEmployee =await context.Employees.FirstOrDefaultAsync(c => c.Id == id);
            if (oldEmployee != null)
            {
                oldEmployee.Name = employee.Name;
                oldEmployee.Address = employee.Address;
                oldEmployee.Age = employee.Age;
                oldEmployee.Salary = employee.Salary;
                oldEmployee.departmentId = employee.departmentId;
            }
            await context.SaveChangesAsync();
        }
        public async Task DeleteEmployee(int id)
        {
            var Employee =await context.Employees.FirstOrDefaultAsync(c => c.Id == id);
            if (Employee is not null)
                context.Employees.Remove(Employee);
           await context.SaveChangesAsync();
        }
    }
}
