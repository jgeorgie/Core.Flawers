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
    public class PendingPaymentModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PendingPaymentModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PendingPaymentModels
        public async Task<IActionResult> Index(string id)
        {
            var applicationDbContext = _context.PendingPayments.Include(p => p.Fee).Where(p => p.MembershipFeeForeignKey == id);
            return View(await applicationDbContext.OrderBy(p => p.PaymentDeadline).ToListAsync());
        }

        // GET: PendingPaymentModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pendingPaymentModel = await _context.PendingPayments.SingleOrDefaultAsync(m => m.Id == id);
            if (pendingPaymentModel == null)
            {
                return NotFound();
            }

            return View(pendingPaymentModel);
        }

        public async Task<IActionResult> GetPayments(string id)
        {
            var list = _context.Payments.Where(p => p.MembershipFeeForeignKey == id).OrderBy(p => p.Date);
            return PartialView("_PayedPayments", await list.ToListAsync());
        }


        // GET: PendingPaymentModels/Create
        public IActionResult Create()
        {
            ViewData["MembershipFeeForeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id");
            return View();
        }

        // POST: PendingPaymentModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,MembershipFeeForeignKey,PaymentDeadline,Status")] PendingPaymentModel pendingPaymentModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pendingPaymentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["MembershipFeeForeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id", pendingPaymentModel.MembershipFeeForeignKey);
            return View(pendingPaymentModel);
        }

        // GET: PendingPaymentModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pendingPaymentModel = await _context.PendingPayments.SingleOrDefaultAsync(m => m.Id == id);
            if (pendingPaymentModel == null)
            {
                return NotFound();
            }
            ViewData["MembershipFeeForeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id", pendingPaymentModel.MembershipFeeForeignKey);
            return View(pendingPaymentModel);
        }

        // POST: PendingPaymentModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Amount,MembershipFeeForeignKey,PaymentDeadline,Status")] PendingPaymentModel pendingPaymentModel)
        {
            if (id != pendingPaymentModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pendingPaymentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PendingPaymentModelExists(pendingPaymentModel.Id))
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
            ViewData["MembershipFeeForeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id", pendingPaymentModel.MembershipFeeForeignKey);
            return View(pendingPaymentModel);
        }

        // GET: PendingPaymentModels/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pendingPaymentModel = await _context.PendingPayments.SingleOrDefaultAsync(m => m.Id == id);
            if (pendingPaymentModel == null)
            {
                return NotFound();
            }

            return View(pendingPaymentModel);
        }

        // POST: PendingPaymentModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var pendingPaymentModel = await _context.PendingPayments.SingleOrDefaultAsync(m => m.Id == id);
            _context.PendingPayments.Remove(pendingPaymentModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PendingPaymentModelExists(string id)
        {
            return _context.PendingPayments.Any(e => e.Id == id);
        }
    }
}
