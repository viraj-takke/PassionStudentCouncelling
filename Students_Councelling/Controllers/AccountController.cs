using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Students_Councelling.Interface;
using Students_Councelling.Models.DTO;
using Students_Councelling.Models.Viewmodels;

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
            if (ModelState.IsValid)
            {
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

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("ErrorMessage", "Email or Password is Invalid");
            }

            return View(model);
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
        public async Task<IActionResult> Registration(Students model)
        {
            var result = await _accountRepository.Registration_StudentsAsync(model);
            if (result == true) 
            {
                ModelState.AddModelError("SuccessMessage", "Registered Successfully!");
            }
            else
            {
                ModelState.AddModelError("ErrorMessage", "Registration Failed!");
            }
            return View();
        }
    }

}
