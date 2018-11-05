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
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Rate,Content,ProductID")] Review review)
        {
            review.ApplicationUser = _context.ApplicationUser.FirstOrDefault(x => x.UserName == User.Identity.Name);
            review.ApplicationUserID = review.ApplicationUser.Id;
            review.Product = _context.Product.FirstOrDefault(x => x.ProductID == review.ProductID);
            review.PostDate = DateTime.Now;
            ModelState.Remove("ApplicationUserID");
            ModelState.Remove("ProductID");
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return Redirect("~/Products/Details/"+review.ProductID);
            }
            return NotFound();
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ReviewID,Rate,Content,ProductID,ApplicationUserID")] Review review)
        {
            if (review == null)
            {
                return NotFound();
            }
            review.PostDate = DateTime.Now;
            ModelState.Remove("ApplicationUserID");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ReviewID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("~/Products/Details/" + review.ProductID);
            }
            return Redirect("~/Products/Details/" + review.ProductID);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Bind("ReviewID")] Review review)
        {
            int id = review.ReviewID;
            var reviewd = await _context.Review.SingleOrDefaultAsync(m => m.ReviewID == id);
            if(reviewd == null)
            {
                return NotFound();
            }
            _context.Review.Remove(reviewd);
            await _context.SaveChangesAsync();
            return Redirect("~/Products/Details/" + reviewd.ProductID);
        }

        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.ReviewID == id);
        }
    }
}
