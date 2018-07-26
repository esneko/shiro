using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shiro.Models
{
    [Table("Group", Schema = "usr")]
    public partial class Group
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(350)]
        public string Name { get; set; }
    }
}
