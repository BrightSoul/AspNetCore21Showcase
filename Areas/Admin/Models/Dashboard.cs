using System;

namespace AspNetCore21Showcase.Admin.Models
{
    public class DashboardViewModel {
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