using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoneFinch.SmpMaintenance.Models
{
    [Table("webpages_Roles")]
    public class Role
    {
        public Role()
        {
            this.UserProfiles = new List<UserProfile>();
        }

        [Key]
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}