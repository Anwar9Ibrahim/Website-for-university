using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GroupC.Uni.ConsumingApi.Helper;
using GroupC.Uni.CallingApi.Models;
using GroupC.Uni.Core.Entities;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace GroupC.Uni.CallingApi.Controllers
{
    public class ConsApiHomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        //StudentAPI _api = new StudentAPI();
        //public async Task<IActionResult> Index()
        //{
        //    List<Exam> exams = new List<Exam>();
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = await client.GetAsync("api/Exams");
        //    if(res.IsSuccessStatusCode)
        //    {
        //        var result = res.Content.ReadAsStringAsync().Result;
        //        exams = JsonConvert.DeserializeObject<List<Exam>>(result);
        //    }
        //    return View(exams);
        //}

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
