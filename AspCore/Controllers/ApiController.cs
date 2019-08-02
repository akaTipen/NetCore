using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AspCore.Controllers
{
    public class ApiController : Controller
    {
        private readonly SocketManager _socketManager;

        public ApiController(SocketManager socketManager)
        {
            _socketManager = socketManager;
        }

        public async Task Report(double liquidTemp)
        {
            var reading = new
            {
                Date = DateTime.Now,
                LiquidTemp = liquidTemp
            };

            await _socketManager.SendMessageToAllAsync(JsonConvert.SerializeObject(reading));
        }

        public async Task Generate()
        {
            var rnd = new Random();

            for (var i = 0; i < 100; i++)
            {
                await Report(rnd.Next(23, 35));
                await Task.Delay(5000);
            }
        }
    }
}