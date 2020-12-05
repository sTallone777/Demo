using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApp.Controllers
{
    public class ChartController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Media Chart";

            var task = GetChartData();
            task.Wait();

            return View(task.Result);
        }

        async Task<ChartView> GetChartData()
        {
            Uri url = new Uri("https://localhost:44382/api/FileFolder/MediaChart");

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<ChartView>(response);
            }
        }
    }
}