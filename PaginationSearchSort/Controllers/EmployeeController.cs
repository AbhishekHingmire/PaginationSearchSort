using Microsoft.AspNetCore.Mvc;
using PaginationSearchSort.Data;
using PaginationSearchSort.Models;

namespace PaginationSearchSort.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult AddEmployees(Employee emp)
        {
            //var employeeList = new List<Employee> {
            //    new Employee{Email = "john@gmail.com", Name = "John"},
            //    new Employee{Email = "kane@gmail.com", Name = "Kane"},
            //    new Employee{Email = "izabella@gmail.com", Name = "Izabella"},
            //};
            _context.Employee.Add(emp);
            _context.SaveChanges();
            return View("Employees");
        }

        public IActionResult Employees(string searchTerm = "", string orderBy = "", int currentPage = 1)
        {
            searchTerm = string.IsNullOrEmpty(searchTerm)?"":searchTerm.ToLower(); // Suppose someone searches will null search in input the keep it null

            var empData = new EmployeeViewModel();

            empData.OrderByName = string.IsNullOrEmpty(orderBy) ? "name_desc" : ""; // When pass then set to null so when user click second time it will show previous result (toggle)
            empData.OrderByEmail = orderBy == "email" ? "email_desc" : "email"; /* Initially it will email_desc, so when email_desc cliekcd from UI
                                                                                 It will be set to email only so we can se proper toggle effect */

            var employees = (from emp in _context.Employee
                                 where emp.Name.ToLower().StartsWith(searchTerm) || emp.Email.ToLower().StartsWith(searchTerm)
                                 orderby orderBy
                                 select new Employee
                                 {
                                     Id = emp.Id,
                                     Email = emp.Email,
                                     Name = emp.Name
                                 }
                                );

            switch(orderBy)
            {
                case "email_desc":
                    employees = employees.OrderByDescending( emp => emp.Email );
                    break;
                case "name_desc":
                    employees = employees.OrderByDescending(emp => emp.Name);
                    break;
                case "email":
                    employees = employees.OrderBy(emp => emp.Email);
                    break;
                default:
                    employees = employees.OrderBy(emp => emp.Name);
                    break;
            }

            int totalRecords = employees.Count(); // suppose 11
            int pageSize = 5; // suppose it is pageSize = 5 records per page
            float calc = ((float)totalRecords/pageSize); //  11 / 5 = 2.222
            int totalPages = (int)Math.Ceiling(calc); // So 2.22 will become 3 due to Ceiling, we will add 3 pages and 5 records each page to show 11 records

            //Logic for page skip i.e(when you click of 2 page or 5 page directly via UI)
            employees = employees.Skip((currentPage - 1) * pageSize).Take(pageSize); // Only take records of Page size suppose 10 records but size is 2 then take only 2 records
                                                                                     // 1-1 = 0 * 2 = 0 ; so initially it will not skip any records and take first 2 records
                                                                                     //if currentPage = 2; 2 - 1 = 1 * 2 = 2. so It will skip 2 records and show 3rd and 4th record as we passed 2nd page.
            empData.Employees = employees;

            empData.TotalPages = totalPages;
            empData.CurrentPage = currentPage;
            empData.PageSize = pageSize;
            empData.SearchTerm = searchTerm;
            empData.OrderBy = orderBy;


            return View(empData);
        }
    }
}
