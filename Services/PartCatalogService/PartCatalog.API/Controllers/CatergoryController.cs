using Microsoft.AspNetCore.Mvc;

namespace PartCatalog.API.Controllers
{
    public class CatergoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
