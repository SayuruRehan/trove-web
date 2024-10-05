using System;
using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class UserRegisterDTO
    {
        [Required]

        public string Firstname {get; set;}

        [Required]
        public string Lastname {get; set;}

        [Required]
        [EmailAddress]
        public string Email {get; set;}

        [Required]
        public string Phone {get; set;}

        [Required]
        public string Password {get; set;}
    }
}