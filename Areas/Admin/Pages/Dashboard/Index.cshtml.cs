using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCore21Showcase.Areas.Admin.Pages.Dashboard
{
    public class DashboardModel : PageModel
    {
        public async Task OnGetAsync()
        {
            //TODO: ottieni questi dati dal database
            Amount = 13750;
            Currency = "EUR";
        }

        public DateTime Timestamp
        {
            get {
                return DateTime.Now;
            }
        }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}