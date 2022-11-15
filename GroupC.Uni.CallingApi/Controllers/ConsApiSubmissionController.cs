using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupC.Uni.Core.Entities;
using GroupC.Uni.Infrastructure;
using GroupC.Uni.ConsumingApi.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using GroupC.Uni.Core.Interfaces;
using System.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.AspNetCore.Http.Extensions;

namespace GroupC.Uni.ConsumingApi.Controllers
{
    public class ConsApiSubmissionsController : Controller
    {
        //Hosted web API REST Service base url  
        /*string Baseurl = Request.GetDisplayUrl().Split('/').Take(3).Aggregate((s1, s2) => s1 + "," + s2).Substring(1);*/
        private readonly AppDbContext _context;

        public ConsApiSubmissionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Submissions
        public async Task<IActionResult> Index()


        {
            List<SubmissionViewModel> Subs = new List<SubmissionViewModel>();
            using (var client = new HttpClient())
            {
                string Baseurl = Request.GetDisplayUrl().Split('/').Take(3).Aggregate((s1, s2) => s1 + "/" + s2) + "/";
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Submissions/");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var SubResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list  
                    Subs = JsonConvert.DeserializeObject<List<SubmissionViewModel>>(SubResponse);

                }
                //returning the employee list to view  
                return View(Subs);
                //return View(await appDbContext.ToListAsync());
            }
        }
        // GET: Submissions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            SubmissionViewModel Sub = new SubmissionViewModel();
            using (var client = new HttpClient())
            {
                string Baseurl = Request.GetDisplayUrl().Split('/').Take(3).Aggregate((s1, s2) => s1 + "/" + s2) + "/";
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Submissions/" + id);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var SubResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list  
                    Sub = JsonConvert.DeserializeObject<SubmissionViewModel>(SubResponse);

                }
                return View(Sub);
            }

        }

        // GET: Submissions/Create
        public IActionResult Create()
        {
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Id");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id");
            return View();
        }

        // POST: Submissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("date,StudentId,ExamId,Id,LastUpdateDate,CreationDate,Status")] Submission submission)
        {
            if (ModelState.IsValid)
            {
                submission.Id = Guid.NewGuid();
                _context.Add(submission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Id", submission.ExamId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", submission.StudentId);
            return View(submission);
        }

        // GET: Submissions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submission = await _context.Submissions.FindAsync(id);
            if (submission == null)
            {
                return NotFound();
            }
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Id", submission.ExamId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", submission.StudentId);
            return View(submission);
        }

        // POST: Submissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("date,StudentId,ExamId,Id,LastUpdateDate,CreationDate,Status")] Submission submission)
        {
            if (id != submission.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(submission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubmissionExists(submission.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExamId"] = new SelectList(_context.Exams, "Id", "Id", submission.ExamId);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", submission.StudentId);
            return View(submission);
        }

        // GET: Submissions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var submission = await _context.Submissions
                .Include(s => s.Exam)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (submission == null)
            {
                return NotFound();
            }

            return View(submission);
        }

        // POST: Submissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var submission = await _context.Submissions.FindAsync(id);
            _context.Submissions.Remove(submission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubmissionExists(Guid id)
        {
            return _context.Submissions.Any(e => e.Id == id);
        }
    }
}
