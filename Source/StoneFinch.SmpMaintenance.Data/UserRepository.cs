using StoneFinch.SmpMaintenance.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoneFinch.SmpMaintenance.Data
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(IConnectionStringProvider connectionStringProvider)
        {
            this.ConnectionStringProvider = connectionStringProvider;
        }

        private IConnectionStringProvider ConnectionStringProvider { get; set; }

        public IEnumerable<UserProfile> SelectUserProfiles(UserProfileSearchCriteria userProfileSearchCriteria)
        {
            IEnumerable<UserProfile> result;

            using (var ctx = this.CreateContext())
            {
                var query =
                    from u in ctx.UserProfiles.Include("Roles")
                    select u;

                // filter by UserName if provided
                if (!String.IsNullOrWhiteSpace(userProfileSearchCriteria.UserName))
                {
                    var userNameUpper = userProfileSearchCriteria.UserName.ToUpper();
                    query = query.Where(x => x.UserName.ToUpper().Contains(userNameUpper));
                }

                // filter by Role if provided
                if (!String.IsNullOrWhiteSpace(userProfileSearchCriteria.RoleName))
                {
                    query = query.Where(x => x.Roles.Select(r => r.RoleName).Contains(userProfileSearchCriteria.RoleName));
                }

                // limit results if ResultLimit provided
                if (userProfileSearchCriteria.ResultLimit.HasValue)
                {
                    query = query.Take(userProfileSearchCriteria.ResultLimit.Value);
                }

                result = query.OrderBy(x => x.UserName).ToList();
            }

            return result;
        }

        public IEnumerable<Role> SelectRoles()
        {
            IEnumerable<Role> result;

            using (var ctx = this.CreateContext())
            {
                var query =
                    from r in ctx.Roles
                    select r;

                result = query.OrderBy(x => x.RoleName).ToList();
            }

            return result;
        }

        private SmpContext CreateContext()
        {
            return new SmpContext(this.ConnectionStringProvider.GetConnectionString());
        }
    }
}