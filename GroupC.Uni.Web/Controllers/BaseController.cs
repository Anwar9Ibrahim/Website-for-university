using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GroupC.Uni.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GroupC.Uni.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly IHostingEnvironment _hostingEnv;
        public BaseController(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

       
        public string GetUniqueFileName(CreateUserViewModel CreateUserViewModel)
        {
            string uniqueFileName = null;
            if (CreateUserViewModel.Image != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnv.WebRootPath, "images");
                string imageId = Guid.NewGuid().ToString();
                uniqueFileName = imageId + "_" + CreateUserViewModel.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                CreateUserViewModel.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                uniqueFileName = "/images/" + imageId + "_" + CreateUserViewModel.Image.FileName;
            }
            else
            {
                uniqueFileName = "/images/userDefaultImage.png";
            }
            return uniqueFileName;
        }
        public string GetuniqueNameEdited(CreateUserViewModel CreateUserViewModel)
        {
            string uniqueFileName = null;
            if (!CreateUserViewModel.ImageURL.StartsWith("/images/"))
            {
                string uploadsFolder = Path.Combine(_hostingEnv.WebRootPath, "images");
                string imageId = Guid.NewGuid().ToString();
                uniqueFileName = imageId + "_" + CreateUserViewModel.ImageURL.ToString();
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                CreateUserViewModel.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                uniqueFileName = "/images/" + imageId + "_" + CreateUserViewModel.ImageURL.ToString();
            }
            else
            {
                uniqueFileName = CreateUserViewModel.ImageURL;
            }
            return uniqueFileName;
        }
        public string GetuniqueNameEdited(ProfileViewModel CreateUserViewModel)
        {
            string uniqueFileName = null;
            if (!CreateUserViewModel.ImageURL.StartsWith("/images/"))
            {
                string uploadsFolder = Path.Combine(_hostingEnv.WebRootPath, "images");
                string imageId = Guid.NewGuid().ToString();
                uniqueFileName = imageId + "_" + CreateUserViewModel.ImageURL.ToString();
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                CreateUserViewModel.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                uniqueFileName = "/images/" + imageId + "_" + CreateUserViewModel.ImageURL.ToString();
            }
            else
            {
                uniqueFileName = CreateUserViewModel.ImageURL;
            }
            return uniqueFileName;
        }
    }
}