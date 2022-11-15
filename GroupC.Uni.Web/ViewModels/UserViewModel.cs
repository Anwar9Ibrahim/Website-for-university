using GroupC.Uni.Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroupC.Uni.Web.ViewModels
{
    public class CreateUserViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public StudentViewModel studentViewModel { get; set; }
        public TestCenterViewModel testCenterViewModel { get; set; }
        public MyEnums.UserType UserType { get; set; }
       // [Range(1,6)]
        public int Year { get; set; }
        public IFormFile Image { get; set; }

        public string ImageURL { get; set; }
    }
    public class StudentViewModel
    {
        public MyEnums.UserType UserType = MyEnums.UserType.Student;
        [Required]
        [Range(1, 6)]
        public int Year { get; set; }
    }
    public class TestCenterViewModel
    {
        public MyEnums.UserType UserType = MyEnums.UserType.TestCenter;
        
    }
    public class AdminViewModel
    {
        public MyEnums.UserType UserType = MyEnums.UserType.Admin;

    }
    public class ProfileViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public IFormFile Image { get; set; }

        public string ImageURL { get; set; }
        public MyEnums.UserType UserType { get; set; }
        public changePasswordViewModel changePasswordViewModel { get; set; }
    }
    public class changePasswordViewModel { 

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage =
            "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}