using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GroupC.Uni.ConsumingApi.Helper;
using GroupC.Uni.Core.Entities;
using System.Net.Http;
using Newtonsoft.Json;

namespace GroupC.Uni.ConsumingApi.Controllers
{
    public class ConsApiCoursesController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}