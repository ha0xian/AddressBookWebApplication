using Microsoft.AspNetCore.Mvc;

namespace Address_Book.Controllers
{
    public class AddressBook : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
