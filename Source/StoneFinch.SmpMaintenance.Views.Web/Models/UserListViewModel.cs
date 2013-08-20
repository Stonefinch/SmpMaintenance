using System.Collections.Generic;
using System.Web.Mvc;

namespace StoneFinch.SmpMaintenance.Views.Web.Models
{
    public class UserListViewModel
    {
        public UserListViewModel()
        {
            this.UserProfileSearchCriteriaViewModel = new UserProfileSearchCriteriaViewModel();
        }

        public UserProfileSearchCriteriaViewModel UserProfileSearchCriteriaViewModel { get; set; }

        public List<SelectListItem> AllRoles { get; set; }

        public List<UserProfileViewModel> UserProfileViewModels { get; set; }
    }
}