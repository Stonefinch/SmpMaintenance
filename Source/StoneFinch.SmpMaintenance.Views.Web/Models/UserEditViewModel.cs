using System.Collections.Generic;
using System.Web.Mvc;

namespace StoneFinch.SmpMaintenance.Views.Web.Models
{
    public class UserEditViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string ResetPassword { get; set; }

        public List<string> Roles { get; set; }

        public List<SelectListItem> AllRoles { get; set; }
    }
}