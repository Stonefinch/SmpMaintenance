using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoneFinch.SmpMaintenance.Models
{
    public class UserProfile
    {
        public UserProfile()
        {
            this.Roles = new List<Role>();
        }

        [Key]
        public int UserId { get; set; }

        public string UserName { get; set; }

        public ICollection<Role> Roles { get; set; }
    }
}