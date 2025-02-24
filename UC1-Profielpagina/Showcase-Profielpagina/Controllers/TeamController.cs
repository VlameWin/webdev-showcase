using Microsoft.AspNetCore.Mvc;

namespace Showcase_Profielpagina.Controllers
{
    [Route("/team")]
    public class TeamController : Controller
    {
        public IActionResult Topteam()
        {
            return View();
        }
    }
}
