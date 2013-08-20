using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoneFinch.SmpMaintenance.Models
{
    [Table("webpages_OAuthMembership")]
    public class OAuthMembership
    {
        [Key, Column(Order = 0)]
        public string Provider { get; set; }

        [Key, Column(Order = 1)]
        public string ProviderUserId { get; set; }

        public int UserId { get; set; }
    }
}