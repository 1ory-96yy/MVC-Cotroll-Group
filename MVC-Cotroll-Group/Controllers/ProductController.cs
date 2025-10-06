using Microsoft.AspNetCore.Mvc;
using MVC_Cotroll_Group.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MVC_Cotroll_Group.Data;


namespace MVC_Cotroll_Group.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context) => _context = context;


        public IActionResult Index(int? categoryId, string sortOrder)
        {
            ViewBag.CategoryId = categoryId;
            ViewBag.SortOrder = sortOrder;

            var products = _context.Products.AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            products = sortOrder switch
            {
                "price_asc" => products.OrderBy(p => p.Price),
                "price_desc" => products.OrderByDescending(p => p.Price),
                "rating_asc" => products.OrderBy(p => p.Rating),
                "rating_desc" => products.OrderByDescending(p => p.Rating),
                "name_asc" => products.OrderBy(p => p.Name),
                "name_desc" => products.OrderByDescending(p => p.Name),
                _ => products.OrderBy(p => p.Id)
            };

            return View(products.ToList());
        }


        public IActionResult Create(int categoryId)
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", categoryId);
            return View(new Product { CategoryId = categoryId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { categoryId = product.CategoryId });
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }


        public IActionResult Details(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }



        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index), new { categoryId = product.CategoryId });
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }



        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index), new { categoryId = product?.CategoryId });
        }


    }
}
