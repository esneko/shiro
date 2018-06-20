using System;
using System.ComponentModel.DataAnnotations;

namespace Shiro.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
