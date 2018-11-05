using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameStoreMid.Data;
using GameStoreMid.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Dynamic;

namespace GameStoreMid.Controllers
{
    public class BrowsingHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrowsingHistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrator")]
        // GET: BrowsingHistories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BrowsingHistory.Include(b => b.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BrowsingHistories/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var browsingHistory = await _context.BrowsingHistory
                .Include(b => b.Product)
                .SingleOrDefaultAsync(m => m.BrowsingHistroyID == id);
            if (browsingHistory == null)
            {
                return NotFound();
            }

            return View(browsingHistory);
        }

        // GET: BrowsingHistories/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductName");
            return View();
        }

        // POST: BrowsingHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task Create(int ProductID)
        {
            DateTime time = DateTime.Now;
            BrowsingHistory browsing = new BrowsingHistory
            {
                UserName = User.Identity.Name,

                ApplicationUser = _context.ApplicationUser.FirstOrDefault(x => x.UserName == User.Identity.Name),
                //System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                ProductID = ProductID,
                Viewed = time
            };
            if (ModelState.IsValid)
            {
                _context.BrowsingHistory.Add(browsing);
                await _context.SaveChangesAsync();
                
            }
           
        }

        // GET: BrowsingHistories/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var browsingHistory = await _context.BrowsingHistory.SingleOrDefaultAsync(m => m.BrowsingHistroyID == id);
            if (browsingHistory == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductName", browsingHistory.ProductID);
            return View(browsingHistory);
        }

        // POST: BrowsingHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("BrowsingHistroyID,UserName,ProductID,Viewed")] BrowsingHistory browsingHistory)
        {
            if (id != browsingHistory.BrowsingHistroyID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(browsingHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrowsingHistoryExists(browsingHistory.BrowsingHistroyID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductName", browsingHistory.ProductID);
            return View(browsingHistory);
        }

        // GET: BrowsingHistories/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var browsingHistory = await _context.BrowsingHistory
                .Include(b => b.Product)
                .SingleOrDefaultAsync(m => m.BrowsingHistroyID == id);
            if (browsingHistory == null)
            {
                return NotFound();
            }

            return View(browsingHistory);
        }

        // POST: BrowsingHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var browsingHistory = await _context.BrowsingHistory.SingleOrDefaultAsync(m => m.BrowsingHistroyID == id);
            _context.BrowsingHistory.Remove(browsingHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrowsingHistoryExists(int id)
        {
            return _context.BrowsingHistory.Any(e => e.BrowsingHistroyID == id);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        //return json with 10 most viewed products
        public JsonResult getJson10MostViewed()
        {
            var bh = _context.BrowsingHistory.Include(x => x.Product).GroupBy(x => x.Product.ProductName);
            List<ProductViews> mylist = new List<ProductViews>();
            foreach (var x in bh)
            {
                var myjson = new ProductViews { Name = x.Key,Views =x.Count()};
                mylist.Add(myjson);
            }
            var list = mylist.OrderByDescending(x => x.Views).Take(10);
            return Json(list);
        }
    }
    public class ProductViews
    {
        public string Name { get; set; }
        public int Views { get; set; }
    }
}