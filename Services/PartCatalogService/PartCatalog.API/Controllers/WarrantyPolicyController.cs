using Microsoft.AspNetCore.Mvc;

namespace PartCatalog.API.Controllers
{
    public class WarrantyPolicyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
