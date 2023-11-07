using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeDecoration.Models.DTO;
using HomeDecoration.Models.Domain;
using static NuGet.Packaging.PackagingConstants;

namespace HomeDecoration.Controllers;

[Authorize]
public class OrderController : Controller
{
    private readonly DatabaseContext _context;

    public OrderController(DatabaseContext context)
    {
        _context = context;
    }

    // GET: Orders

    public async Task<IActionResult> Index()
    {
        return _context.Orders != null
            ? View(await _context.Orders.ToListAsync())
            : Problem("Entity set 'DatabaseContext.Orders'  is null.");
    }

    // GET: Orders/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Orders == null)
        {
            return NotFound();
        }

        var order = await _context.Orders.FirstOrDefaultAsync(
            m => m.Id == id
        );
        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // GET: Orders/Create

    public IActionResult Create()
    {
        var model = new Order();
        var product = _context.Products.ToList();
        var customer = _context.Customers.ToList();
        Console.WriteLine($"Product: {string.Join(", ", product)}\nCustomer: {string.Join(", ", customer)}");

        // Replace `_context.Orders.ToList()` with your logic to retrieve the Orders list

        var productList = product
           .Select (a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
            .ToList();
        ViewBag.Products = productList;

        var customerList = customer
         .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
          .ToList();
        ViewBag.Customers = customerList;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CustomerId,ProductId,Total_Amount,Quantity")] Order order)
    {
        if (ModelState.IsValid)
        {
            // Retrieve the selected product based on the provided ProductId
            var product = await _context.Products.FindAsync(order.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            // Set the Total_Amount of the order to the price of the selected product
            order.Total_Amount = product.Price * order.Quantity;

            _context.Add(order);
            await _context.SaveChangesAsync();

            var invoice = new Invoice()
            {
                OrderId = order.Id,
                Total_Amount = order.Total_Amount,
                CreatedAt = DateTime.Now,
                Payment_Status = "Not Paid"
            };
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        PopulateDropDownLists(order.ProductId, order.CustomerId);
        return View(order);
    }

    private void PopulateDropDownLists(int? selectedProductId = null, int? selectedCustomerId = null)
    {
        var products = _context.Products.ToList();
        var customers = _context.Customers.ToList();

        ViewBag.Products = new SelectList(products, "Id", "Name", selectedProductId);
        ViewBag.Customers = new SelectList(customers, "Id", "Name", selectedCustomerId);
    }



    // GET: Orders/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Orders == null)
        {
            return NotFound();
        }

        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit( int id,[Bind("Id,CustomerId,ProductId,Total_Amount,Quantity")] Order order)
    {
        if (id != order.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersExists(order.Id))
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
        return View(order);
    }

    // GET: Orders/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Orders == null)
        {
            return NotFound();
        }

        var order = await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);
        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }

    // POST: Orders/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Orders == null)
        {
            return Problem("Entity set 'DatabaseContext.Orders'  is null.");
        }
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OrdersExists(int id)
    {
        return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    public ActionResult OrderIndex()
    {
        var model = _context.Orders.Include(o => o.Product).Include(o => o.Customer).ToList();
        return View(model);
    }

    // GET: Orders/Print/5
    public IActionResult Print(int id)
    {
        var order = _context.Orders.FirstOrDefault(m => m.Id == id);
        return PartialView("_Print", order);
    }
}
