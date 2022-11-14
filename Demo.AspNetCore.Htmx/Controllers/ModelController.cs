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
using Demo.AspNetCore.Htmx.Helper;

namespace Demo.AspNetCore.Htmx.Controllers
{
    public class ModelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string IdForm = "idModelForm";

        public ModelController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Model
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Models.Include(m => m.Category).Include(m => m.Manufacturer);
            var items = await applicationDbContext.OrderBy(x => x.Manufacturer.ManufacturerName).ThenBy(x => x.ModelName).ToListAsync();
            if (Request.IsHtmxRequest())
            {
                ViewData["TitlePartial"] = "Models";
                return PartialView(items);
            }
            else
            {
                ViewData["Title"] = "Models";
                return View(items);
            }
        }

        // GET: Model/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // see the loading spinner (Remove in entire file for production)
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (id == null || _context.Models == null)
            {
                return Problem("Model not found", statusCode: 404, title: "Error");
            }

            var model = await _context.Models
                .Include(m => m.Category)
                .Include(m => m.Manufacturer)
                .FirstOrDefaultAsync(m => m.ModelId == id);
            if (model == null)
            {
                return Problem("Model not found", statusCode: 404, title: "Error");
            }

            return PartialView(model);
        }

        // GET: Model/Create
        public IActionResult Create()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName");
            ViewBag.idForm = IdForm;
            Response.AddHxHeader(IdForm, HxModeEnum.initValid);
            return PartialView();
        }

        // POST: Model/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ModelId,CategoryId,ManufacturerId,ModelName,Power_KW,Capacity,Price")] Model model)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", model.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName", model.ManufacturerId);
            ViewBag.idForm = IdForm;
            Response.AddHxHeader(IdForm, HxModeEnum.initValid);
            return PartialView(model);
        }

        // GET: Model/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (id == null || _context.Models == null)
            {
                return Problem("Model not found", statusCode: 404, title: "Error");
            }

            var model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return Problem("Model not found", statusCode: 404, title: "Error");
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", model.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName", model.ManufacturerId);
            ViewBag.idForm = IdForm;
            Response.AddHxHeader(IdForm, HxModeEnum.initValid);
            return PartialView(model);
        }

        // POST: Model/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ModelId,CategoryId,ManufacturerId,ModelName,Power_KW,Capacity,Price")] Model model)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (id != model.ModelId)
            {
                return Problem("Model not found", statusCode: 404, title: "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelExists(model.ModelId))
                    {
                        return Problem("Model not found", statusCode: 404, title: "Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", model.CategoryId);
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "ManufacturerId", "ManufacturerName", model.ManufacturerId);
            ViewBag.idForm = IdForm;
            Response.AddHxHeader(IdForm, HxModeEnum.initValid);
            return PartialView(model);
        }

        // GET: Model/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (id == null || _context.Models == null)
            {
                return Problem("Model not found", statusCode: 404, title: "Error");
            }

            var model = await _context.Models
                .Include(m => m.Category)
                .Include(m => m.Manufacturer)
                .FirstOrDefaultAsync(m => m.ModelId == id);
            if (model == null)
            {
                return Problem("Model not found", statusCode: 404, title: "Error");
            }

            return PartialView(model);
        }

        // POST: Model/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (_context.Models == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Model'  is null.");
            }
            var model = await _context.Models.FindAsync(id);
            if (model != null)
            {
                _context.Models.Remove(model);
            }
            
            await _context.SaveChangesAsync();

            // use the Button 'Back to List' to load the list
            Response.Headers.Add("HX-Trigger-After-Settle", "evtHxBackToList");

            // Send confirmation and replace submit button with it
            string selector = "button[hx-post^=\"/Model/Delete/\"]";
            HttpContext.Response.Headers.Add("HX-Retarget", selector);
            HttpContext.Response.Headers.Add("HX-Reswap", "outerHTML");

            return PartialView("_successFormPost", "Deleted");
        }

        private bool ModelExists(int id)
        {
          return _context.Models.Any(e => e.ModelId == id);
        }
    }
}
