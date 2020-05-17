using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.DTOs
{
    public class UserDTO
    {
        [Required]
        [EmailAddress]
        public string EmailAdress { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "Password is limited to {2} to {1} characters", MinimumLength = 6)]
        public string Password{ get; set; }
    }
}
