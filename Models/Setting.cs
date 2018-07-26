using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shiro.Models
{
    [Table("Setting", Schema = "usr")]
    public partial class Setting
    {
        [Column("userId")]
        public Guid UserId { get; set; }

        [Column("type")]
        [StringLength(200)]
        public string Type { get; set; }

        [Required]
        [Column("value")]
        [StringLength(250)]
        public string Value { get; set; }
    }
}
