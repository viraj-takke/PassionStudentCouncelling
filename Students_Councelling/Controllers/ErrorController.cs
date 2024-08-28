using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Students_Councelling.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        [Route("{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            Log.Warning($"Unexpected Status Code: {statusCode}, OriginalPath: {statusCodeResult?.OriginalPath}");

            switch (statusCode)
            {
                case 404:
                    return View("NotFound");
                case 500:
                    return View("ServerError");
                default:
                    return View("GeneralError");
            }
        }

        [Route("")]
        public IActionResult General()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            Log.Error(exceptionDetails.Error, $"An exception occurred on path {exceptionDetails.Path}");

            return View("GeneralError");
        }
    }
}
