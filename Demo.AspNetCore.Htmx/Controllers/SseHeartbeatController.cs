using Demo.AspNetCore.Htmx.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Demo.AspNetCore.Htmx.Controllers
{
    public class SseHeartbeatController : Controller
    {
        public IActionResult Index()
        {
            if (Request.IsHtmxRequest())
            {
                return PartialView();
            }
            else
            {
                return View();
            }
        }
    }
}