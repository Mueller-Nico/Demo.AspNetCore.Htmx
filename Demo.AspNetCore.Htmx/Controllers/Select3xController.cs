using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Demo.AspNetCore.Htmx.Data;
using Demo.AspNetCore.Htmx.Extensions;
using Demo.AspNetCore.Htmx.Models;

namespace Demo.AspNetCore.Htmx
{
    public class Select3xController : Controller
    {
        private readonly ApplicationDbContext _context;
        public Select3xController(ApplicationDbContext context, HtmlEncoder htmlEncoder)
        {
            _context = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            Model model = new Model();
            if (_context.Categories != null)
            {
                List<Category> categories = await _context.Categories.ToListAsync();
                ViewBag.Categories = categories;

                if (Request.IsHtmxRequest())
                {
                    return PartialView(model);
                }
                return View(model);
            }
            return Problem("Entity set 'ApplicationDbContext.Categories'  is null.");
        }

        [HttpGet]
        public async Task<IActionResult> Manufacturers(int categoryId)
        {
            // see the loading spinner (Remove in entire file for production)
            await Task.Delay(TimeSpan.FromSeconds(1));

            List<Manufacturer> manufacturers =
                    await _context.Manufacturers.FromSqlRaw("SELECT DISTINCT Manufacturers.ManufacturerId, " +
                    "Manufacturers.ManufacturerName FROM Models INNER JOIN " +
                    "Manufacturers ON Models.ManufacturerId = Manufacturers.ManufacturerId " +
                    "WHERE CategoryId = {0} ORDER BY ManufacturerName",
                    categoryId).ToListAsync();
            return PartialView(manufacturers);
        }

        [HttpGet]
        public async Task<IActionResult> Models(int manufacturerId, int categoryId)
        {
           
            await Task.Delay(TimeSpan.FromSeconds(1));

            if (_context.Models != null)
            {
                List<Model> models = await _context.Models.Where(x => x.ManufacturerId == manufacturerId & x.CategoryId == categoryId).ToListAsync();
                return PartialView(models);
            }
            return Problem("Entity set 'ApplicationDbContext.Models'  is null.");
        }


        public async Task<IActionResult> SelectedBike(int? ModelId)
        {
            Model model = await _context.Models.Include(m => m.Category).Include(m => m.Manufacturer).FirstOrDefaultAsync(m => m.ModelId == ModelId);
            if (model == null)
            {
                return Problem("Resouce not found");
            }
            return PartialView(model);
        }
    }
}
