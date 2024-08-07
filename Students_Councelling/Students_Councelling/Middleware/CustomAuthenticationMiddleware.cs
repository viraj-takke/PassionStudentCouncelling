using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Students_Councelling.Models.DAL;
using Students_Councelling.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Students_Councelling.Middleware
{
    public class CustomAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                var studentId = context.Request.Cookies["StudentId"];
                var email = context.Request.Cookies["Email"];

                if (!string.IsNullOrEmpty(studentId) && !string.IsNullOrEmpty(email))
                {
                    var dbContext = context.RequestServices.GetRequiredService<ApplicationDbContext>();
                    var student = await dbContext.students
                        .FirstOrDefaultAsync(s => s.StudentId.ToString() == studentId && s.Email == email);

                    if (student != null)
                    {
                        var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, student.StudentId.ToString()),
                        new Claim(ClaimTypes.Name, student.Email)
                    };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    }
                }
            }

            await _next(context);
        }
    }
}
