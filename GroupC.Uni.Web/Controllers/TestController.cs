using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GroupC.Uni.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            TempData["TempMsg"] = "Hello from Index";
            return RedirectToAction("Test");
        }

        public IActionResult Test()
        {
            ViewBag.TempMsg = TempData["TempMsg"];
            return Json(new { data = ViewBag.TempMsg } );
        }
    }
}