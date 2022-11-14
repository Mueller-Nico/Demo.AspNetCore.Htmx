using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Demo.AspNetCore.Htmx.Data;
using Demo.AspNetCore.Htmx.Extensions;
using Demo.AspNetCore.Htmx.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Demo.AspNetCore.Htmx
{
    public class Select2xController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HtmlEncoder _htmlEncoder;
        public Select2xController(ApplicationDbContext context, HtmlEncoder htmlEncoder)
        {
            _htmlEncoder = htmlEncoder;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            Model model = new();
            if (_context.Manufacturers != null)
            {
                List<Manufacturer> manufacturers = await _context.Manufacturers.ToListAsync();
                ViewBag.Manufacturers = manufacturers;

                if (Request.IsHtmxRequest())
                {
                    ViewData["TitlePartial"] = "Select2x";
                    return PartialView(model);
                }
                ViewData["Title"] = "Select2x";
                return View(model);

            }
            return Problem("Entity set 'ApplicationDbContext.Manufacturers'  is null.");
        }

        [HttpGet]
        public async Task<IActionResult> Models(int? manufacturerId)
        {
            // see the loading spinner (Remove in entire file for production)
            await Task.Delay(TimeSpan.FromSeconds(1));

            if (_context.Models != null)
            {
                List<Model> models = await _context.Models.Where(x => x.ManufacturerId == manufacturerId).ToListAsync();
                return PartialView(models);
            }
            return Problem("Entity set 'ApplicationDbContext.Models'  is null.");
        }

        public async Task<IActionResult> SelectedBike(int? ModelId)
        {
            Model model = await _context.Models.Include(m => m.Manufacturer).FirstOrDefaultAsync(m => m.ModelId == ModelId);
            if (model == null)
            {
                return Problem("Resouce not found"); ;
            }
            return Content(
                _htmlEncoder.Encode(model.Manufacturer.ManufacturerName) + " " +
                _htmlEncoder.Encode(model.ModelName) + "&nbsp;&nbsp; Power: " +
                _htmlEncoder.Encode(model.Power_KW) + " KW&nbsp;&nbsp; Capacity: " + model.Capacity + " CC");
        }
    }
}
