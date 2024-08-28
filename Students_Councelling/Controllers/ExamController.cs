using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Students_Councelling.Controllers
{
    [Authorize]
    public class ExamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
