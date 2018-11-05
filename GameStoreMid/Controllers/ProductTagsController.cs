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
    public class ProductTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProductTags
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductTag.ToListAsync());
        }

        // GET: ProductTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTag = await _context.ProductTag
                .SingleOrDefaultAsync(m => m.ProductID == id);
            if (productTag == null)
            {
                return NotFound();
            }

            return View(productTag);
        }

        // GET: ProductTags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductTags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,TagID")] ProductTag productTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productTag);
        }

        // GET: ProductTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTag = await _context.ProductTag.SingleOrDefaultAsync(m => m.ProductID == id);
            if (productTag == null)
            {
                return NotFound();
            }
            return View(productTag);
        }

        // POST: ProductTags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,TagID")] ProductTag productTag)
        {
            if (id != productTag.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductTagExists(productTag.ProductID))
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
            return View(productTag);
        }

        // GET: ProductTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productTag = await _context.ProductTag
                .SingleOrDefaultAsync(m => m.ProductID == id);
            if (productTag == null)
            {
                return NotFound();
            }

            return View(productTag);
        }

        // POST: ProductTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productTag = await _context.ProductTag.SingleOrDefaultAsync(m => m.ProductID == id);
            _context.ProductTag.Remove(productTag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductTagExists(int id)
        {
            return _context.ProductTag.Any(e => e.ProductID == id);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        //return json with 10 most used tags
        public JsonResult getJson10MostUsedTags()
        {
            var bh = _context.ProductTag.GroupBy(x => x.TagID);
            List<TagCount> mylist = new List<TagCount>();
            foreach (var x in bh)
            {
                var myjson = new TagCount { Name = _context.Tag.Where(t=>t.TagID== x.Key).FirstOrDefault().Name, Count = x.Count() };
                mylist.Add(myjson);
            }
            var list = mylist.OrderBy(x => x.Count).Take(10);
            return Json(list);
        }
    }
    public class TagCount
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
