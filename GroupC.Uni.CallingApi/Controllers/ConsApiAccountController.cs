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
    public class ConsApiAccountController : Controller
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
        //    if (res.IsSuccessStatusCode)
        //    {
        //        var result = res.Content.ReadAsStringAsync().Result;
        //        exams = JsonConvert.DeserializeObject<List<Exam>>(result);
        //    }
        //    return View(exams);
        //}
        //public async Task<IActionResult> Details(Guid id)
        //{
        //    Exam exam = new Exam();
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = await client.GetAsync("api/Exams/"+id);
        //    if (res.IsSuccessStatusCode)
        //    {
        //        var result = res.Content.ReadAsStringAsync().Result;
        //        exam = JsonConvert.DeserializeObject<Exam>(result);
        //    }
        //    return View(exam);

        //}
    }
}