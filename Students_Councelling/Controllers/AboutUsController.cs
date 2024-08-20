using Microsoft.AspNetCore.Mvc;

namespace Students_Councelling.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
