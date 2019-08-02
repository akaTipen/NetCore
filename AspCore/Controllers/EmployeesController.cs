using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspCore.Data;
using System.IO;

namespace AspCore.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListEmployee(int pageNumber = 0, int pageSize = 10)
        {
            int totalRows = 0;

            IEnumerable<EmployeesViewModel> result = (from emp in _context.Employees
                                                      join dep in _context.Departments on emp.DepartmentId equals dep.DepartmentId
                                                      select new EmployeesViewModel
                                                      {
                                                          EmployeeId = emp.EmployeeId,
                                                          EmployeeName = emp.EmployeeName,
                                                          JoinDate = convertDate(emp.JoinDate),
                                                          Weight = emp.Weight,
                                                          Height = emp.Height,
                                                          DepartmentId = emp.DepartmentId,
                                                          DepartmentName = dep.DepartmenName
                                                      }).ToList();

            totalRows = result.Count();
            var modulo = totalRows % pageSize;
            var pageCount = totalRows / pageSize + (modulo > 0 ? 1 : 0);
            if (pageSize >= 0 && pageNumber >= 0)
                result = result.Skip(pageSize * pageNumber).Take(pageSize).ToList();
            return Json(new { totalRows = totalRows, pageCount = pageCount, data = result });
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            var DepartementList = _context.Departments.Select(a => new { a.DepartmentId, a.DepartmenName }).ToList();
            List<SelectListItem> DepartementListItem = new SelectList(DepartementList, "DepartmentId", "DepartmenName").ToList();
            ViewBag.DepartementListItem = DepartementListItem;
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeesViewModel employee)
        {
            if (ModelState.IsValid)
            {
                byte[] fileBytes = null;
                if (employee.SendPhoto != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        employee.SendPhoto.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                }

                Employees temp = new Employees
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = employee.EmployeeName,
                    JoinDate = convertDate(employee.JoinDate),
                    Weight = employee.Weight,
                    Height = employee.Height,
                    Photo = fileBytes,
                    DepartmentId = employee.DepartmentId

                };
                _context.Add(temp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
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
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employees employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }

        #region helper
        private string convertDate(DateTime? param)
        {
            DateTime result = (DateTime)param;
            string[] ValSplit = param.ToString().Substring(0, 10).Split('/');
            string split = result.Day + "-" + result.Month + "-" + result.Year;
            return split;
        }

        private DateTime? convertDate(string param)
        {
            string[] ValSplit = param.ToString().Substring(0, 10).Split('/');
            DateTime result = new DateTime(Convert.ToInt32(ValSplit[2]), Convert.ToInt32(ValSplit[1]), Convert.ToInt32(ValSplit[0]));
            
            return result;
        }
        #endregion
    }
}
