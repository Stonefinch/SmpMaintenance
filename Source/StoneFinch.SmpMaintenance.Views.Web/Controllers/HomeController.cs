using StoneFinch.SmpMaintenance.Data;
using StoneFinch.SmpMaintenance.Models;
using StoneFinch.SmpMaintenance.Views.Web.Interop;
using StoneFinch.SmpMaintenance.Views.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace StoneFinch.SmpMaintenance.Views.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(
            IReferenceDictionaryProvider referenceDictionaryProvider,
            IUserRepository userRepository)
        {
            this.ReferenceDictionaryProvider = referenceDictionaryProvider;
            this.UserRepository = userRepository;
        }

        private IReferenceDictionaryProvider ReferenceDictionaryProvider { get; set; }

        private IUserRepository UserRepository { get; set; }

        public ActionResult Index()
        {
            // if a connectionString has already been configured, display it
            var rd = this.ReferenceDictionaryProvider.GetReferenceDictionary();
            this.ViewBag.ConnectionString = rd.ConnectionString;

            return View();
        }

        [HttpPost]
        public ActionResult Index(string c)
        {
            try
            {
                // test connection string
                var connection = new SqlConnection(c);

                connection.Open();
                connection.Close();
                connection.Dispose();

                // connection string valid, set connection string, will be used by UserRepository
                var rd = this.ReferenceDictionaryProvider.GetReferenceDictionary();
                rd.ConnectionString = c;

                // test database schema
                // attempt to retrieve single UserProfile, this will create a new SmpContext which will verify the Schema
                var userProfileSearchCriteria = new UserProfileSearchCriteria() { ResultLimit = 1 };
                var userProfile = this.UserRepository.SelectUserProfiles(userProfileSearchCriteria);

                // initialize provider
                // NOTE: can be done only once per app start
                WebSecurity.InitializeDatabaseConnection(rd.ConnectionString, "System.Data.SqlClient", "UserProfile", "UserId", "UserName", false);

                // if we got this far, everything worked, redirect to User List
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                // some issue, display it
                this.ModelState.AddModelError("", "There was an error while attempting to connect to the database.  See below for details.");
                this.ModelState.AddModelError("", ex.Message);
                this.ModelState.AddModelError("", ex.ToString());
            }

            // display given connection string if page needs to reload
            this.ViewBag.ConnectionString = c;

            return View();
        }

        public ActionResult UserList()
        {
            var vm = new UserListViewModel();

            var userProfiles = this.UserRepository.SelectUserProfiles(new UserProfileSearchCriteria());
            vm.UserProfileViewModels = userProfiles.Select(x => Mapper.Map(x)).ToList();

            vm.AllRoles = this.GetAllRolesSelectListWithAny();

            return View(vm);
        }

        [HttpPost]
        public ActionResult UserList(UserProfileSearchCriteriaViewModel userProfileSearchCriteriaViewModel)
        {
            var vm = new UserListViewModel();
            vm.UserProfileSearchCriteriaViewModel = userProfileSearchCriteriaViewModel;

            var sc = new UserProfileSearchCriteria();

            sc.UserName = userProfileSearchCriteriaViewModel.UserName;
            sc.RoleName = userProfileSearchCriteriaViewModel.RoleName;
            sc.ResultLimit = userProfileSearchCriteriaViewModel.ResultLimit;

            var userProfiles = this.UserRepository.SelectUserProfiles(sc);
            vm.UserProfileViewModels = userProfiles.Select(x => Mapper.Map(x)).ToList();

            vm.AllRoles = this.GetAllRolesSelectListWithAny();

            return View(vm);
        }

        public ActionResult UserAdd()
        {
            var vm = new UserAddViewModel();

            vm.AllRoles = this.GetAllRolesSelectListWithBlank();

            return View(vm);
        }

        [HttpPost]
        public ActionResult UserAdd(UserAddViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(vm.UserName, vm.Password);

                    foreach (var roleName in vm.Roles)
                    {
                        Roles.AddUserToRole(vm.UserName, roleName);
                    }

                    return RedirectToAction("UserList");
                }
                catch (Exception ex)
                {
                    this.ModelState.AddModelError("", "There was an issue while trying to create the user.");
                    this.ModelState.AddModelError("", ex.Message);
                    this.ModelState.AddModelError("", ex.ToString());
                }
            }

            vm.AllRoles = this.GetAllRolesSelectListWithBlank();

            return View(vm);
        }

        public ActionResult UserBulkAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserBulkAdd(string userBulkAddCsv)
        {
            if (String.IsNullOrWhiteSpace(userBulkAddCsv))
                return RedirectToAction("UserList");

            try
            {
                var issues = new List<string>();

                var lines = userBulkAddCsv.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    try
                    {
                        var parts = line.Split(',');

                        if (parts.Length != 3)
                        {
                            issues.Add("Line has incorrect length. " + line);
                            continue;
                        }

                        var userName = parts[0];
                        var password = parts[1];
                        var rolesString = parts[2];

                        var roles = String.IsNullOrWhiteSpace(rolesString) ? new string[0] : rolesString.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                        if (WebSecurity.UserExists(userName))
                        {
                            issues.Add("User Already Exists. " + line);
                            continue;
                        }

                        WebSecurity.CreateUserAndAccount(userName, password);

                        foreach (var role in roles)
                        {
                            if (Roles.RoleExists(role))
                                Roles.AddUserToRole(userName, role);
                            else
                                issues.Add("Role does not exist. " + line);
                        }
                    }
                    catch (Exception ex)
                    {
                        issues.Add("Issue while trying to create user. " + line + ex.ToString());
                    }
                }

                if (issues.Count == 0)
                {
                    // success, redirect to user list
                    return RedirectToAction("UserList");
                }
                else
                {
                    foreach (var issue in issues)
                    {
                        this.ModelState.AddModelError("", issue);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", "There was an error while trying to add the users.");
                this.ModelState.AddModelError("", ex.Message);
                this.ModelState.AddModelError("", ex.ToString());
            }

            return View();
        }

        public ActionResult UserEdit(string userName)
        {
            var userProfiles = this.UserRepository.SelectUserProfiles(new UserProfileSearchCriteria() { UserName = userName });

            if (userProfiles != null
                && userProfiles.Count() == 1)
            {
                var userProfile = userProfiles.First();

                var vm = new UserEditViewModel();

                vm.UserId = userProfile.UserId;
                vm.UserName = userProfile.UserName;
                vm.Roles = userProfile.Roles.Select(x => x.RoleName).ToList();

                vm.AllRoles = this.GetAllRolesSelectListWithBlank();

                return View(vm);
            }

            // user with given userName does not exist, redirect to list
            return RedirectToAction("UserList");
        }

        [HttpPost]
        public ActionResult UserEdit(UserEditViewModel vm)
        {
            if (vm == null || !WebSecurity.UserExists(vm.UserName))
                return RedirectToAction("UserList");

            try
            {
                // ensure vm.Roles is not null
                vm.Roles = vm.Roles ?? new List<string>();

                var currentRoles = Roles.GetRolesForUser(vm.UserName);

                // ensure not null
                currentRoles = currentRoles ?? new string[0];

                // find roles to remove user from
                foreach (var removeRole in currentRoles.Except(vm.Roles))
                {
                    if (Roles.IsUserInRole(vm.UserName, removeRole))
                    {
                        Roles.RemoveUserFromRole(vm.UserName, removeRole);
                    }
                }

                // find new roles to add to user
                foreach (var addRole in vm.Roles.Except(currentRoles))
                {
                    if (Roles.RoleExists(addRole)
                        && !Roles.IsUserInRole(vm.UserName, addRole))
                    {
                        Roles.AddUserToRole(vm.UserName, addRole);
                    }
                }

                // if reset password provided, reset the users password
                if (!String.IsNullOrWhiteSpace(vm.ResetPassword))
                {
                    var token = WebSecurity.GeneratePasswordResetToken(vm.UserName);
                    WebSecurity.ResetPassword(token, vm.ResetPassword);
                }

                // update success, redirect back to list
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError("", "There was an issue while trying to update the user.");
                this.ModelState.AddModelError("", ex.Message);
                this.ModelState.AddModelError("", ex.ToString());

                return View(vm);
            }
        }

        public ActionResult RoleAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RoleAdd(string r)
        {
            if (!String.IsNullOrWhiteSpace(r)
                && !Roles.RoleExists(r))
            {
                Roles.CreateRole(r);

                return RedirectToAction("UserList");
            }

            return View();
        }

        private List<SelectListItem> GetAllRolesSelectList()
        {
            var roles = this.UserRepository.SelectRoles();

            return
                roles
                .Select(x => new SelectListItem()
                {
                    Value = x.RoleName,
                    Text = x.RoleName
                })
                .ToList();
        }

        private List<SelectListItem> GetAllRolesSelectListWithAny()
        {
            var roles = this.GetAllRolesSelectList();

            roles.Insert(0, new SelectListItem() { Value = "", Text = "Any" });

            return roles;
        }

        private List<SelectListItem> GetAllRolesSelectListWithBlank()
        {
            var roles = this.GetAllRolesSelectList();

            roles.Insert(0, new SelectListItem() { Value = "", Text = "" });

            return roles;
        }
    }
}