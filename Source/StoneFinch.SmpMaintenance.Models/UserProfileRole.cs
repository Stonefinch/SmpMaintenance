using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoneFinch.SmpMaintenance.Models
{
    [Table("webpages_UsersInRoles")]
    public class UserProfileRole
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }

        [Key, Column(Order = 1)]
        public int RoleId { get; set; }
    }
}