using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shiro.Models
{
    [Table("UserGroup", Schema = "usr")]
    public partial class UserGroup
    {
        [Column("groupId")]
        public Guid GroupId { get; set; }

        [Column("userId")]
        public Guid UserId { get; set; }
    }
}
