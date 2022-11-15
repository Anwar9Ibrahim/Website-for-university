using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GroupC.Uni.Web.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }
        //this is used whenever the user tries to navigate to an URL that doesn't match any route
        [AllowAnonymous]
        [Route("/Error/{statusCode}")]
        public IActionResult Error404(int statusCode)
        {
            ViewBag.CurrentPage = "404 Error";
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resourse you Requested could not be found";
                    //ViewBag.Path = statusCodeResult.OriginalPath;
                    //ViewBag.Qs = statusCodeResult.OriginalQueryString;
                    logger.LogWarning($"404 Error occourd Path= {statusCodeResult.OriginalPath} " +
                        $"and Query string is {statusCodeResult.OriginalQueryString}");
                    break;
                //case 500:
                //    logger.LogWarning($"500 Error occourd Path= {statusCodeResult.OriginalPath} " +
                //        $"and Query string is {statusCodeResult.OriginalQueryString} currently unable to handle this request.");
                //    break;
            }
            return View("NotFound");
        }
        //this will be used if there ia an unhandeled exception
        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            // Retrieve the exception Details
            var exceptionHandlerPathFeature =
                    HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            //Security Risk
            //ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            //ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            //ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;
            //instade we will log them
            logger.LogError($"The Path {exceptionHandlerPathFeature.Path} threw an exception" +
                $" {exceptionHandlerPathFeature.Error}");
            
            
            
            //string ExceptionMessage = exceptionPathFeature.Error.Message;
            //if (exceptionPathFeature?.Error is FileNotFoundException)
            //{ ExceptionMessage = "File error thrown"; }
            //if (exceptionPathFeature?.Path == "/index")
            //{ ExceptionMessage += " from home page"; }

            return View("Error");
        }

    }

}