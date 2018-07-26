using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shiro.Models
{
    [Table("User", Schema = "usr")]
    public partial class User
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("username")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [Column("firstName")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [Column("lastName")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Column("isAdministrator")]
        public bool IsAdministrator { get; set; }

        [Column("userGroupId")]
        public Guid? UserGroupId { get; set; }
    }
}
