using StoneFinch.SmpMaintenance.Models;
using System.Collections.Generic;

namespace StoneFinch.SmpMaintenance.Data
{
    public interface IUserRepository
    {
        IEnumerable<UserProfile> SelectUserProfiles(UserProfileSearchCriteria userProfileSearchCriteria);

        IEnumerable<Role> SelectRoles();
    }
}