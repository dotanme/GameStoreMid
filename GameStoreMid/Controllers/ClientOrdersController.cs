using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameStoreMid.Data;
using GameStoreMid.Models;
using Microsoft.AspNetCore.Authorization;
using GameStoreMid.Services;

namespace GameStoreMid.Controllers
{
    [Authorize]
    public class ClientOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly MLApriori _mlApriori;


        public ClientOrdersController(ApplicationDbContext context, MLApriori mlApriori)
        {
            _context = context;
            _mlApriori = mlApriori;
        }

        // GET: ClientOrders
        public async Task<IActionResult> Index()
        {
            List<Product> orderedProducts = _context.ProductOrder.Join(_context.Product, x => x.ProductID, y => y.ProductID, (x, y) => y).ToList();
            ViewBag.orderedProducts = orderedProducts;
            var applicationDbContext = _context.OrderClient.Include(c => c.ApplicationUser).Include(c => c.ProductOrders).Where(c=>c.ApplicationUser.Email == User.Identity.Name);

           
            //List<double> deals = new List<double>();
            //List<int> pids = orderedProducts.Select(o => o.ProductID).ToList();
            //foreach (int pid in pids)
            //{
            //    deals.Add(_context.Deal.FirstOrDefault(y => y.DealID == _context.Product.FirstOrDefault(x => x.ProductID == pid).DealID).PercentageDiscount);
            //}
            //ViewBag.deals = deals;

            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AdminIndex()
        {
            ViewData["mapskey"] = Startup.SettingFactory()["GoogleMaps:Key"];

            ViewBag.orderedProducts = _context.ProductOrder.Join(_context.Product, x => x.ProductID, y => y.ProductID, (x, y) => y).ToList();
            var applicationDbContext = _context.OrderClient.Include(c => c.ApplicationUser).Include(c => c.ProductOrders).OrderByDescending(c=>c.OrderDate).Take(100);
            return View(await applicationDbContext.ToListAsync());
        }



        // GET: ClientOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<ProductOrder> productOrder = _context.ProductOrder.Join(_context.OrderClient, x => x.OrderID, y => y.OrderID, (x, y) => x).Where(x=>x.OrderID==id).Include(x=>x.Product).ToList();
            List<Product> orderedProducts = productOrder.Select(x => x.Product).ToList();
            ViewBag.products = orderedProducts;
            ViewBag.deals = orderedProducts.Join(_context.Deal, x => x.DealID, y => y.DealID, (x, y) => y.PercentageDiscount).ToList(); ;


            var clientOrder = await _context.OrderClient
                .Include(c=>c.ProductOrders)
                .Include(c => c.ApplicationUser.Address)
                .SingleOrDefaultAsync(m => m.OrderID == id);
            if (clientOrder == null)
            {
                return NotFound();
            }
            if(clientOrder.ApplicationUser.UserName != User.Identity.Name)
            {
                return Unauthorized();

            }

            return View(clientOrder);
        }

        // GET: ClientOrders/Create
        public IActionResult Create()
        {
            var init = Request.Cookies.Where(v => v.Key == "quantity").FirstOrDefault();
            if (init.Key == null || init.Value == "null")
            {
                return RedirectToAction("Index","ClientOrders");
            }

            HashSet<int> productInts = getCart();
            List<double> deals = new List<double>();
            List<Product> products = new List<Product>();
            ClientOrder clientOrder = new ClientOrder();
            String[] selectedProducts = productInts.Select(x => x.ToString()).ToArray();
            List<int> quantity = Request.Cookies.FirstOrDefault(v => v.Key == "quantity").Value.Split(",").ToList().Select(s => int.Parse(s)).ToList();

            foreach (int pid in productInts) {
                Product p = _context.Product.Include(x => x.Deal).FirstOrDefault(x => x.ProductID == pid);
                products.Add(p);
                if (p.DealID!=null)
                    deals.Add(p.Deal.PercentageDiscount);
            }

            ViewBag.deals = deals;
            ViewBag.products = products;

            if (_context.OrderClient.Count() > 0)
                clientOrder.OrderID = _context.OrderClient.LastOrDefault().OrderID + 1;
            else
                clientOrder.OrderID = 1;

            clientOrder.OrderDate = DateTime.Now;
            clientOrder.ExpectedDate = DateTime.Now.AddDays(2);
            clientOrder.ProductOrders = new List<ProductOrder>();
            clientOrder.Total = 0;

            for (int i = 0; i < selectedProducts.Length; i++)
            {
                ProductOrder temp = new ProductOrder();
                temp.OrderID = Convert.ToInt32(clientOrder.OrderID);
                temp.Order = clientOrder;
                temp.ProductID = Convert.ToInt32(selectedProducts[i]);
                temp.Quantity = quantity[i]; //will be changed by the cart quantity with js probably
                temp.Product = _context.Product.Include(p=>p.Deal).FirstOrDefault(x => x.ProductID == Convert.ToInt32(selectedProducts[i]));
                clientOrder.ProductOrders.Add(temp);
            }
            ViewBag.currUser = _context.ApplicationUser.Include(x=>x.Address).FirstOrDefault(a => a.UserName == User.Identity.Name);
            return View(clientOrder);
        }

