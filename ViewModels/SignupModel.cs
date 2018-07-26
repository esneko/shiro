using System;
using System.ComponentModel.DataAnnotations;

namespace Shiro.Models
{
    public class SignupModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
