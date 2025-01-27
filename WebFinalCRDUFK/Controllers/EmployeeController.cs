using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebFinalCRDUFK.Models;

namespace WebFinalCRDUFK.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employee/Index
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.Dept)
                .Select(e => new EmployeeViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Gender = e.Gender,
                    DepartmentName = e.Dept.DeptName
                })
                .ToListAsync();

            return View(employees);
        }

        // GET: Employee/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new EmployeeViewModel
            {
                Departments = await _context.Departments.Select(d => new SelectListItem
                {
                    Value = d.DeptId.ToString(),
                    Text = d.DeptName
                }).ToListAsync()
            };
            return View(viewModel);
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    Name = model.Name,
                    Gender = model.Gender,
                    DeptId = model.DeptId
                };

                _context.Add(employee);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Employee Added successfully!";
                return RedirectToAction(nameof(Index));
            }

            model.Departments = await _context.Departments.Select(d => new SelectListItem
            {
                Value = d.DeptId.ToString(),
                Text = d.DeptName
            }).ToListAsync();

            return View(model);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var viewModel = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Gender = employee.Gender,
                DeptId = employee.DeptId,
                Departments = await _context.Departments.Select(d => new SelectListItem
                {
                    Value = d.DeptId.ToString(),
                    Text = d.DeptName
                }).ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var employee = await _context.Employees.FindAsync(id);
                    if (employee == null)
                    {
                        return NotFound();
                    }

                    employee.Name = model.Name;
                    employee.Gender = model.Gender;
                    employee.DeptId = model.DeptId;

                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Employee Updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            model.Departments = await _context.Departments.Select(d => new SelectListItem
            {
                Value = d.DeptId.ToString(),
                Text = d.DeptName
            }).ToListAsync();

            return View(model);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Dept)
                .Select(e => new EmployeeViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Gender = e.Gender,
                    DepartmentName = e.Dept.DeptName
                })
                .FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp != null)
            {
                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Employee Deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }

}
