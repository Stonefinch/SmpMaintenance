namespace StoneFinch.SmpMaintenance.Views.Web.Models
{
    public class UserProfileSearchCriteriaViewModel
    {
        public UserProfileSearchCriteriaViewModel()
        {
            this.ResultLimit = 100;
        }

        public string UserName { get; set; }

        public string RoleName { get; set; }

        public int? ResultLimit { get; set; }
    }
}