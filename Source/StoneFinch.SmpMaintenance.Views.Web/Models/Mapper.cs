using StoneFinch.SmpMaintenance.Models;
using System;
using System.Linq;

namespace StoneFinch.SmpMaintenance.Views.Web.Models
{
    public class Mapper
    {
        public static UserProfileViewModel Map(UserProfile m)
        {
            var vm = new UserProfileViewModel();

            vm.UserId = m.UserId;
            vm.UserName = m.UserName;

            vm.Roles = String.Join(", ", m.Roles.OrderBy(x => x.RoleName).Select(r => r.RoleName));

            return vm;
        }
    }
}