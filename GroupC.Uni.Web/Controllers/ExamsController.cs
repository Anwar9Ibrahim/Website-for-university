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
using GroupC.Uni.Web.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace GroupC.Uni.Web.Views
{
    [Authorize(Roles = "ExamsManagement")]
    public class ExamsController : Controller
    {
        private readonly ITestCenterService _TestCenterService;
        private readonly ICourseService _CourseService;
        private readonly IGenerateExamService _GenerateExamService;
        private readonly IExamService _ExamService;
        private readonly IQuestionService _questionService;
        public ExamsController( ITestCenterService ITestCenterService,
        ICourseService ICourseService, IGenerateExamService IGenerateExamService, IExamService IExamService,
        IQuestionService questionService)
        {
            _TestCenterService = ITestCenterService;
            _CourseService = ICourseService;
            _GenerateExamService = IGenerateExamService;
            _ExamService = IExamService;
            _questionService = questionService;
        }

        [AllowAnonymous]
        // GET: Exams
        public async Task<IActionResult> Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }

            ViewBag.CurrentPage = "ViewExam";
            var ExamList = await _ExamService.ListAllAsyncWithExTT();
            var ExamViewModel = new List<ExamViewModel>();
            foreach (var e in ExamList)
            {
                ICollection<ExamQuestionViewModel> _ExamQuestionsViewModel = new List<ExamQuestionViewModel>();
                foreach (var examQuestion in e.ExamQuestions)
                {

                    var question = await _questionService.GetByIdWithTopic(examQuestion.QuestionId);
                    List<String> stringChoices = new List<string>();
                    List<ChoiceViewModels> _choices = new List<ChoiceViewModels>();
                    foreach (Choice c in question.Choices)
                    {
                        stringChoices.Add(c.Text);
                        _choices.Add (new ChoiceViewModels()
                        {
                            Id=c.Id,
                            Text=c.Text,
                            Type=c.Type,
                            Question=c.Question
                        });

                    }
                    string stringChoice = JsonConvert.SerializeObject(stringChoices); //  [1,2,6,7,8,18,25,61,129]

                    ExamQuestionViewModel curr = new ExamQuestionViewModel()
                    {
                        Order= examQuestion.Order,
                        QuestionId=examQuestion.QuestionId,
                        Question=new QuestionIndexViewModel()
                        {
                            Id = question.Id,
                            Text = question.Text,
                            Mark = question.Mark,
                            IsHtml = question.IsHtml,
                            TopicId = question.TopicId,
                            TopicName = question.Topic.Name,
                            Choices=_choices,
                            choices_string= stringChoice
                        }
                    };
                    _ExamQuestionsViewModel.Add(curr);
                }
                ExamViewModel currQVM = new ExamViewModel()
                {
                    Id = e.Id,
                    QuestionsCount = e.QuestionsCount,
                    CourseViewModel = new CourseModelView()
                    {
                        Id = e.Course.Id,
                        Name = e.Course.Name,
                        Code = e.Course.Code,
                        ImageURL = e.Course.ImageURL,
                        Status = e.Course.Status
                    },
                    TestCenterViewModel = new TestCenterExamViewModel()
                    {
                        Id = e.TestCenter.Id,
                        Name = e.TestCenter.ApplicationUser.Name,
                        Email = e.TestCenter.ApplicationUser.Email,
                        Phone = e.TestCenter.ApplicationUser.PhoneNumber,
                        UserType = e.TestCenter.userType,
                        ImageURL = e.TestCenter.ApplicationUser.ImageURL
                    },
                    DurationInMinutes = e.DurationInMinutes,
                    ExamDateTime = e.ExamDate,

                    ExamQuestionsViewModel = _ExamQuestionsViewModel
                };
                ExamViewModel.Add(currQVM);
            }
            return View(ExamViewModel);
        }

        // GET: Exams/Details/5
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
            
            var exam = await _ExamService.GetExamWithAttributes(id);
            if (exam == null)
            {
                return NotFound();
            }
            CourseModelView _courseViewModel = new CourseModelView()
            {
                ///////////////////////////////////
                Id = exam.Course.Id,
                Name = exam.Course.Name,
                Code = exam.Course.Code,
                ImageURL = exam.Course.ImageURL
                /// ///////////////////////////////
                ///
            };
            TestCenterExamViewModel _testCenterVM = new TestCenterExamViewModel()
            {
                Id = exam.TestCenter.Id,
                Name = exam.TestCenter.ApplicationUser.Name,
                Email = exam.TestCenter.ApplicationUser.Email,
                UserType = MyEnums.UserType.TestCenter,
                ImageURL = exam.TestCenter.ApplicationUser.ImageURL
            };
            List<ExamQuestionViewModel> _ExamQuestionViewModel = new List<ExamQuestionViewModel>();
            foreach (ExamQuestion temp in exam.ExamQuestions)
            {
                Question q = await _questionService.GetByIdWithTopic(temp.QuestionId);

                List<ChoiceViewModels> _choices = new List<ChoiceViewModels>();
                List<String> stringChoices = new List<string>();
                foreach (var choice in q.Choices)
                {
                    stringChoices.Add(choice.Text);
                    _choices.Add(new ChoiceViewModels()
                    {
                        
                        Text = choice.Text,
                        Type = choice.Type
                        
                    });
                }

                string stringChoice = JsonConvert.SerializeObject(stringChoices);
                QuestionIndexViewModel QuestionViewM = new QuestionIndexViewModel()
                {
                    Mark = q.Mark,
                    Choices = _choices,
                    Id = q.Id,
                    IsHtml = q.IsHtml,
                    Text = q.Text,TopicName=q.Topic.Name,
                    TopicId = q.TopicId,choices_string= stringChoice
                };
                ExamQuestionViewModel tempE = new ExamQuestionViewModel()
                {
                    Mark=temp.Mark,
                    Order=temp.Order,
                    QuestionId=temp.QuestionId,
                    Question=QuestionViewM,
                    ExamId=temp.ExamId
                };
                _ExamQuestionViewModel.Add(tempE);
            }
            List<QuestionIndexViewModel> _QuestionsViewModel = new List<QuestionIndexViewModel>();
            
            foreach (ExamQuestion temp in exam.ExamQuestions)
            {
                Question q = await _questionService.GetByIdWithTopic(temp.QuestionId);
                List<ChoiceViewModels> _choices = new List<ChoiceViewModels>();
                List<String> stringChoices = new List<string>();
                foreach (var choice in q.Choices)
                {
                    stringChoices.Add(choice.Text);
                    _choices.Add(new ChoiceViewModels()
                {

                    Text = choice.Text,
                    Type = choice.Type
                }); }

                string stringChoice = JsonConvert.SerializeObject(stringChoices);
                QuestionIndexViewModel tempQ = new QuestionIndexViewModel()
                {
                    Choices= _choices,
                    choices_string= stringChoice,
                    Id=q.Id,
                    IsHtml=q.IsHtml,
                    Mark=q.Mark,
                    Status=q.Status,
                    Text=q.Text,
                    TopicId=q.TopicId,
                    TopicName=q.Topic.Name
                };
                _QuestionsViewModel.Add(tempQ);
            }
            
            ExamViewModel _examViewModel = new ExamViewModel()
            {
                Id=exam.Id,
                DurationInMinutes=exam.DurationInMinutes,
                QuestionsCount =exam.QuestionsCount,
                CourseId =exam.CourseId,
                CourseViewModel =_courseViewModel,
                TestCenterId =exam.TestCenterId,
                TestCenterViewModel=_testCenterVM,
                ExamDateTime =exam.ExamDate,
                ExamQuestionsViewModel = _ExamQuestionViewModel,
                QuestionsViewModel = _QuestionsViewModel
            };
            return View(_examViewModel);
            
        }

        // GET: Exams/Create
        public IActionResult Create()
        {
          
            ViewBag.CurrentPage = "CreateExam";
            ////List<Topic> l = _topicService.getAllAsList();
            ////ViewData["TopicId"] = new SelectList(l, "Id", "Name");
            ////return View();
            List<Course> c = _CourseService.GetAllAsList();
            List<TestCenter> t = _TestCenterService.GetAllAsListWithAppUser();
            ViewData["CourseId"] = new SelectList(c, "Id", "Name");
            ViewData["TestCenterId"] = new SelectList(t, "Id", "ApplicationUser.Name");
            return View();
        }

        // POST: Exams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DurationInMinutes,QuestionsCount,CourseId,TestCenterId,ExamDateTime")] ExamViewModel exam)
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = JsonConvert.DeserializeObject<Message>((string)TempData["Message"]);
            }

            String message;
            Course  _course = await _CourseService.GetByIdAsync(exam.CourseId);
            TestCenter _testcenter = await _TestCenterService.GetByIdAsync(exam.TestCenterId);
            
            List<ExamQuestion> examQues = _GenerateExamService.GenerateExam(exam.QuestionsCount,exam.CourseId);
            if (examQues == null)
            {
                TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Failed To Generate Exam! No Enough Questions Found!"));
                List<Course> co = _CourseService.GetAllAsList();
                List<TestCenter> te = _TestCenterService.GetAllAsListWithAppUser();
                ViewData["CourseId"] = new SelectList(co, "Id", "Name");
                ViewData["TestCenterId"] = new SelectList(te, "Id", "ApplicationUser.Name");
                return View(exam);
            }
            
            //List<ExamQuestion> examQues = new List<ExamQuestion>();
            //int i = 1;
            //foreach (ExamQuestion q in Questions)
            //{
            //    examQues.Add(new ExamQuestion() {Order=i,QuestionId=q.Id,Question= q });
            //    i++;
            //}
           
            if (ModelState.IsValid)
            {
                Exam _exam = new Exam()
                {
                    Id = new Guid(),
                    CourseId = exam.CourseId,
                    DurationInMinutes = exam.DurationInMinutes,
                    Course = _course,
                    TestCenter = _testcenter,
                    ExamQuestions = examQues,
                    IsRandom = false,
                    QuestionsCount = exam.QuestionsCount,
                    TestCenterId = exam.TestCenterId,
                    ExamDate=exam.ExamDateTime
                };
                await _ExamService.AddAsync(_exam);
                 message = "A New Exam has been Generated And Added Successfully";
                TempData["Message"] = JsonConvert.SerializeObject(Message.AddSuccessMessage(message));
                return RedirectToAction("Details", "Exams", new { id = _exam.Id });
           }

            List<Course> c = _CourseService.GetAllAsList();
            List<TestCenter> t = _TestCenterService.GetAllAsListWithAppUser();
            ViewData["CourseId"] = new SelectList(c, "Id", "Name");
            ViewData["TestCenterId"] = new SelectList(t, "Id", "ApplicationUser.Name");
            TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Failed To Create/Add Exam!"));
            return View(exam);

        }

        // GET: Exams/Edit/5
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
            var exam = await _ExamService.GetByIdAsync(id); 
            if (exam == null)
            {
                return NotFound();
            }
            ExamViewModel _exam = new ExamViewModel() {
                Id=exam.Id,
                DurationInMinutes = exam.DurationInMinutes,
                QuestionsCount = exam.QuestionsCount,
                TestCenterId = exam.TestCenterId,
                ExamDateTime = exam.ExamDate

            };
            List<TestCenter> t = _TestCenterService.GetAllAsListWithAppUser();
            ViewData["TestCenterId"] = new SelectList(t, "Id", "ApplicationUser.Name","ApplicationUser.Name");
            return View(_exam);
        }

        // POST: Exams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DurationInMinutes,TestCenterId,ExamDateTime")] ExamViewModel exam)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    Exam _exam = new Exam() {
                        Id=id,
                    //Course = exam.Course;
                    //_exam.CourseId = exam.CourseId;
                    DurationInMinutes = exam.DurationInMinutes,
                    ExamDate = exam.ExamDateTime,
                    //ExamQuestions = new List<ExamQuestion>();
                    //_exam.ExamQuestions = exam.ExamQuestions;
                    //_exam.IsRandom = exam.IsRandom;
                    //_exam.QuestionsCount = exam.QuestionsCount;
                    //_exam.Status = exam.Status;
                    //_exam.Submissions = exam.Submissions;
                    //_exam.TestCenter = exam.TestCenter;
                    TestCenterId = exam.TestCenterId
                };
                    await _ExamService.specialUpdateAsync(_exam);
                    TempData["Message"] = JsonConvert.SerializeObject(Message.AddSuccessMessage("The Exam was edited successfully"));
                    return RedirectToAction("Details", "Exams", new { id = id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation failed!"));
                    return View(id);

                }
              
            }
            TempData["Message"] = JsonConvert.SerializeObject(Message.AddFailedMessage("Operation Failed!"));
            return View(id);
        }

        // GET: Exams/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            //var exam = await _context.Exams
            //    .Include(e => e.Course)
            //    .Include(e => e.TestCenter)
            //    .FirstOrDefaultAsync(m => m.Id == id);
            if (id == null)
            {
                return NotFound();
            }
            var exam = await _ExamService.GetByIdAsync(id);

            if (exam == null)
            {
                return NotFound();
            }
            await _ExamService.Deactivate(exam);
            return RedirectToAction(nameof(Index));
        }

        // POST: Exams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var exam = await _ExamService.GetByIdAsync(id);
            await _ExamService.Deactivate(exam);
            return RedirectToAction(nameof(Index));
            
        }

        private bool ExamExists(Guid id)
        {
            var exam = _ExamService.GetByIdAsync(id);
            if (exam != null)
                return true;
            return false;
        }
    }
}
