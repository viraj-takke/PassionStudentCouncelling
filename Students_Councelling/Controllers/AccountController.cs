using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Students_Councelling.Interface;
using Students_Councelling.Models.DTO;
using Students_Councelling.Models.Viewmodels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Students_Councelling.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public AccountController(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var student = await _accountRepository.LoginAsync(model.Email, model.Password);
            if (student != null)
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = model.RememberMe ? DateTime.Now.AddDays(Convert.ToInt32(_configuration["Sessiontimeout"])) : DateTime.Now.AddMinutes(20),
                    IsEssential = true,
                    Secure = true,
                    HttpOnly = true
                };
                Response.Cookies.Append("StudentId", student.StudentId.ToString(), options);
                Response.Cookies.Append("Email", student.Email, options);

                return Json(new { success = true, message = "Login successful", url = Url.Action("Index", "Home") });
            }
            return Json(new { success = false, message = "Username or password is incorrect" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("StudentId");
            Response.Cookies.Delete("Email");
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegisterStudentDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var studentId = _accountRepository.GetStudentIdByMailId(model.Email);

            if (studentId != 0)
            {
                return Json(new { success = false, message = "The EMail id is Already Registered!" });
            }

            var registrationResult = await _accountRepository.Registration_StudentsAsync(model);
            if (registrationResult)
            {
                var redirectUrl = Url.Action("Registration", "Account");
                return Json(new { success = true, message = "Registration successful", url = redirectUrl });
            }

            return Json(new { success = false, message = "Registration Failed!" });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            if (User.Identity.Name != null)
            {
                var student = await _accountRepository.GetStudentByMailId(User.Identity.Name);
                if (student == null)
                {
                    return NotFound();
                }
                return View(student);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(Students model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountRepository.EditProfileAsync(model);
            if (result)
            {
                return Json(new { success = true, message = "Profile Updated Successfully!", url = Url.Action("EditProfile", "Account") });
            }
            else
            {
                return Json(new { success = false, message = "Profile update failed!" });
            }

            return View(model);
        }

    }

}
