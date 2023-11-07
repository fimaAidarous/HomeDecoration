using HomeDecoration.Models.Domain;
using HomeDecoration.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HomeDecoration.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private readonly DatabaseContext _context;

        public InvoiceController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Invoices

        public async Task<IActionResult> Index()
        {
            return _context.Invoices != null
                ? View(await _context.Invoices.ToListAsync())
                : Problem("Entity set 'DatabaseContext.invoices'  is null.");
        }

        // GET: invoice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FirstOrDefaultAsync(
                m => m.Id == id
            );
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: invoice/Create

        //public IActionResult Create()
        //{
        //    var model = new Invoice();
        //    var order = _context.Orders.ToList();
        //    Console.WriteLine(order);

        //    Replace `_context.Order.ToList()` with your logic to retrieve the Order list

        //    var ordertList = order
        //        .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a. })
        //        .ToList();
        //    ViewBag.Order = ordertList;

        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(
        //    [Bind("Id,OrderId,Total_Amount,Payment_Status")]
        //    Invoice invoice
        //)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        invoice.CreatedAt = DateTime.Now;
        //        _context.Add(invoice);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    return View(invoice);
        //}

        // GET: invoice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.InvoiceId == id);
            invoice.Payment = payment;
            return View(invoice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,OrderId,Total_Amount,Payment_Status")]
            Invoice invoice
        )
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (invoice.Payment != null)
                    {
                        _context.Update(invoice.Payment);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!invoiceExists(invoice.Id))
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
            return View(invoice);
        }

        // GET: invoice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Invoices == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Invoices == null)
            {
                return Problem("Entity set 'DatabaseContext.Invoices'  is null.");
            }
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool invoiceExists(int id)
        {
            return (_context.Invoices?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public ActionResult InvoiceIndex()
        {
            var model = _context.Invoices.Include("Order").ToList();
            return View(model);
        }

        // GET: invoice/Print/5
        public IActionResult Print(int id)
        {
            var invoices = _context.Invoices.FirstOrDefault(m => m.Id == id);
            return PartialView("_Print", invoices);
        }
    }
}
