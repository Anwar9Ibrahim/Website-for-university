using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GroupC.Uni.Web.Models;
using GroupC.Uni.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using GroupC.Uni.Core.Entities;
using GroupC.Uni.Core.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http.Internal;
using System.Net;

namespace GroupC.Uni.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ICourseService _courseService;
        private readonly ITopicService _topicService;
        private readonly IQuestionService _questionService;
        private readonly IExamService _examService;
        //private readonly IEmailSender _emailSender;
        //private readonly IAppLogger<ManageController> _logger;
        //private readonly UrlEncoder _urlEncoder;

        public HomeController(
      UserManager<ApplicationUser> userManager,
      SignInManager<ApplicationUser> signInManager,
      IStringLocalizer<HomeController> localizer,
      ICourseService courseService,
      ITopicService topicService,
      IQuestionService questionService,
      IExamService examService
      //IUserService userService
      /*IEmailSender emailSender,
      IAppLogger<ManageController> logger,
      UrlEncoder urlEncoder*/)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _localizer = localizer;
            _courseService=courseService;
            _topicService = topicService;
            _questionService = questionService;
            _examService = examService;
            // _userService = userService;

            //_emailSender = emailSender;
            //_logger = logger;
            //_urlEncoder = urlEncoder;
        }
        private async Task<string> getCoursesCountAsync()
        {
            var CoursesList = await _courseService.ListActiveSync();
            string result = CoursesList.Count().ToString();
            ViewData["CoursesCount"] = result;
            return result;

        }
        private async Task<string> getTopicsCountAsync()
        {
            var TopicsList = await _topicService.ListActiveSyncWithCourse();
            string result = TopicsList.Count().ToString();
            ViewData["TopicsCount"] = result;
            return result;
        }
        private async Task<string> getQuestionsCountAsync()
        {
            var QuestionsList = await _questionService.ListActiveSyncWithTopic();
            string result = QuestionsList.Count().ToString();
            ViewData["QuestionsCount"] = result;
            return result;
        }
        private async Task<string> getExamsCountAsync()
        {          
            var ExamsList = await _examService.ListAllAsync();
            string result = ExamsList.Count().ToString();
            ViewData["ExamsCount"] = result;
            return result;
        }
        private async Task<Dictionary<string, string>> GetLatestCoursesCountAsync()
        {
            var CoursesList = await _courseService.ListRecentCoursesSync();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (var course in CoursesList)
            {
                try
                {
                    if (course.Topics.Count() != 0)
                        dict.Add(course.Name, course.Topics.Count().ToString());
                    else
                        dict.Add(course.Name, "0");
                }
                catch
                {
                    continue;
                }

            }

           ViewData["CCourse"] = dict;
            return dict;
        }
    

        private async Task<Dictionary<DateTime, string>> GetLatestExamsAsync()
        {
            var ExamsList = await _examService.GetLatestExamsAsync();
            Dictionary<DateTime, string> dict = new Dictionary<DateTime, string>();
            foreach (var exam in ExamsList)
            {
                dict.Add(exam.CreationDate, exam.Course.Name);                
            }        
           ViewData["EExams"] = dict;
            return dict;
        }
        public IActionResult Index()
        {
            var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            // Culture contains the information of the requested culture
            var culture = rqf.RequestCulture.Culture;
            ViewData["Language"] = culture;
            string temp;
            temp = getCoursesCountAsync().Result;
            temp = getTopicsCountAsync().Result;
            temp = getQuestionsCountAsync().Result;
            temp = getExamsCountAsync().Result;
            var Temps =  GetLatestCoursesCountAsync().Result;
            var Tempss = GetLatestExamsAsync().Result;


            //return View();
            if (_signInManager.IsSignedIn(User))
            {
                var user = _userManager.FindByEmailAsync(User.Identity.Name).Result;
                if (user.UserType == MyEnums.UserType.Admin || user.UserType == MyEnums.UserType.TestCenter)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("login", "Account");
                }
            }
            else
            {
                return RedirectToAction("login", "Account");
            }
        }
        public IActionResult SetCulture(string id = "en")
        {
            string culture = id;
            Response.Cookies.Append(
               CookieRequestCultureProvider.DefaultCookieName,
               CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
               new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
           );

            ViewData["Message"] = "Culture set to " + culture;
            ViewData["Language"] = culture;
            return View("Home");
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true,  //critical settings to apply new culture
                    Path = "/",
                    HttpOnly = false,
                }
                    );
           
            return LocalRedirect(returnUrl);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}