using AspNetCore21Showcase.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore21Showcase.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller {
        public IActionResult Index() {
            //A questa view viene fornito un DashboardViewModel di esempio
            //in modo che possa visualizzare dei dati riepilogativi in una dashboard

            var viewModel = new DashboardViewModel {
                Amount = 13750, //TODO: ottenere questi dati da un database, questa è solo una dimostrazione
                Currency = "EUR"
            };
            return View(viewModel);
        }
    }
}