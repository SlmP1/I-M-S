using IMS.Data;
using IMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IMS.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Injecting ApplicationDbContext to interact with the database
        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Inventory
        public async Task<IActionResult> Index()
        {
            var inventories = await _context.Inventories
           .Include(i => i.Product)  
           .ToListAsync();
            return View(inventories);
        }

        public IActionResult Create()
        {
            // Fetch the products from the database
            var products = _context.Products
                .Select(p => new { p.ProductId, p.ProductName })
                .ToList();

            // Populate the dropdown with product names and IDs
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                _context.Inventories.Add(inventory);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Re-populate the dropdown in case of validation errors
            var products = _context.Products
                .Select(p => new { p.ProductId, p.ProductName })
                .ToList();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName");

            return View(inventory);
        }


        // GET: Inventory/Update/5
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = _context.Inventories.Find(id);
            if (inventory == null)
            {
                return NotFound();
            }

            // Populate the dropdown with product names and IDs
            var products = _context.Products
                .Select(p => new { p.ProductId, p.ProductName })
                .ToList();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", inventory.ProductId);

            return View(inventory);
        }

        // POST: Inventory/Update/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Inventory inventory)
        {
            if (id != inventory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventory);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Inventories.Any(e => e.Id == inventory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Re-populate the dropdown in case of validation errors
            var products = _context.Products
                .Select(p => new { p.ProductId, p.ProductName })
                .ToList();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", inventory.ProductId);

            return View(inventory);
        }


        // GET: Inventory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           
            var inventory = await _context.Inventories.FindAsync(id);
            
            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Inventory");
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.Id == id);
        }
    }
}
