using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.AspNetCore.Htmx.Data;
using Demo.AspNetCore.Htmx.Models;
using Demo.AspNetCore.Htmx.Extensions;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Demo.AspNetCore.Htmx.Helper;

namespace Demo.AspNetCore.Htmx.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string IdForm = "idCategoryForm";

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            // see the loading spinner (Remove in entire file for production)
            Thread.Sleep(TimeSpan.FromSeconds(1));

            var items = await _context.Categories.OrderBy(x => x.CategoryName).ToListAsync();

            if (Request.IsHtmxRequest())
            {
                return PartialView(items);
            }
            else
            {
                return View(items);
            }
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (id == null || _context.Categories == null)
            {
                return Problem("Category not found", statusCode: 404, title: "Error");
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return Problem("Category not found", statusCode: 404, title: "Error");
            }

            return PartialView(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            ViewBag.idForm = IdForm;
            Response.AddHxHeader(IdForm, HxModeEnum.initValid);
            return PartialView();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName")] Category category)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.idForm = IdForm;
            Response.AddHxHeader(IdForm, HxModeEnum.initValid);
            return PartialView(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (id == null || _context.Categories == null)
            {
                return Problem("Category not found", statusCode: 404, title: "Error");
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return Problem("Category not found", statusCode: 404, title: "Error");
            }
            ViewBag.idForm = IdForm;
            Response.AddHxHeader(IdForm, HxModeEnum.initValid);
            return PartialView(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName")] Category category)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (id != category.CategoryId)
            {
                return Problem("Category not found", statusCode: 404, title: "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return Problem("Category not found", statusCode: 404, title: "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.idForm = IdForm;
            Response.AddHxHeader(IdForm, HxModeEnum.initValid);
            return PartialView(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (id == null || _context.Categories == null)
            {
                return Problem("Category not found", statusCode: 404, title: "Error");
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return Problem("Category not found", statusCode: 404, title: "Error");
            }

            return PartialView(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Category'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();

            // use the Button 'Back to List' to load the list
            Response.Headers.Add("HX-Trigger-After-Settle", "evtHxBackToList");

            // Send confirmation and replace submit button with it
            string selector = "button[hx-post^=\"/Category/Delete/\"]";
            HttpContext.Response.Headers.Add("HX-Retarget", selector);
            HttpContext.Response.Headers.Add("HX-Reswap", "outerHTML");

            return PartialView("_successFormPost", "Deleted");
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
