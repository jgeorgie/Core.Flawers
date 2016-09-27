using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Flaw.Data;
using Flaw.Models;

namespace Flaw.Controllers
{
    public class PaymentModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PaymentModels
        public async Task<IActionResult> Index(string feeId)
        {
            var payments = await _context.PaymentModel.Where(p => p.MembershipFeeForeignKey == feeId).ToListAsync();
            //var applicationDbContext = _context.PaymentModel.Include(p => p.Fee);
            //return View(await applicationDbContext.ToListAsync());
            return View(payments);
        }

        // GET: PaymentModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentModel = await _context.PaymentModel.SingleOrDefaultAsync(m => m.Id == id);
            if (paymentModel == null)
            {
                return NotFound();
            }

            return View(paymentModel);
        }

        // GET: PaymentModels/Create
        public IActionResult Create()
        {
            ViewData["MembershipFeeForeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id");
            return View();
        }

        // POST: PaymentModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,Date,MembershipFeeForeignKey,Type")] PaymentModel paymentModel)
        {
            if (ModelState.IsValid)
            {
                paymentModel.Id = Guid.NewGuid().ToString();
                _context.Add(paymentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["MembershipFeeForeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id", paymentModel.MembershipFeeForeignKey);
            return View(paymentModel);
        }

        // GET: PaymentModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentModel = await _context.PaymentModel.SingleOrDefaultAsync(m => m.Id == id);
            if (paymentModel == null)
            {
                return NotFound();
            }
            ViewData["MembershipFeeForeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id", paymentModel.MembershipFeeForeignKey);
            return View(paymentModel);
        }

        // POST: PaymentModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Amount,Date,MembershipFeeForeignKey,Type")] PaymentModel paymentModel)
        {
            if (id != paymentModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentModelExists(paymentModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["MembershipFeeForeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id", paymentModel.MembershipFeeForeignKey);
            return View(paymentModel);
        }

        // GET: PaymentModels/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentModel = await _context.PaymentModel.SingleOrDefaultAsync(m => m.Id == id);
            if (paymentModel == null)
            {
                return NotFound();
            }

            return View(paymentModel);
        }

        // POST: PaymentModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var paymentModel = await _context.PaymentModel.SingleOrDefaultAsync(m => m.Id == id);
            _context.PaymentModel.Remove(paymentModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PaymentModelExists(string id)
        {
            return _context.PaymentModel.Any(e => e.Id == id);
        }
    }
}
