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
    public class ManufacturerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string IdForm = "idManufacturerForm";

        public ManufacturerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Manufacturer
        public async Task<IActionResult> Index()
        {
            // see the loading spinner (Remove in entire file for production)
            Thread.Sleep(TimeSpan.FromSeconds(1));

            var items = await _context.Manufacturers.OrderBy(x => x.ManufacturerName).ToListAsync();
            if (Request.IsHtmxRequest())
            {
                ViewData["TitlePartial"] = "Manufacturers";
                return PartialView(items);
            }
            else
            {
                ViewData["Title"] = "Manufacturers";
                return View(items);
            }
        }

        // GET: Manufacturer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (id == null || _context.Manufacturers == null)
            {
                return Problem("Manufacturer not found", statusCode: 404, title: "Error");
            }

            var manufacturer = await _context.Manufacturers
                .FirstOrDefaultAsync(m => m.ManufacturerId == id);
            if (manufacturer == null)
            {
                return Problem("Manufacturer not found", statusCode: 404, title: "Error");
            }

            return PartialView(manufacturer);
        }

        // GET: Manufacturer/Create
        public IActionResult Create()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            ViewBag.idForm = IdForm;
            Response.AddHxHeader(IdForm, HxModeEnum.initValid);
            return PartialView();
        }

        // POST: Manufacturer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ManufacturerId,ManufacturerName")] Manufacturer manufacturer)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (ModelState.IsValid)
            {
                _context.Add(manufacturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.idForm = IdForm;
            Response.AddHxHeader(IdForm, HxModeEnum.initValid);
            return PartialView(manufacturer);
        }

        // GET: Manufacturer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (id == null || _context.Manufacturers == null)
            {
                return Problem("Manufacturer not found", statusCode: 404, title: "Error");
            }

            var manufacturer = await _context.Manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return Problem("Manufacturer not found", statusCode: 404, title: "Error");
            }
            ViewBag.idForm = IdForm;
            Response.AddHxHeader(IdForm, HxModeEnum.initValid);
            return PartialView(manufacturer);
        }

        // POST: Manufacturer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ManufacturerId,ManufacturerName")] Manufacturer manufacturer)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (id != manufacturer.ManufacturerId)
            {
                return Problem("Manufacturer not found", statusCode: 404, title: "Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manufacturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManufacturerExists(manufacturer.ManufacturerId))
                    {
                        return Problem("Manufacturer not found", statusCode: 404, title: "Error");
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
            return PartialView(manufacturer);
        }

        // GET: Manufacturer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (id == null || _context.Manufacturers == null)
            {
                return Problem("Manufacturer not found", statusCode: 404, title: "Error");
            }

            var manufacturer = await _context.Manufacturers
                .FirstOrDefaultAsync(m => m.ManufacturerId == id);
            if (manufacturer == null)
            {
                return Problem("Manufacturer not found", statusCode: 404, title: "Error");
            }

            return PartialView(manufacturer);
        }

        // POST: Manufacturer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (_context.Manufacturers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Manufacturer'  is null.");
            }
            var manufacturer = await _context.Manufacturers.FindAsync(id);
            if (manufacturer != null)
            {
                _context.Manufacturers.Remove(manufacturer);
            }

            await _context.SaveChangesAsync();

            // use the Button 'Back to List' to load the list
            Response.Headers.Add("HX-Trigger-After-Settle", "evtHxBackToList");

            // Send confirmation and replace submit button with it
            string selector = "button[hx-post^=\"/Manufacturer/Delete/\"]";
            HttpContext.Response.Headers.Add("HX-Retarget", selector);
            HttpContext.Response.Headers.Add("HX-Reswap", "outerHTML");

            return PartialView("_successFormPost", "Deleted");
        }

        private bool ManufacturerExists(int id)
        {
            return _context.Manufacturers.Any(e => e.ManufacturerId == id);
        }
    }
}