        public HashSet<int> getCart()
        {
            var init = Request.Cookies.Where(v => v.Key == "product").FirstOrDefault();
            if (init.Key != null)
            {
                HashSet<String> productIds = Request.Cookies.FirstOrDefault(v => v.Key == "product").Value.Split(",",StringSplitOptions.RemoveEmptyEntries).ToHashSet();
                HashSet<int> productInts = productIds.Select(s => int.Parse(s)).ToHashSet();
                return productInts;
            }
            else
            {
                return new HashSet<int>();
            }
        }

        // POST: ClientOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientOrder clientOrder,String[] orderedProducts)
        {
            ModelState.Remove("ProductOrders");
            if (ModelState.IsValid)
            {
                List<int> quantity = Request.Cookies.FirstOrDefault(v => v.Key == "quantity").Value.Split(",").ToList().Select(s => int.Parse(s)).ToList();
                //delete the cart cookie because the order has been done
                Response.Cookies.Delete("product");
                Response.Cookies.Delete("quantity");
                clientOrder.ApplicationUser = _context.ApplicationUser.FirstOrDefault(u => u.UserName == User.Identity.Name);
                clientOrder.ApplicationUserID = clientOrder.ApplicationUser.Id;
                clientOrder.ProductOrders = new List<ProductOrder>();

                for (int i = 0; i < orderedProducts.Length; i++)
                {
                    ProductOrder temp = new ProductOrder();
                    temp.OrderID = Convert.ToInt32(clientOrder.OrderID);
                    temp.Order = clientOrder;
                    temp.ProductID = Convert.ToInt32(orderedProducts[i]);
                    temp.Quantity = quantity[i]; //will be changed by the cart quantity with js probably
                    temp.Product = _context.Product.Include(p => p.Deal).FirstOrDefault(x => x.ProductID == Convert.ToInt32(orderedProducts[i]));
                    clientOrder.ProductOrders.Add(temp);
                }

                _context.Add(clientOrder);
                await _context.SaveChangesAsync();
                await _mlApriori.UpdateRecommendedProductsAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserID"] = new SelectList(_context.ApplicationUser, "Id", "Id", clientOrder.ApplicationUserID);
            return View(clientOrder);
        }

        // POST: ClientOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                //_context.ProductOrder.Where(p => p.OrderID == id).ToList().ForEach(p => _context.Remove(p));
                _context.BrowsingHistory.Where(b => b.ApplicationUser.Id == _context.OrderClient.FirstOrDefault(o=>o.OrderID==id).ApplicationUserID).ToList().ForEach(x => _context.Remove(x));
                _context.SaveChanges();

                var clientOrder = await _context.OrderClient.SingleOrDefaultAsync(m => m.OrderID == id);
                if(clientOrder == null)
                {
                    return NotFound();
                }
                _context.OrderClient.Remove(clientOrder);
                await _context.SaveChangesAsync();
            }catch(Exception e)
            {

            }
            var item = Request.Headers["Referer"][0].Split("/ClientOrders/")[1];
            return RedirectToAction(item =="" ? "" : item);
            //return RedirectToAction(nameof(Index));
        }

        private bool ClientOrderExists(int id)
        {
            return _context.OrderClient.Any(e => e.OrderID == id);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        //Get 100 most recent Orders
        public JsonResult MostRecentOrders()
        {
            var bh = _context.OrderClient.Include(x => x.ApplicationUser).OrderByDescending(x => x.OrderDate).Select(x => x.ApplicationUser.Address).ToHashSet().Take(100);
            List<Address> mylist = new List<Address>();
            foreach (var x in bh)
            {
                var myjson = new Address { address = x.Street + " ,"+x.City+ " ,"+x.Country};
                mylist.Add(myjson);
            }
            return Json(mylist);
        }
    }
    public class Address
    {
        public string address { get; set; }
    }
}
