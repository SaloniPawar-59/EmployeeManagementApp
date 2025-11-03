using Microsoft.AspNetCore.Mvc;
using EmployeeManagementApp.Models;
using System.Collections.Generic;

namespace EmployeeManagementApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var employees = new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe", Department = "HR", Salary = 50000 },
                new Employee { Id = 2, Name = "Jane Smith", Department = "Finance", Salary = 60000 },
                new Employee { Id = 3, Name = "Sam Johnson", Department = "IT", Salary = 70000 }
            };

            return View(employees);
        }
    }
}
