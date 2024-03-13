using Microsoft.AspNetCore.Mvc;

namespace CinemaSystem.Controllers
{
    public class QRController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("http://127.0.0.1:5501/qr.html");
        }
    }
}
