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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text;
using Accord.MachineLearning.Rules;
using GameStoreMid.Services;
using unirest_net.http;
using Newtonsoft.Json;

namespace GameStoreMid.Controllers
{
    
    public class ProductsController : Controller
    {
        
        
        private readonly ApplicationDbContext _context;
        private HashSet<int> m_markedTags;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly MLApriori _mlApriori;

        public ProductsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, MLApriori mlapriori)
        {
            
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            m_markedTags = new HashSet<int>();
            _mlApriori = mlapriori;

        }

        // GET: Products
        public IActionResult Index(string sortOrder, string searchString, string currentFilter, HashSet<int> prices, DateTime from, DateTime until, HashSet<int> tags)
        {
            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.CostSortParm = sortOrder == "Cost" ? "Cost_desc" : "Cost";
            ViewBag.QuantitySortParm = sortOrder == "Quantity" ? "Quantity_desc" : "Quantity";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "Description_desc" : "Description";
            ViewBag.DiscountSortParm = sortOrder == "Discount" ? "Discount_desc" : "Discount";
            ViewBag.CurrentFilter = searchString;
            ViewBag.AllTags = _context.Tag.ToList();
            ViewBag.From = from;
            ViewBag.Until = until;
            ViewBag.Prices = prices;
            ViewBag.Tags = tags;

            var products = _context.Product.AsQueryable();
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName.Contains(searchString) || s.ProductDescription.Contains(searchString));
            }
            //Filter Dates
            if(from != DateTime.MinValue)
            {
                products = products.Where(p => p.ReleaseDate >= from);
            }
            if(until != DateTime.MinValue)
            {
                products = products.Where(p => p.ReleaseDate <= until);
            }
            //Filter tags
            if (tags.Count > 0)
            {
                products = products.Include(p=>p.ProductTags).Where(delegate (Product p) 
                {
                    List<ProductTag> productTags = p.ProductTags.ToList();
                    HashSet<int> pt = productTags.Select(x => x.TagID).ToHashSet();
                    /*foreach (ProductTag pt in productTags) // returns the products where one of its tags is in the tags chosen
                    {
                        if (tags.Contains(pt.TagID))
                            return true;
                    }
                    return false;*/

                    foreach (int tag in tags) // returns true if all of the tags in the product appear in the tags chosen list
                    {
                        if (!pt.Contains(tag))
                            return false;
                    }
                    return true;

                }).AsQueryable();
            }

            if(prices.Count > 0)
            {
                products = products.Where(delegate (Product p)
                {
                    foreach (int price in prices)
                    {
                        if (price == 1 && p.Cost <= 50) // special case
                        {
                            return true;
                        }
                        else if (price == 251 && p.Cost >= 251)
                        {
                            return true;
                        }
                        else if (p.Cost >= price && p.Cost <= price + 49) // if cose is between 51 to 100 for example
                        {
                            return true;
                        }
                    }
                    return false; // if non of the condition met, dont include this product
                }).AsQueryable();
            }

            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "Name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                case "Date":
                    products = products.OrderBy(p => p.ReleaseDate);
                    break;
                case "Date_desc":
                    products = products.OrderByDescending(p => p.ReleaseDate);
                    break;
                case "Cost":
                    products = products.OrderBy(p => p.Cost);
                    break;
                case "Cost_desc":
                    products = products.OrderByDescending(p => p.Cost);
                    break;
                case "Quantity":
                    products = products.OrderBy(p => p.TotalQuantity);
                    break;
                case "Quantity_desc":
                    products = products.OrderByDescending(p => p.TotalQuantity);
                    break;
                case "Description":
                    products = products.OrderBy(p => p.ProductDescription);
                    break;
                case "Description_desc":
                    products = products.OrderByDescending(p => p.ProductDescription);
                    break;
                case "Discount":
                    products = products.OrderBy(p => p.Deal.PercentageDiscount);
                    break;
                case "Discount_desc":
                    products = products.OrderByDescending(p => p.Deal.PercentageDiscount);
                    break;
                default:  // Name ascending 
                    products = products.OrderBy(s => s.ProductName);
                    break;
            }
            
            return View(products.Include(p=>p.Deal));
        }

        // GET: Products
        public IActionResult AdminIndex(string sortOrder, string searchString, string currentFilter, HashSet<int> prices, DateTime from, DateTime until, HashSet<int> tags)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewBag.CostSortParm = sortOrder == "Cost" ? "Cost_desc" : "Cost";
            ViewBag.QuantitySortParm = sortOrder == "Quantity" ? "Quantity_desc" : "Quantity";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "Description_desc" : "Description";
            ViewBag.DiscountSortParm = sortOrder == "Discount" ? "Discount_desc" : "Discount";
            ViewBag.CurrentFilter = searchString;
            ViewBag.AllTags = _context.Tag.ToList();
            ViewBag.From = from;
            ViewBag.Until = until;
            ViewBag.Prices = prices;
            ViewBag.Tags = tags;

            var products = _context.Product.AsQueryable();
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName.Contains(searchString) || s.ProductDescription.Contains(searchString));
            }
            //Filter Dates
            if (from != DateTime.MinValue)
            {
                products = products.Where(p => p.ReleaseDate >= from);
            }
            if (until != DateTime.MinValue)
            {
                products = products.Where(p => p.ReleaseDate <= until);
            }
            //Filter tags
            if (tags.Count > 0)
            {
                products = products.Include(p => p.ProductTags).Where(delegate (Product p) // returns the products where one of its tags is in the tags chosen
                {
                    List<ProductTag> productTags = p.ProductTags.ToList();
                    foreach (ProductTag pt in productTags)
                    {
                        if (tags.Contains(pt.TagID))
                            return true;
                    }
                    return false;
                }).AsQueryable();
            }

            if (prices.Count > 0)
            {
                products = products.Where(delegate (Product p)
                {
                    foreach (int price in prices)
                    {
                        if (price == 1 && p.Cost <= 50) // special case
                        {
                            return true;
                        }
                        else if (price == 251 && p.Cost >= 251)
                        {
                            return true;
                        }
                        else if (p.Cost >= price && p.Cost <= price + 49) // if cose is between 51 to 100 for example
                        {
                            return true;
                        }
                    }
                    return false; // if non of the condition met, dont include this product
                }).AsQueryable();
            }

            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {
                case "Name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                case "Date":
                    products = products.OrderBy(p => p.ReleaseDate);
                    break;
                case "Date_desc":
                    products = products.OrderByDescending(p => p.ReleaseDate);
                    break;
                case "Cost":
                    products = products.OrderBy(p => p.Cost);
                    break;
                case "Cost_desc":
                    products = products.OrderByDescending(p => p.Cost);
                    break;
                case "Quantity":
                    products = products.OrderBy(p => p.TotalQuantity);
                    break;
                case "Quantity_desc":
                    products = products.OrderByDescending(p => p.TotalQuantity);
                    break;
                case "Description":
                    products = products.OrderBy(p => p.ProductDescription);
                    break;
                case "Description_desc":
                    products = products.OrderByDescending(p => p.ProductDescription);
                    break;
                case "Discount":
                    products = products.OrderBy(p => p.Deal.PercentageDiscount);
                    break;
                case "Discount_desc":
                    products = products.OrderByDescending(p => p.Deal.PercentageDiscount);
                    break;
                default:  // Name ascending 
                    products = products.OrderBy(s => s.ProductName);
                    break;
            }

            return View(products.Include(p => p.Deal));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Product.Include(x => x.Reviews).Include(x => x.Deal)
               .SingleOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            //Users that reviewed the product
            var users = _context.Review
                .Join(_context.ApplicationUser, x => x.ApplicationUserID, y => y.Id, (x, y) => new { ApplicationUser = y })
                .Select(x => x.ApplicationUser);

            var users2 = _context.Review.Select(x => x).Where(x => x.ProductID == id).Include(x => x.ApplicationUser);

            ViewBag.ApplicationUsers = users.ToList();
           

            ViewBag.SimilarItems = _mlApriori.GetRecommendedProducts(product.ProductID);
            if (product.ImageUrl.Contains("igdb"))
            {
                ViewBag.Videos = GetVideos(product.ProductID);
            }
            ViewData["appid"] = Startup.SettingFactory()["Authentication:Facebook:AppId"];


            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewBag.Tag = _context.Tag.ToList();
            PopulateDealsDropDownList();
            ViewBag.MarkedTag = new HashSet<int>();
            return View();
        }

        private class Root
        {
            public Video[] videos { get; set; }
        }
        private class Video
        {
            public string video_id { get; set; }

        }


        private List<string> GetVideos(int prodId)
        {
            string endpoint = Startup.SettingFactory()["IGDB:endpoint"];
            string userkey = Startup.SettingFactory()["IGDB:user-key"];
            List<string> retVal = new List<string>();

            string url = "https://api-" + endpoint + ".apicast.io/games/";
            Task<HttpResponse<string>> jsonResponse = Unirest.get(url + prodId +"?fields=videos")
                .header("user-key", userkey)
                .header("Accept", "application/json")
                .asJsonAsync<string>();
            var x = jsonResponse.Result.Body;
            var model = JsonConvert.DeserializeObject<List<Root>>(jsonResponse.Result.Body);
            if (model[0].videos != null && model[0].videos.Count() > 0)
            {
                retVal = model[0].videos.AsQueryable().Select(video => "https://www.youtube.com/embed/" + video.video_id).ToList();
            }
            return retVal;
        }

        private void PopulateDealsDropDownList(object selectedDeal = null)
        {
            var dealsQuery = from d in _context.Deal
                             select d;
            ViewBag.DealsID = new SelectList(dealsQuery, "DealID", "DescriptionDiscount", selectedDeal);
        }
        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("ProductID,ProductName,Cost,TotalQuantity,ProductDescription,ReleaseDate,DealID,ImageUrl")] Product product, string[] tags, List<IFormFile> files)
        {
            
            if (ModelState.IsValid)
            {
                CreateAndAddImages(product, files);
                _context.Add(product);
                foreach (string tag in tags)
                {
                    ProductTag temp = new ProductTag();
                    temp.TagID = Convert.ToInt32(tag);
                    temp.ProductID = product.ProductID;
                    _context.ProductTag.Add(temp);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AdminIndex));
            }
            return View(product);
        }

        private void CreateAndAddImages(Product product, List<IFormFile> files)
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                path = Path.Combine(path, "img");
                Directory.CreateDirectory(path);
            }
            string uploadPath = "uploads/img/";
            StringBuilder sb = new StringBuilder();

            foreach (var file in files)
            {

                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                    var uploadPathWithfileName = Path.Combine(uploadPath, fileName);

                    var uploadAbsolutePath = Path.Combine(_hostingEnvironment.WebRootPath, uploadPathWithfileName);

                    using (var fileStream = new FileStream(uploadAbsolutePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        sb.Append(fileName + ",");
                    }
                }
            }
            if (product.ImageUrl == null)
            {
                product.ImageUrl = "";
            }
            product.ImageUrl += sb.ToString();
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Tag = _context.Tag.ToList();
            var markedTags = _context.ProductTag.Select(s => s).Where(pt => pt.ProductID == id).Select(s => s.TagID).ToHashSet();
            var product = await _context.Product.SingleOrDefaultAsync(m => m.ProductID == id);
            ViewBag.MarkedTag = markedTags;
            m_markedTags = markedTags;
            List<string> images = new List<string>();
            if (product == null)
            {
                return NotFound();
            }
            if (product.ImageUrl != null)
            {
                images = product.ImageUrl.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            ViewBag.Images = images;
            
            PopulateDealsDropDownList(product.Deal);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,Cost,TotalQuantity,ProductDescription,ReleaseDate,DealID,ImageUrl")] Product product, string[] tags, List<IFormFile> files, string pathsToDelete)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    HashSet<int> nowMarked = new HashSet<int>();
                    foreach (string s in tags)
                    {
                        nowMarked.Add(Convert.ToInt32(s));
                    }
                    DeleteImages(product, pathsToDelete);
                    CreateAndAddImages(product, files);
                    _context.Update(product);
                    _context.ProductTag.RemoveRange(_context.ProductTag.Select(pt => pt).Where(pt => pt.ProductID == id)); // delete all the product tags
                    //add the tags that are now marked
                    _context.SaveChanges();
                    foreach (int tag in nowMarked)
                    {
                        ProductTag temp = new ProductTag();
                        temp.TagID = tag;
                        temp.ProductID = product.ProductID;
                        temp.Product = product;
                        _context.ProductTag.Add(temp);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdminIndex));
            }
            return View(product);
        }

        private void DeleteImages(Product product, string pathsToDelete)
        {
            string[] paths;
            HashSet<string> currentPaths;
            StringBuilder photosToKeep = new StringBuilder();
            if (pathsToDelete != null)
            {
                paths = pathsToDelete.Split(",", StringSplitOptions.RemoveEmptyEntries);
                currentPaths = product.ImageUrl.Split(",", StringSplitOptions.RemoveEmptyEntries).ToHashSet();

                foreach (string path in paths)
                {
                    if (currentPaths.Contains(path))
                    {
                        DeleteImage(path);
                        currentPaths.Remove(path);
                    }
                }
                foreach (string photos in currentPaths)
                {
                    photosToKeep.Append(photos + ",");
                }
                product.ImageUrl = photosToKeep.ToString();
            }
        }

        private void DeleteImage(string photoName)
        {
            string fullPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            fullPath = Path.Combine(fullPath, "img");
            fullPath = Path.Combine(fullPath, photoName);
            if (!fullPath.Contains("igdb")&& System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Deal)
                .SingleOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.SingleOrDefaultAsync(m => m.ProductID == id);
            if(product == null)
            {
                return NotFound();
            }

            DeleteImages(product, product.ImageUrl);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminIndex));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }
    }
}
