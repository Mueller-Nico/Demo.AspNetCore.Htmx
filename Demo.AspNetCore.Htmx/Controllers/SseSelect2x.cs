
using System.Text.Encodings.Web;
using Demo.AspNetCore.Htmx.Data;
using Demo.AspNetCore.Htmx.Extensions;
using Demo.AspNetCore.Htmx.Models;
using Demo.AspNetCore.Htmx.Services.SseNotifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Demo.AspNetCore.Htmx
{
    public class SseSelect2x : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationsService _notificationsService;
        private readonly HtmlEncoder _htmlEncoder;
        public SseSelect2x(ApplicationDbContext context, INotificationsService notificationsService, HtmlEncoder htmlEncoder)
        {
            _notificationsService = notificationsService;
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
                    ViewData["TitlePartial"] = "SSE Insert Row";
                    return PartialView(model);
                }
                ViewData["Title"] = "SSE Insert Row";
                return View(model);

            }
            return Problem("Entity set 'ApplicationDbContext.Manufacturers'  is null.");
        }

       
        [HttpGet]
        public async Task<IActionResult> Models(int? manufacturerId)
        {
            if (_context.Models != null)
            {
                List<Model> models = await _context.Models.Where(x => x.ManufacturerId == manufacturerId).ToListAsync();
                return PartialView(models);
            }
            return Problem("Entity set 'ApplicationDbContext.Models'  is null.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectedBike(int? ModelId)
        {

            Model model = await _context.Models.Include(m => m.Manufacturer).FirstOrDefaultAsync(m => m.ModelId == ModelId);
            if(model == null)
            {
                return Problem("Resouce not found"); ;
            }

            string type = "new_bike";
            string msgId = Guid.NewGuid().ToString();
            
            //* send json 
            string message = System.Text.Json.JsonSerializer.Serialize(new {
                modelId= model.ModelId,
                manufacturer = model.Manufacturer.ManufacturerName, 
                model = model.ModelName, 
                power = model.Power_KW,
                capacity = model.Capacity
            });

            await _notificationsService.SendNotificationAsync(message, type, false, msgId);

            return NoContent();
        }
    }
}
