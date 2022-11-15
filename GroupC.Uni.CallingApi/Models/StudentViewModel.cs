using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GroupC.Uni.ConsumingApi.Models
{
    public class StudentViewModel
    {
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
        public int Year { get; set; }
        public IFormFile Image { get; set; }

        public string ImageURL { get; set; }
    }
}
 