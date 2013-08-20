using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StoneFinch.SmpMaintenance.Views.Web.Models
{
    public class UserAddViewModel
    {
        [Required]
        [StringLength(250)]
        public string UserName { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 10)]
        public string Password { get; set; }

        public List<string> Roles { get; set; }

        public List<SelectListItem> AllRoles { get; set; }
    }
}