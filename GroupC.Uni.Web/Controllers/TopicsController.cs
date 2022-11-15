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
using Microsoft.AspNetCore.Authorization;

namespace GroupC.Uni.Web.Controllers
{
    [Authorize(Roles = "TopicsManagement")]
    public class TopicsController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ITopicService _topicService;

        public TopicsController(ICourseService courseService, ITopicService topicService)
        {
            _courseService = courseService;
            _topicService = topicService;
        }

        // GET: Topics
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var TopicList = await _topicService.ListActiveSyncWithCourse();
            var QueistionViewModelList = new List<TopicViewModels>();
            foreach (var Topic in TopicList)
            {
                TopicViewModels currQVM = new TopicViewModels()
                {
                    Id = Topic.Id,
                    Name = Topic.Name,
                    Course = Topic.Course,
                    CourseId = Topic.CourseId
                };
                QueistionViewModelList.Add(currQVM);
            }
            return View(QueistionViewModelList);

        }

        // GET: Topics/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var _Topic = await _topicService.GetByIdWithCourse(id);
            if (_Topic == null)
            {
                return NotFound();
            }
            TopicViewModels _TopicViewModel = new TopicViewModels()
            {
                Id = _Topic.Id,
                Name = _Topic.Name,
                Status = _Topic.Status,
                Course = _Topic.Course,
                CourseId = _Topic.CourseId
            };

            return View(_TopicViewModel);
        }

        // GET: Topics/Create
        public IActionResult Create()
        {
            List<Course> l = _courseService.GetAllAsList();
            ViewData["CourseID"] = new SelectList(l, "Id", "Name");
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,CourseId,Id,LastUpdateDate,CreationDate,Status")] TopicViewModels topicViewModel)
        {
            if (ModelState.IsValid)
            {
                topicViewModel.Id = Guid.NewGuid();
                Course _Course = await _courseService.GetByIdAsync(topicViewModel.CourseId);
                Topic _Topic = new Topic()
                {
                    Id = topicViewModel.Id,
                    Name = topicViewModel.Name,
                    Status = topicViewModel.Status,
                    Course = _Course,
                    CourseId = topicViewModel.CourseId,

                };
                await _topicService.CreateTopicAsync(_Topic);
                return RedirectToAction(nameof(Index));
            }

            ViewData["CourseId"] = new SelectList(_courseService.GetAllAsList(), "Id", "Name", topicViewModel.CourseId);
            return View(topicViewModel);
        }

        // GET: Topics/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Topic = await _topicService.GetByIdWithCourse(id);
            if (Topic == null)
            {
                return NotFound();
            }
            List<Topic> l = _topicService.GetAllAsList();
            ViewData["CourseId"] = new SelectList(l, "Id", "Name", Topic.CourseId);
            TopicViewModels _TopicViewModel = new TopicViewModels()
            {
                Id = Topic.Id,
                Name = Topic.Name,
                Status = Topic.Status,
                Course = Topic.Course,
                CourseId = Topic.CourseId
            };
            return View(_TopicViewModel);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,CourseId,Id,LastUpdateDate,CreationDate,Status")] TopicViewModels topicViewModel)
        {
            if (id != topicViewModel.Id)
            {
                return NotFound();
            }
            Topic _Topic = new Topic()
            {
                Id = topicViewModel.Id,
                Name = topicViewModel.Name,
                
            };
            if (ModelState.IsValid)
            {
                try
                {
                    await _topicService.SpecialUpdateAsync(_Topic);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(_Topic.Id))
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
            ViewData["CourseId"] = new SelectList(_topicService.GetAllAsList(), "Id", "Name", topicViewModel.CourseId);
            return View(topicViewModel);
        }

        // GET: Topics/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Topic = await _topicService.GetByIdWithCourse(id);

            if (Topic == null)
            {
                return NotFound();
            }
            TopicViewModels _TopicViewModel = new TopicViewModels()
            {
                Id = Topic.Id,
                Name = Topic.Name,
                Status = Topic.Status,
                Course = Topic.Course,
                CourseId = Topic.CourseId
            };
            return View(_TopicViewModel);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var topic = await _topicService.GetByIdWithCourse(id);
            await _topicService.Deactivate(topic);
            return RedirectToAction(nameof(Index));
        }

        private bool TopicExists(Guid id)
        {
            var topic = _topicService.GetByIdAsync(id);
            if (topic != null)
                return true;
            return false;
        }
    }
}
