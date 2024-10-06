using System.ComponentModel.DataAnnotations;
using backend.Models;

namespace backend.DTOs
{
    public class UserUpdateDTO
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }

        [EnumDataType(typeof(UserStatus))]
        public string Status { get; set; }
    }
}
