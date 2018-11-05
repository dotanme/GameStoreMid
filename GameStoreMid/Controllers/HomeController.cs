using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStoreMid.Migrations;
using Microsoft.AspNetCore.Mvc;
using GameStoreMid.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using unirest_net.http;
using IgdbAPI;
using GameStoreMid.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStoreMid.Controllers
{
    public class HomeController : Controller
    {
        private int offset = 0;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var bestsellers = _context.ProductOrder.Include(x => x.Product).Select(x => x.Product).GroupBy(x => x.ProductID).OrderByDescending(x => x.Count()).Select(x => x.First()).Take(5).ToList();
            ViewBag.bestsellers = bestsellers;
            return View();
        }

        public class PulseId
        {
            public int id { get; set; }
        }

        public class Pulse
        {
            public string title { get; set; }
            public string summary  { get; set; }
            public string url { get; set; }
            public long created_at { get; set; }
            public Cover pulse_image { get; set; }
        }

        private List<Pulse> GetPulses()
        {
            string url = "https://api-endpoint.igdb.com//pulses/";
            Task<HttpResponse<string>> jsonResponse = Unirest.get(url + "?limit=6&offset="+offset)
                .header("user-key", "de594b8b36d90687b800f09eba174443")
                .header("Accept", "application/json")
                .asJsonAsync<string>();
            offset += 6;
            var x = jsonResponse.Result.Body;
            var model = JsonConvert.DeserializeObject<List<PulseId>>(jsonResponse.Result.Body);

            StringBuilder ids = new StringBuilder();
            ids.Append(model[0].id);

            for(int i = 1; i < model.Count;i++)
            {
                ids.Append("," + model[i].id);
            }

            jsonResponse = Unirest.get(url + ids.ToString())
                .header("user-key", "de594b8b36d90687b800f09eba174443")
                .header("Accept", "application/json")
                .asJsonAsync<string>();
            var pulses = JsonConvert.DeserializeObject<List<Pulse>>(jsonResponse.Result.Body);
            x = jsonResponse.Result.Body;

            return pulses;

        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
