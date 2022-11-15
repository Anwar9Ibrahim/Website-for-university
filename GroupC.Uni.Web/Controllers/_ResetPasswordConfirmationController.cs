using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GroupC.Uni.Web.Controllers
{
    public class _ResetPasswordConfirmationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}