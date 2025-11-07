using EmployeeManagementApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagementApp.Repositories
{
    public class InMemoryEmployeeRepository : IEmployeeRepository
    {
        private static List<Employee> employees = new List<Employee>();

        public IEnumerable<Employee> GetAllEmployees() => employees;

        public Employee GetEmployeeById(int id) => employees.FirstOrDefault(e => e.Id == id);

        public void AddEmployee(Employee employee) => employees.Add(employee);

        public void UpdateEmployee(Employee employee)
        {
            var existing = employees.FirstOrDefault(e => e.Id == employee.Id);
            if (existing != null)
            {
                existing.Name = employee.Name;
                existing.Department = employee.Department;
                existing.Email = employee.Email;
            }
        }

        public void DeleteEmployee(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee != null)
                employees.Remove(employee);
        }
    }
}
