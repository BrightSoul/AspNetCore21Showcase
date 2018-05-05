using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AspNetCore21Showcase.Areas.Admin.Pages.Filters;

namespace AspNetCore21Showcase.Areas.Admin.Pages.Dashboard
{
    [LogFilter]
    public class DashboardModel : PageModel
    {
        public Task OnGetAsync()
        {
            //TODO: ottieni questi dati dal database
            Amount = 13750;
            Currency = "EUR";

            return Task.CompletedTask;
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