using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApi_Demo.Dtos;
using WebApi_Demo.Models;

namespace WebApi_Demo.Repository
{
    public class DepartmentRepository :IDepartmentRepository
    {
        private readonly ApplicationEntity context;
        public DepartmentRepository(ApplicationEntity context)
        {
            this.context = context;
        }
        public async Task<List<Department>> GetAll()
        {
            var departments = await context.Department.OrderBy(x => x.Name).ToListAsync();
            return departments;
        }
        public async Task<Department> GetDepartmentWithEmployeesById(int id)
        {
            var department = await context.Department.Include(d => d.Employees).FirstOrDefaultAsync(d => d.Id == id);
            return department!;
        }
        public async Task<Department> GetDepartmentById(int id)
        {
            var department = await context.Department.FirstOrDefaultAsync(d => d.Id == id);
            return department!;
        }
        public async Task AddDepartment(DepartmentDto newDepartment)
        {
            var department = new Department();
            department.Name = newDepartment.DepartmentName;
            department.DepartmentManager = newDepartment.DepartmentManager;
            await context.Department.AddAsync(department);
            await context.SaveChangesAsync();
        }
        public async Task UpdateDepartment(int id, DepartmentDto department)
        {
            var oldDepartment = await context.Department.FirstOrDefaultAsync(c => c.Id == id);

            if (oldDepartment != null)
            {
                oldDepartment.Name = department.DepartmentName;
                oldDepartment.DepartmentManager = department.DepartmentManager;
            }
            await context.SaveChangesAsync();
        }
        public async Task DeleteDepartment(int id)
        {
            var department = await context.Department.FirstOrDefaultAsync(c => c.Id == id);
            if (department is not null)
                context.Department.Remove(department);
            await context.SaveChangesAsync();

        }
    }
}
