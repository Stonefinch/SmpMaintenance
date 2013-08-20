namespace StoneFinch.SmpMaintenance.Models
{
    public class UserProfileSearchCriteria
    {
        public UserProfileSearchCriteria()
        {
            this.ResultLimit = 100;
        }

        public string UserName { get; set; }

        public string RoleName { get; set; }

        public int? ResultLimit { get; set; }
    }
}