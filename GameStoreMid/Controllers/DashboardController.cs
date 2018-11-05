using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreMid.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Orders()
        {
            return View();
        }
        public IActionResult Products()
        {
            return View();
        }
        public IActionResult Users()
        {
            return View();
        }
        public IActionResult Deals()
        {
            return View();
        }
        public IActionResult Tags()
        {
            return View();
        }
    }
}