using Demo.AspNetCore.Htmx.Extensions;
using Demo.AspNetCore.Htmx.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Demo.AspNetCore.Htmx.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            if (Request.IsHtmxRequest())
            {
                ViewData["TitlePartial"] = "Home";
                return PartialView();
            }
            else
            {
                ViewData["Title"] = "Home";
                return View();
            }
        }

        public IActionResult Privacy()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            if (Request.IsHtmxRequest())
            {
                return PartialView();
            }
            else
            {
                return View();
            }
        }

        /// <summary>
        /// The user enters a URL for a nonexistent page in the address bar
        /// </summary>
        /// <returns></returns>
        [Route("/NotFound")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult PageNotFound()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}