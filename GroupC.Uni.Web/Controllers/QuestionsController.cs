using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using GroupC.Uni.Infrastructure;
using GroupC.Uni.Web.ViewModels;
using Microsoft.AspNetCore.Mvc.Filters;
using GroupC.Uni.Web.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace GroupC.Uni.Web.Controllers
{
    [Authorize(Roles = "QuestionsManagement")]
    public class QuestionsController : Controller
    {

        private readonly IQuestionService _questionService;
        private readonly ICourseService _courseService;
        private readonly ITopicService _topicService;
        private readonly IChoiceService _choiceService;

        public QuestionsController(ICourseService courseService,IQuestionService questionService, ITopicService topicService, IChoiceService choiceService)
        {

            _questionService = questionService;
            _topicService = topicService;
            _courseService = courseService;
            _choiceService = choiceService;
        }
     
        // GET: Questions
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }

            ViewBag.CurrentPage = "ViewQuestion";
            var QuestionList = await _questionService.ListActiveSyncWithTopic();
            var QueistionViewModelList = new List<QuestionIndexViewModel>();
            foreach (var question in QuestionList)
            {
                QuestionIndexViewModel currQVM = new QuestionIndexViewModel()
                {
                    Id = question.Id,
                    Text = question.Text,
                    Mark = question.Mark,
                    IsHtml = question.IsHtml,
                    TopicId = question.TopicId,
                    TopicName = question.Topic.Name,
                    Status = question.Status
                };
                QueistionViewModelList.Add(currQVM);
            }
            return View(QueistionViewModelList);
        }
        //server side datatable
        
        public JsonResult GetFilteredItems()
        {
            int draw = Convert.ToInt32(Request.Query["draw"]);

            // Data to be skipped , 
            // if 0 first "length" records will be fetched
            // if 1 second "length" of records will be fethced ...
            int start = Convert.ToInt32(Request.Query["start"]);

            // Records count to be fetched after skip
            int length = Convert.ToInt32(Request.Query["length"]);
            
            // Getting Sort Column Name
            int sortColumnIdx = Convert.ToInt32(Request.Query["order[0][column]"]);
            string sortColumnName = Request.Query["columns[" + sortColumnIdx + "][name]"];
            
            // Sort Column Direction  
            string sortColumnDirection = Request.Query["order[0][dir]"];
            
            // Search Value
            string searchValue = Request.Query["search[value]"].FirstOrDefault()?.Trim();

            // Total count matching search criteria 
            int recordsFilteredCount = _questionService.recordsFilteredCount(searchValue);

            // Total Records Count
            int recordsTotalCount = _questionService.AllRecordsCount();

            // Filtered & Sorted & Paged data to be sent from server to view
            List<Question> filteredData = null;
            if (sortColumnDirection == "asc")
            {
                filteredData = _questionService.FilteredDataAsc(searchValue,sortColumnName,start,length);
            }
            else
            {
                filteredData = _questionService.FilteredDataDesc(searchValue, sortColumnName, start, length);

            }
            List<String> stringChoices =new List<string>();
           
            List<QuestionIndexViewModel> dataToShow = new List<QuestionIndexViewModel>();
            foreach (Question record in filteredData)
            {

                foreach (Choice c in record.Choices)
                {
                    stringChoices.Add(c.Text);

                }
                string stringChoice = JsonConvert.SerializeObject(stringChoices); //  [1,2,6,7,8,18,25,61,129]

                dataToShow.Add(new QuestionIndexViewModel() {
                    Id = record.Id,
                    Text = record.Text,
                    Mark = record.Mark,
                    IsHtml = record.IsHtml,
                    TopicId = record.TopicId,
                    TopicName = record.Topic.Name,
                    choices_string= stringChoice
                    }
                    );
            }
            return Json(
                        new {
                              data = dataToShow,
                              draw = Request.Query["draw"],
                              recordsFiltered = recordsFilteredCount,
                              recordsTotal = recordsTotalCount
                            }
                    );
        }

        // GET: Questions/Details/5
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
            var question = await _questionService.GetByIdWithTopic(id);
            List<Choice> listChoices = await _choiceService.listChoicesByQuestionId(question.Id);
            QuestionViewModel _questionViewModel = new QuestionViewModel()
            {
                Id = question.Id,
                Text = question.Text,
                Mark = question.Mark,
                IsHtml = question.IsHtml,
                TopicId = question.TopicId,
                Topic = question.Topic,
                Status = question.Status,
                Choices = new List<CreateChoiceViewModel>()
            };

            if (question.Choices.Count() != 0)
                foreach (var choice in listChoices)
                {
                    _questionViewModel.Choices.Add(new CreateChoiceViewModel()
                    {

                        Text = choice.Text,
                        Type = choice.Type
                    });
                }


            return View(_questionViewModel);
        }

        // GET: Questions/Create
        public async Task<IActionResult> Create()

        {
            ViewBag.CurrentPage = "CreateQuestion";
            //List<Topic> l = _topicService.getAllAsList();
            //ViewData["TopicId"] = new SelectList(l, "Id", "Name");
            //return View();
            List<SelectListItem> topicItems = new List<SelectListItem>();
            IReadOnlyList <Course> Courses = await _courseService.ListActiveSync();
            Courses = Courses.OrderBy(i => i.Name).ToList();
            //Loop and add the Parent Nodes.
            foreach (Course course in Courses)
            {
                //Create the Group.
                SelectListGroup group = new SelectListGroup() { Name = course.Name };
                //list of topics in current course
                List<Topic> listTopics = _topicService.ListActiveTopicByCourseId(course.Id);
                listTopics = listTopics.OrderBy(i => i.Name).ToList();
                //Loop and add the Items.
                foreach (Topic topic in listTopics)
                {
                    topicItems.Add(new SelectListItem
                    {
                        Value = topic.Id.ToString(),
                        Text = topic.Name,
                        Group = group
                    });
                }
            }
            ViewData["TopicId"] = topicItems;
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionViewModel questionViewModel)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }

            if (ModelState.IsValid)
            {
                questionViewModel.Id = Guid.NewGuid();
                Topic _Topic = await _topicService.GetByIdAsync(questionViewModel.TopicId);
                Question _question = new Question()
                {
                    Id = questionViewModel.Id,
                    Text = questionViewModel.Text,
                    Mark = questionViewModel.Mark,
                    IsHtml = questionViewModel.IsHtml,
                    TopicId = questionViewModel.TopicId,
                    Topic = _Topic
                };
                if (questionViewModel.Choices != null)
                    foreach (var choice in questionViewModel.Choices)
                    {
                        _question.Choices.Add(new Choice()
                        {
                            Id = Guid.NewGuid(),
                            Text = choice.Text,
                            Type = choice.Type
                        });
                    }


                await _questionService.CreateQuestionAsync(_question);
                String message = "";
                if (questionViewModel.Choices != null)
                {
                    message = "Added Question with Choices Successfully !";
                }
                else message = "Added Question Successfully !";

                TempData["Message"] = JsonConvert.SerializeObject(Message.AddSuccessMessage(message));
                return RedirectToAction("Details", "Questions", new { id = questionViewModel.Id });

            }

            ViewBag.CurrentPage = "CreateQuestion";
            List<SelectListItem> topicItems = new List<SelectListItem>();
            IReadOnlyList<Course> Courses = await _courseService.ListActiveSync();
            Courses = Courses.OrderBy(i => i.Name).ToList();
            //Loop and add the Parent Nodes.
            foreach (Course course in Courses)
            {
                //Create the Group.
                SelectListGroup group = new SelectListGroup() { Name = course.Name };
                //list of topics in current course
                List<Topic> listTopics = _topicService.ListActiveTopicByCourseId(course.Id);
                listTopics = listTopics.OrderBy(i => i.Name).ToList();
                //Loop and add the Items.
                foreach (Topic topic in listTopics)
                {
                    topicItems.Add(new SelectListItem
                    {
                        Value = topic.Id.ToString(),
                        Text = topic.Name,
                        Group = group
                    });
                }
            }
            ViewData["TopicId"] = topicItems;
            TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed!"));
            return View(questionViewModel);
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }

            var question = await _questionService.GetByIdWithTopic(id);
            List<Choice> listChoices = await _choiceService.listChoicesByQuestionId(question.Id);
            List<SelectListItem> topicItems = new List<SelectListItem>();
            IReadOnlyList<Course> Courses = await _courseService.ListActiveSync();
            Courses = Courses.OrderBy(i => i.Name).ToList();
            //Loop and add the Parent Nodes.
            foreach (Course course in Courses)
            {
                //Create the Group.
                SelectListGroup group = new SelectListGroup() { Name = course.Name };
                //list of topics in current course
                List<Topic> listTopics = _topicService.ListActiveTopicByCourseId(course.Id);
                listTopics = listTopics.OrderBy(i => i.Name).ToList();
                //Loop and add the Items.
                foreach (Topic topic in listTopics)
                {
                    topicItems.Add(new SelectListItem
                    {
                        Value = topic.Id.ToString(),
                        Text = topic.Name,
                        Group = group
                    });
                }
            }

            ViewData["TopicId"] = topicItems;
            QuestionViewModel _questionViewModel = new QuestionViewModel()
            {
                Id = question.Id,
                Text = question.Text,
                Mark = question.Mark,
                IsHtml = question.IsHtml,
                TopicId = question.TopicId,
                Topic = question.Topic,
                Status = question.Status,
                Choices = new List<CreateChoiceViewModel>()
            };

            if (question.Choices.Count() != 0)
                foreach (var choice in listChoices)
                {
                    _questionViewModel.Choices.Add(new CreateChoiceViewModel()
                    {

                        Text = choice.Text,
                        Type = choice.Type
                    });
                }
            return View(_questionViewModel);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(QuestionViewModel questionViewModel)
        {
             Topic _Topic = await _topicService.GetByIdAsync(questionViewModel.TopicId);
            Question _question = new Question()
            {
                Id = questionViewModel.Id,
                Text = questionViewModel.Text,
                Mark = questionViewModel.Mark,
                IsHtml = questionViewModel.IsHtml,
                Topic = _Topic,
                TopicId = questionViewModel.TopicId
            };
            List<Choice> listChoices = await _choiceService.listChoicesByQuestionId(questionViewModel.Id);
            if (listChoices.Count() != 0)
            {
                foreach (var choice in listChoices)
                {
                    await _choiceService.DeleteAsync(choice);
                }
                foreach (var choice in questionViewModel.Choices)
                {
                    _question.Choices.Add(new Choice()
                    {
                        Id = new Guid(),
                        QuestionId = questionViewModel.Id,
                        Text = choice.Text,
                        Type = choice.Type
                    });
                }
            }
            else if (questionViewModel.Choices.Count() != 0)
            {
                foreach (var choice in questionViewModel.Choices)
                {
                    _question.Choices.Add(new Choice()
                    {
                        Id = new Guid(),
                        QuestionId = questionViewModel.Id,
                        Text = choice.Text,
                        Type = choice.Type
                    });
                }
            }
            else
            {

            }
        
            string message = "";
            if (ModelState.IsValid)
            {
                try
                {
                    await _questionService.specialUpdateAsync(_question);
                    message = "Edited Question Successfully !";
                    TempData["Message"] = JsonConvert.SerializeObject(Message.AddSuccessMessage(message));
                    return RedirectToAction("Details", "Questions", new { id = questionViewModel.Id });

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(_question.Id))
                    {
                        List<Topic> ll = _topicService.GetAllAsList();
                        ViewData["TopicId"] = new SelectList(ll, "Id", "Name", questionViewModel.TopicId);
                        TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed"));
                        return View(questionViewModel.Id);
                    }
                }
                RedirectToAction("Details", "Questions", new { id = questionViewModel.Id });
            }
            List<Topic> l = _topicService.GetAllAsList();
            ViewData["TopicId"] = new SelectList(l, "Id", "Name", questionViewModel.TopicId);
            TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed"));
            return View(questionViewModel.Id);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var question = await _questionService.GetByIdAsync(id);

            if (question == null)
            {
                return NotFound();
            }
            await _questionService.Deactivate(question);
            return RedirectToAction(nameof(Index));
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var question = await _questionService.GetByIdAsync(id);
            await _questionService.Deactivate(question);
            return RedirectToAction(nameof(Index));
        }


        private bool QuestionExists(Guid id)
        {
            var Q = _questionService.GetByIdAsync(id);
            if (Q != null)
                return true;
            return false;
        }
    }
}
