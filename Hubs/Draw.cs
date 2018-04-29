using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Threading.Tasks;
using AspNetCore21Showcase.Models;
using Microsoft.AspNetCore.SignalR;

namespace AspNetCore21Showcase.Hubs
{
    public class DrawHub : Hub {
        public async Task Draw(DrawPoint from, DrawPoint to) {
            await Clients.Others.SendAsync("draw", from, to);
        }

        public async Task Clear() {
            await Clients.Others.SendAsync("clear");
        }
    }
}