using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using VehicleManagementApp.BLL;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.ViewModels;

namespace VehicleManagementApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        //public AccountController()
        //{
        //}

        //public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        //{
        //    UserManager = userManager;
        //    SignInManager = signInManager;
        //}

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    //return returnUrl != null ? RedirectToLocal(returnUrl) : RedirectToAction("Dashboard", "Home");
                    return RedirectToAction("Dashboard", "Home");

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        //
        // GET: /Account/Register

        //private IEmployeeManager _employeeManager;
        //private IDepartmentManager _departmentManager;
        //private IDesignationManager _designationManager;
        //private IDivisionManager _divisionManager;
        //private IDistrictManager _districtManager;
        //private IThanaManager _thanaManager;
        //public AccountController(IEmployeeManager employee, IDepartmentManager department,
        //   IDesignationManager designation,
        //   IDivisionManager division, IDistrictManager district, IThanaManager thana)
        //{
        //    this._employeeManager = employee;
        //    this._departmentManager = department;
        //    this._designationManager = designation;
        //    this._divisionManager = division;
        //    this._districtManager = district;
        //    this._thanaManager = thana;
        //}

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }



        IDepartmentManager departmentManager = new DepartmentManager();
        IDesignationManager designationManager = new DesignationManager();

        IDivisionManager divisionManager = new DivisionManager();
        IDistrictManager districtManager = new DistrictManager();
        IThanaManager thanaManager = new ThanaManager();

        IDriverStatusManager driverStatusManager = new DriverStatusManager();

        private void GetDropDownsValues(EmployeeRegisterWithRoleViewModel model)
        {
            var departments = departmentManager.GetAll();
            var designations = designationManager.GetAll();

            var divisions = divisionManager.GetAll();
            var districts = districtManager.GetAll();
            var thanas = thanaManager.GetAll();

            ViewBag.Departments = departments;
            ViewBag.Divisions = divisions;


            model.Designations = designations;
            model.Districts = districts;
            model.Thanas = thanas;
        }
        private static void AddEmployeeRole(ApplicationUser user)
        {
            var role = new IdentityUserRole();
            role.UserId = user.Id;
            // assign User role Id
            role.RoleId = "648d557d-307b-4a72-9555-5f60070d80c9";
            user.Roles.Add(role);
        }
        private static void AddOperatorRole(ApplicationUser user)
        {
            var role = new IdentityUserRole();
            role.UserId = user.Id;
            // assign User role Id
            role.RoleId = "00c884db-10ae-4fc2-acb4-177c50e25eeb";

            user.Roles.Add(role);
        }
        private static void AddDriverRole(ApplicationUser user)
        {
            var role = new IdentityUserRole();
            role.UserId = user.Id;
            // assign User role Id
            role.RoleId = "8a0b3939-369f-4f3a-ac22-5d1bb15fd169";
            user.Roles.Add(role);
        }
        private void CreateEmployee(EmployeeRegisterWithRoleViewModel model, ApplicationUser user, byte[] imageData)
        {
            IEmployeeManager employeeManager = new EmployeeManager();
            Employee employee = new Employee
            {
                Name = model.Name,
                UserId = user.Id,
                Email = model.Email,
                DepartmentId = model.DepartmentId,
                DesignationId = model.DesignationId,
                DivisionId = model.DivisionId,
                DistrictId = model.DistrictId,
                ThanaId = model.ThanaId,
                Address1 = model.Address1,
                //Address2 = model.Address2,
                ContactNo = model.ContactNo,
                IsDriver = false,
                Image = imageData,
                ImagePath = "~/EmployeeImages/" + model.Name + DateTime.Now,
                IsDeleted = false,
                UserRole = "Employee"
            };

            employeeManager.Add(employee);

            //}
        }
        private void CreateDriver(EmployeeRegisterWithRoleViewModel model, ApplicationUser user, byte[] imageData)
        {
            IEmployeeManager employeeManager = new EmployeeManager();
            Employee employee = new Employee
            {
                Name = model.Name,
                UserId = user.Id,
                Email = model.Email,
                DepartmentId = model.DepartmentId,
                DesignationId = model.DesignationId,
                DivisionId = model.DivisionId,
                DistrictId = model.DistrictId,
                ThanaId = model.ThanaId,
                Address1 = model.Address1,
                //Address2 = model.Address2,
                ContactNo = model.ContactNo,
                LicenceNo = model.LicenceNo,
                IsDriver = true,
                Image = imageData,
                ImagePath = "~/EmployeeImages/" + model.Name + DateTime.Now,
                IsDeleted = false,
                UserRole = "Driver"
            };

            employeeManager.Add(employee);
            var allEmployees = employeeManager.GetAll();
            var maxEmployeeId = allEmployees.Max(c => c.Id);

            AddDataToDriverStatusTable(maxEmployeeId);
        }
        private void AddDataToDriverStatusTable(int? driverId)
        {
            if (driverId == null)
            {
                return;
            }

            DriverStatus dv = new DriverStatus
            {

                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                EmployeeId = (int)driverId,
                Status = "New Recurit"
            };
            bool isSaved = driverStatusManager.Add(dv);
            if (isSaved)
            {
                return;
            }
            return;
        }
        // GET: /Account/EmployeeRegisterWithRole
        [HttpGet]
        [Authorize(Roles = "Controller,Operator")]
        public ActionResult EmployeeRegisterWithRole()
        {
            var departments = departmentManager.GetAll();
            var divisions = divisionManager.GetAll();

            EmployeeRegisterWithRoleViewModel model = new EmployeeRegisterWithRoleViewModel();
            model.Designations = new List<Designation>();
            model.Districts = new List<District>();
            model.Thanas = new List<Thana>();


            ViewBag.Departments = departments;
            ViewBag.DesignationId = new SelectListItem[] { new SelectListItem() { Value = "", Text = "Select Department First...." } };

            ViewBag.Divisions = divisions;
            ViewBag.DistrictId = new SelectListItem[] { new SelectListItem() { Value = "", Text = "Select Division First....." } };
            ViewBag.ThanaId = new SelectListItem[] { new SelectListItem() { Value = "", Text = "Select District First...." } };


            return View(model);
        }
        public static string GetFileNameToSave(string s)
        {
            return s
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
        }
        private byte[] GetImageData(string imgName)
        {
            byte[] imageData = null;

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase imgFile = Request.Files["UserPhoto"];
                if (imgFile != null && imgFile.ContentLength > 0)
                {
                    var fileName = GetFileNameToSave(imgName + DateTime.Now);
                    imgFile.SaveAs(Server.MapPath("~/EmployeeImages/" + fileName));
                    //imgFile.SaveAs(Server.MapPath(Path.Combine()));

                    using (var binary = new BinaryReader(imgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(imgFile.ContentLength);
                    }
                }
            }
            return imageData;
        }
        public bool HasFile(byte[] file)
        {

            return file != null;
        }

        //
        // POST: /Account/EmployeeRegisterWithRole
        [HttpPost]
        [Authorize(Roles = "Controller,Operator")]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeRegisterWithRole([Bind(Exclude = "UserPhoto")]EmployeeRegisterWithRoleViewModel model)
        {

            // ModelState.Remove("UserPhoto");
            if (ModelState.IsValid)
            {
                var imageData = GetImageData(model.Name);
                if (HasFile(imageData))
                {
                    // To convert the user uploaded Photo as Byte Array before save to DB

                    var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                    user.Id = Guid.NewGuid().ToString();
                    var password = "1234";
                    user.UserPhoto = imageData;

                    if (model.Role == "Employee")
                    {
                        AddEmployeeRole(user);
                        CreateEmployee(model, user, imageData);
                    }
                    if (model.Role == "Operator")
                    {
                        AddOperatorRole(user);
                        CreateEmployee(model, user, imageData);
                    }
                    if (model.Role == "Driver")
                    {
                        AddDriverRole(user);
                        CreateDriver(model, user, imageData);
                    }

                    var result = UserManager.Create(user, password);
                    if (result.Succeeded)
                    {
                        TempData["msg"] = model.Role + " Saved Successfully!";
                        return RedirectToAction("EmployeeRegisterWithRole");
                    }

                    AddErrors(result);
                }
                else
                {
                    TempData["msgPhoto"] = "Please give user photo";
                    GetDropDownsValues(model);
                    return View(model);
                }

            }
            TempData["msg"] = model.Role + " Not Saved!";

            GetDropDownsValues(model);
            return View(model);
        }
        [AllowAnonymous]
        public JsonResult SuggestUserName(string name, string contactNo)
        {
            if (name != null && contactNo != null)
            {
                var trimContactNo = contactNo.Substring(contactNo.Length - 4);

                var result = name + trimContactNo;
                //Response.AddHeader("X-ID", "result");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);

        }
        [AllowAnonymous]
        public JsonResult UserAlreadyExists(string userName)
        {
            ApplicationUser result =
                 UserManager.FindByName(userName);

            if (result == null)
            {
                // add the id that you want to communicate to the client
                // in case of validation success as a custom HTTP header
                //Response.AddHeader("X-ID", "123");
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json("User Name Already Exist, Try Another!", JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}