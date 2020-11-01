using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookListMVC.Controllers
{
    public class TodosController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Todo Todo { get; set; }

        public TodosController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Todo = new Todo();
            if (id == null)
            {
                //create
                return View(Todo);
            }
            //update
            Todo = _db.Todos.FirstOrDefault(u => u.Id == id);
            if (Todo == null)
            {
                return NotFound();
            }
            return View(Todo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Todo todo)
        {
            if (ModelState.IsValid)
            {
                if (Todo.Id == 0)
                {
                    //create
                    _db.Todos.Add(Todo);
                }
                else
                {
                    _db.Todos.Update(Todo);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Todo);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Todos.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var todoFromDb = await _db.Todos.FirstOrDefaultAsync(u => u.Id == id);
            if (todoFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Todos.Remove(todoFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
