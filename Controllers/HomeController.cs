using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCore21Showcase.Models;
using System.Runtime.InteropServices;
using System.Management;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Net;
using AspNetCore21Showcase.Services;

namespace AspNetCore21Showcase.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Aree()
        {
            return View();
        }

        public IActionResult GDPR()
        {
            return View();
        }

        public IActionResult HttpClient()
        {
            return View();
        }

        public async Task<IActionResult> HttpClientWithRetryLogic([FromServices] IHttpClientFactory clientFactory)
        {
            using (var client = clientFactory.CreateClient("ClientForThirdPartyWebApi")) {
                //Questa richiesta GET può fallire, quindi verrà tentanta più volte
                //secondo le disposizioni che abbiamo messo nel file Startup.cs
                //Abbiamo quindi centralizzato la logica di retry. In tutti i punti in cui usiamo
                //l'HttpClient, il codice resta semplice e leggibile così come segue 
                var response = await client.GetStringAsync($"{Request.Scheme}://{Request.Host}/Home/ThirdPartyApi");
                return Content(response);
            }
        }

        public async Task<IActionResult> HttpClientWithoutRetryLogic() {
            //In questo caso invece creiamo un'istanza di HttpClient nel modo tradizionale
            //Non verrà applicata alcuna retry logic
            using (var client = new HttpClient()) {
                var response = await client.GetStringAsync($"{Request.Scheme}://{Request.Host}/Home/ThirdPartyApi");
                return Content(response);
            }
        }

        public IActionResult ThirdPartyApi() {
            //Se il secondo è pari, restituito OK 200, altrimenti 500 Internal Server Error
            //per simulare una web api di terze parti che a volte dà errore
            if (DateTime.Now.Second % 2 == 0) {
                return Json(new { ServerTime = DateTime.Now.ToString() });
            } else {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        public IActionResult ApiController()
        {
            return View();
        }

        public IActionResult SignalR()
        {
            return View();
        }

        public IActionResult WinCompat() {
            List<DriveInfo> driveInfo = null;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                driveInfo = new List<DriveInfo>();
                var volumes = new ManagementObjectSearcher("Select * from Win32_Volume").Get();
                foreach (ManagementObject volume in volumes) {
                    var letter = volume["DriveLetter"];
                    var freeSpace = volume["FreeSpace"];
                    if (letter == null || freeSpace == null) continue;

                    var remainingSpaceInMB = Convert.ToInt32(ulong.Parse(freeSpace.ToString()) / 1024 / 1024);
                    driveInfo.Add(new DriveInfo { RemainingSpaceInMB = remainingSpaceInMB, Letter = letter.ToString() });
                }
            }
            return View(driveInfo);
        }

        public IActionResult WinCompatImage() {
            var ms = new System.IO.MemoryStream();
            using (var image = new Bitmap(150, 100)) {
                using (var graphics = Graphics.FromImage(image)) {
                    var brush = new LinearGradientBrush(new Rectangle(Point.Empty, image.Size), Color.Red, Color.Blue, 0f);
                    graphics.FillRectangle(brush, 0, 0, image.Width, image.Height);
                }
                image.Save(ms, ImageFormat.Jpeg);
            }
            ms.Position = 0;
            return File(ms, "image/jpeg");          
        }
    }
}
