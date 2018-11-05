using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStoreMid.Data;
using GameStoreMid.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameStoreMid.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CartController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }


        //Get  method cart index
        [Authorize]
        public IActionResult Index(int? id)
        {
            if (id != null)
            {
            }
            var init = Request.Cookies.Where(v => v.Key == "product").FirstOrDefault();

            List<Product> products = new List<Product>();

            if (init.Key != null)
            {
                HashSet<String> productIds = Request.Cookies.Where(v => v.Key == "product").FirstOrDefault().Value.Split(",").ToHashSet();

                foreach (String product in productIds)
                {
                    if (!product.Equals("")) {
                        Product p = _context.Product.FirstOrDefault(x => x.ProductID == Convert.ToInt32(product));
                        if (p != null) { 
                            p.Deal = _context.Deal.FirstOrDefault(d => d.DealID == p.DealID);
                            products.Add(p);
                        }
                    }
                }
            }
            Models.Address addr = _context.ApplicationUser.Include(a=>a.Address).FirstOrDefault(u => u.UserName == User.Identity.Name).Address;
            string country = addr.Country;
            string city = addr.City;
            string street = addr.Street;
            bool haveAddr = false;
            if (country != null && city != null && street != null) {
                haveAddr = true;
            }
            ViewBag.havingAddress = haveAddr;
            ViewBag.products = products;
            return View();
        }

        public IActionResult updateAddress(string country, string city, string street, int zipCode)
        {
            ApplicationUser user = _context.ApplicationUser.Include(a => a.Address).FirstOrDefault(u => u.UserName == User.Identity.Name);
            user.Address.Country = country;
            user.Address.City = city;
            user.Address.Street = street;
            user.Address.ZipCode = zipCode;
            _context.Update(user);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public JsonResult CheckCookie()
        {
            var init = Request.Cookies.Where(v => v.Key == "product").FirstOrDefault();
            HashSet<String> productIds = new HashSet<String>();

            if (init.Key != null)
            {
                 var temp = Request.Cookies.Where(v => v.Key == "product").FirstOrDefault().Value.Split(",").ToHashSet();
                foreach(var item in temp)
                {
                    productIds.Add(item);
                }

                foreach (String product in temp)
                {
                    if (!product.Equals(""))
                    {
                        Product p = _context.Product.FirstOrDefault(x => x.ProductID == Convert.ToInt32(product));
                        if (p != null)
                        {
                            productIds.Remove(product);
                        }
                    }
                }
            }

            return Json(productIds);
        }
    }
}