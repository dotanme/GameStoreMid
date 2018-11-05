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

namespace GameStoreMid.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DealsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DealsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Deals
        public async Task<IActionResult> Index(string searchString)
        {
            var deals = _context.Deal.AsQueryable();
            if (!String.IsNullOrEmpty(searchString))
            {
                deals = deals.Where(s => s.Description.Contains(searchString) || s.PercentageDiscount.ToString().Contains(searchString));
            }
            return View(await deals.ToListAsync());
        }

        // GET: Deals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deals = await _context.Deal
                .SingleOrDefaultAsync(m => m.DealID == id);
            if (deals == null)
            {
                return NotFound();
            }

            return View(deals);
        }

        // GET: Deals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Deals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DealID,PercentageDiscount,Description")] Deal deals)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deals);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Index), await _context.Deal.ToListAsync());
        }

        // GET: Deals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deals = await _context.Deal.SingleOrDefaultAsync(m => m.DealID == id);
            if (deals == null)
            {
                return NotFound();
            }
            return View(deals);
        }

        // POST: Deals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("DealID,PercentageDiscount,Description")] Deal deals)
        {
            if (deals == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deals);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DealsExists(deals.DealID))
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
            return View(nameof(Index));
        }

        // GET: Deals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            var deals = await _context.Deal
                .SingleOrDefaultAsync(m => m.DealID == id);
            if (deals == null)
            {
                return NotFound();
            }

            return View(nameof(Index));
        }

        // POST: Deals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Product.Select(p=>p).Where(p => p.DealID == id).ToListAsync();
            var deals = await _context.Deal.SingleOrDefaultAsync(m => m.DealID == id);
            if (deals == null)
            {
                return NotFound();
            }
            _context.Deal.Remove(deals);
            foreach (Product p in products)
            {
                p.DealID = null;
                p.Deal = null;
                _context.Product.Update(p);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DealsExists(int id)
        {
            return _context.Deal.Any(e => e.DealID == id);
        }
    }
}
