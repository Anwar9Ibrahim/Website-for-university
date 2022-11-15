using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupC.Uni.Core.Entities;
using GroupC.Uni.Infrastructure;
using GroupC.Uni.Core.Interfaces;
using GroupC.Uni.Web.ViewModels;
using GroupC.Uni.Core.Services;
using GroupC.Uni.Web.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;

namespace GroupC.Uni.Web.Controllers
{
    [Authorize(Roles = "CourseManagement")]
    public class CoursesController : Controller
    {
        //private readonly AppDbContext _context;
        private readonly ICourseService _courseService;
        private readonly ITopicService _topicService;
        private readonly IHostingEnvironment _hostingEnv;
        private readonly IStringLocalizer<HomeController> _localizer;
        public CoursesController(ICourseService courseService, ITopicService topicService, IHostingEnvironment hostingEnv, IStringLocalizer<HomeController> localizer)
        {

            _courseService = courseService;
            _hostingEnv = hostingEnv;
            _topicService = topicService;
            _localizer = localizer;
        }

        [AllowAnonymous]
        // GET: Courses
        public async Task<IActionResult> Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }

            ViewBag.CurrentPage = "ViewCourse";
            var CoursesList = await _courseService.ListActiveSync();
            var CoursesViewModelList = new List<CourseModelView>();
            foreach (var course in CoursesList)
            {
                CourseModelView courseView = new CourseModelView
                {
                    Id = course.Id,
                    Name = course.Name,
                    Code = course.Code,
                    ImageURL = course.ImageURL,
                    Status = course.Status
                };
                CoursesViewModelList.Add(courseView);
            }
            return View(CoursesViewModelList);
        }

        // GET: Courses/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid id)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }



            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
            {
                // Response.StatusCode = 404;
                //Response.StatusCode = 404;
                // return View("CourseNotFound", id);
                return NotFound();
            }

            List<Topic> listTopics = _topicService.ListActiveTopicByCourseId(course.Id);
            var topics = _topicService.ListActiveTopicByCourseId(id);

            CourseModelView coursemodelview = new CourseModelView
            {
                Id = course.Id,
                Name = course.Name,
                Code = course.Code,
                ImageURL = course.ImageURL,
                Status = course.Status,
                Topics = new List<CreateTopicViewModel>()

            };
            if (course.Topics.Count() != 0)
                foreach (var topic in listTopics)
                {
                    coursemodelview.Topics.Add(new CreateTopicViewModel()
                    {
                        TopicName = topic.Name
                    });
                }

            return View(coursemodelview);
        }

        //public IActionResult CourseNotFound(int id)
        //{
        //    return View(id);
        //}

        public IActionResult Create()
        {

            ViewBag.CurrentPage = "CreateCourse";
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseModelView CourseModelView)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }


            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (CourseModelView.Image != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnv.WebRootPath, "images");
                    string imageId = Guid.NewGuid().ToString();
                    uniqueFileName = imageId + "_" + CourseModelView.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    CourseModelView.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                    uniqueFileName = "/images/" + imageId + "_" + CourseModelView.Image.FileName;
                }
                else
                {
                    uniqueFileName = "/images/courseDefaultImage.jpg";
                }
                CourseModelView.Id = Guid.NewGuid();
                ViewBag.CourseID = CourseModelView.Id;
                Course course = new Course
                {
                    Id = CourseModelView.Id,
                    Name = CourseModelView.Name,
                    Code = CourseModelView.Code,
                    ImageURL = uniqueFileName,
                    Status = CourseModelView.Status
                };

                if (CourseModelView.Topics != null)
                    foreach (var topic in CourseModelView.Topics)
                    {
                        course.Topics.Add(new Topic()
                        {
                            Id = Guid.NewGuid(),
                            Name = topic.TopicName
                        });
                    }
                try
                {
                    await _courseService.AddAsync(course);
                    String message = "";
                    if (CourseModelView.Topics != null)
                    {
                        message = "Added " + CourseModelView.Name + " Course with " + CourseModelView.Topics.Count().ToString() + " Topics Successfully !";
                    }
                    else message = "Added " + CourseModelView.Name + " Course Successfully !";
                    TempData["Message"] = JsonConvert.SerializeObject(Message.AddSuccessMessage(message));
                    return RedirectToAction("Details", "Courses", new { id = CourseModelView.Id });
                }
                catch (Exception)
                {
                    TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed!"));
                    return View(CourseModelView);
                }

            }

            TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed!"));
            return View(CourseModelView);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }

            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseService.GetByIdAsync(id);
            if (course == null)
            {
                return NotFound();
                //return StatusCode(404);
            }
            List<Topic> listTopics = _topicService.ListActiveTopicByCourseId(course.Id);
            var topics = _topicService.ListActiveTopicByCourseId(id);
            CourseModelView coursemodelview = new CourseModelView
            {
                Id = course.Id,
                Name = course.Name,
                Code = course.Code,
                ImageURL = course.ImageURL,
                Status = course.Status,
                Topics = new List<CreateTopicViewModel>()

            };
            if (course.Topics.Count() != 0)
                foreach (var topic in listTopics)
                {
                    coursemodelview.Topics.Add(new CreateTopicViewModel()
                    {
                        TopicName = topic.Name
                    });
                }

            return View(coursemodelview);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseModelView courseModelView)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (!courseModelView.ImageURL.StartsWith("/images/"))
                {
                    string uploadsFolder = Path.Combine(_hostingEnv.WebRootPath, "images");
                    string imageId = Guid.NewGuid().ToString();
                    uniqueFileName = imageId + "_" + courseModelView.ImageURL.ToString();
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    courseModelView.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                    uniqueFileName = "/images/" + imageId + "_" + courseModelView.ImageURL.ToString();
                }
                else
                {
                    uniqueFileName = courseModelView.ImageURL;
                }
                Course course = new Course
                {
                    Id = courseModelView.Id,
                    Name = courseModelView.Name,
                    Code = courseModelView.Code,
                    ImageURL = uniqueFileName,
                    Status = courseModelView.Status,
                    Topics = new List<Topic>()
                };

                if (courseModelView.Topics != null)
                {
                    List<Topic> topics = _topicService.ListActiveTopicByCourseId(courseModelView.Id);
                    foreach (var topic in topics)
                    {

                        await _topicService.DeleteAsync(topic);
                    }

                    foreach (var topic in courseModelView.Topics)
                    {
                        course.Topics.Add(new Topic()
                        {
                            CourseId = courseModelView.Id,
                            Id = Guid.NewGuid(),
                            Name = topic.TopicName
                        });
                    }
                }
                try
                {
                    await _courseService.specialUpdateAsync(course);
                    string message = "Edited " + courseModelView.Name + " Course Successfully !";
                    TempData["Message"] = JsonConvert.SerializeObject(Message.AddSuccessMessage(message));
                    return RedirectToAction("Details", "Courses", new { id = courseModelView.Id });
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed!"));
                        return View(courseModelView.Id);

                    }
                }

            }
            TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed!"));
            return View(courseModelView.Id);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _courseService.GetByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }
            await _courseService.Deactivate(course);
            return RedirectToAction(nameof(Index));
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var course = await _courseService.GetByIdAsync(id);
            await _courseService.Deactivate(course);
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(Guid id)
        {
            var topic = _courseService.GetByIdAsync(id);
            if (topic != null)
                return true;
            return false;
        }



    }
}
