using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using StudentNav.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentNav.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

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
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        //
        // GET: /Manage/AddPhoneNumber
        public async Task<ActionResult> EditProfile()
        {
            var userId = User.Identity.GetUserId();
            var userProfile = await UserManager.FindByIdAsync(userId);
            ViewBag.LevelStudy = new SelectList(GlobVars.LevelOfStudy);
            var user = new UpdateUserProfileViewModel
            {
                FirstName = userProfile.FirstName,
                Surname = userProfile.Surname,
                FieldOfStudy = userProfile.FieldOfStudy,
                Gender = userProfile.Gender,
                InstitutionType = userProfile.InstitutionType,
                Race = userProfile.Race,
                LevelOfStudy = userProfile.LevelOfStudy,
                Grade = userProfile.Grade,
                HighSchool = userProfile.HighSchool,
                Institution = userProfile.Institution
            };
            return View(user);
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile([Bind(Include = "FirstName,Surname,FieldOfStudy,LevelOfStudy,HighSchool,Grade,Institution,Province,InstitutionType,Race,Gender,ProfileImagePath")] UpdateUserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = User.Identity.GetUserId();
            //var userProfile = await UserManager.FindByIdAsync(userId);
            var userProfile = db.Users.Find(userId);
            if (userProfile.ProfileImagePath == null && (model.ProfileImagePath == null))
            {
                return View(model);
            }
            userProfile.FirstName = model.FirstName;
            userProfile.Surname = model.Surname;
            userProfile.InstitutionType = model.InstitutionType;
            if (model.InstitutionType == InstitutionType.HighSchool)
            {
                userProfile.FieldOfStudy = null;
                userProfile.LevelOfStudy = null;
                userProfile.Institution = null;
                if (model.HighSchool != null)
                    userProfile.HighSchool = model.HighSchool;
                if (model.Grade == 0)
                    userProfile.Grade = model.Grade;
                if (model.Province != null)
                    userProfile.Province = model.Province;
            }
            else
            {
                if (model.FieldOfStudy != null)
                {
                    userProfile.FieldOfStudy = model.FieldOfStudy;
                }
                if (model.LevelOfStudy != null)
                    userProfile.LevelOfStudy = model.LevelOfStudy;

                if (model.Institution != null)
                    userProfile.Institution = model.Institution;
                userProfile.HighSchool = null;
                userProfile.Grade = 0;
                userProfile.Province = null;
            }
            userProfile.Race = model.Race;
            userProfile.Gender = model.Gender;
            if (model.ProfileImagePath != null)
            {
                var fileName = Path.GetFileName(model.ProfileImagePath.FileName);
                var serverPath = Path.Combine(Server.MapPath("/Images/ProfilePics/" + fileName));
                model.ProfileImagePath.SaveAs(serverPath);
                userProfile.ProfileImagePath = "/Images/ProfilePics/" + fileName;
            }
            db.Entry(userProfile).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: /Manage/AddPhoneNumber
        public ActionResult FileUploads()
        {
            var articlePics = Directory.GetFiles(Server.MapPath("/Images/ArticlePics/"));
            var articleFileNames = new List<string>();
            foreach (var item in articlePics)
            {
                articleFileNames.Add($"/Images/ArticlePics/{ item.Substring(item.LastIndexOf(@"\") + 1)}");
            }
            var fileUploadViewModel = new FileUploadsViewModel();
            fileUploadViewModel.ArticlePicFiles = articleFileNames;
            var adsFiles = new List<string>();
            for(int i = 1; i < 23; i++)
            {
                adsFiles.Add("ads"+i+".jpg");
            }
            fileUploadViewModel.AdPics = new List<string>(adsFiles);

            return View(fileUploadViewModel);
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FileUploads([Bind(Include = "ArticleOrAds ,uploads,Adsthing")] FileUploadsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.ArticleOrAds.Contains("Article"))
            {
                foreach (HttpPostedFileBase upload in model.uploads)
                {
                    var fileName = Path.GetFileName(upload.FileName);
                    var serverPath = Path.Combine(Server.MapPath("/Images/ArticlePics/" + fileName));
                    upload.SaveAs(serverPath);
                }
            }
            else
            {
                var count = model.Adsthing;
                foreach (HttpPostedFileBase upload in model.uploads)
                {
                    var fileName = Path.GetFileName(upload.FileName);
                    var serverPath = Path.Combine(Server.MapPath($"/Images/ads{count}.jpg"));
                    upload.SaveAs(serverPath);
                    count++;                    
                }
            }

            return RedirectToAction("Index");
        }

        // GET: /Account/ForgotPassword
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SuspendUser(string userId = null)
        {
            if (userId != null)
            {
                var date =(DateTime.Now).AddYears(5);
                await UserManager.SetLockoutEnabledAsync(userId, true);
                await UserManager.SetLockoutEndDateAsync(userId,date);
                db.SaveChanges();
            }
            var users = db.Users.Where(c=>c.LockoutEndDateUtc == null).Select(s => new UserViewModel
            {
                Id = s.Id,
                Name = s.FirstName ?? s.UserName,
                ProfilePic = s.ProfileImagePath ?? "/Images/user.png"
            });
            return View(users);
        }

        //
        // GET: /Account/ForgotPassword
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AssignRoles(string userId = null, string role = null)
        {
            if (userId != null && role != null)
            {
                if (!role.Contains("User"))
                {
                    await UserManager.AddToRoleAsync(userId, role);
                    await UserManager.RemoveFromRoleAsync(userId, "User");
                }
                else
                {
                    await UserManager.AddToRoleAsync(userId, role);
                }
                
            }
            var users = db.Users.Select(s => new UserViewModel
            {
                Id = s.Id,
                Name = s.FirstName ?? s.UserName,
                ProfilePic = s.ProfileImagePath ?? "/Images/user.png"
            });
            return View(users);
        }

        [AllowAnonymous]
        public string GetUserImage()
        {
            var userid = User.Identity.GetUserId();
            if (userid == null)
            {
                return "/Images/user.png";
            }
            else
            {
                var user = UserManager.FindById(userid);
                return user.ProfileImagePath ?? "/Images/user.png";
            }
        }

        [AllowAnonymous]
        public string GetUserName()
        {
            var userid = User.Identity.GetUserId();
            if (userid == null)
            {
                return "User";
            }
            else
            {
                var user = UserManager.FindById(userid);

                return user.FirstName?.Split(' ')[0] ?? "User";
            }
        }

        [AllowAnonymous]
        public string GetUserRole()
        {
            var userid = User.Identity.GetUserId();
            if (userid == null)
            {
                return "User";
            }
            else
            {
                var userRoles = UserManager.GetRoles(userid);
                return userRoles.Contains("User") ? "User" : "NotUser";
            }
        }

        [AllowAnonymous]
        public string GetBlogPermission()
        {
            var userid = User.Identity.GetUserId();
            if (userid == null)
            {
                return "NotYetAllowed";
            }
            else
            {
                var name = db.Users.Find(userid).FirstName;
                if (name == null)
                {
                    return "NotYetAllowed";
                }
                return "Allowed";
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Users()
        {
            return View(db.Users.ToList());
        }

        [Authorize]
        // GET: Users/Details/5
        public ActionResult UserDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
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

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion Helpers
    }
}