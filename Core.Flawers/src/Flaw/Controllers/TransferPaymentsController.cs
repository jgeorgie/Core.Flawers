using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Flaw.Data;
using Flaw.Models;
using Microsoft.AspNetCore.Authorization;

namespace Flaw.Controllers
{
    [Authorize]
    public class TransferPaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransferPaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TransferPayments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TransferPayments.Include(t => t.Fee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TransferPayments/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferPayment = await _context.TransferPayments.SingleOrDefaultAsync(m => m.Id == id);
            if (transferPayment == null)
            {
                return NotFound();
            }

            return View(transferPayment);
        }

        // GET: TransferPayments/Create
        public IActionResult Create()
        {
            ViewData["MembershipFeeId"] = new SelectList(_context.MembershipFees, "Id", "Id");
            return View();
        }

        // POST: TransferPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,Date,PaymentNo,Destination,FullName,MembershipFeeId")] TransferPayment transferPayment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transferPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["MembershipFeeId"] = new SelectList(_context.MembershipFees, "Id", "Id", transferPayment.MembershipFeeId);
            return View(transferPayment);
        }

        public IActionResult CreateForFee(string feeId)
        {
            return PartialView("_CreateTransferForFee");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForFee([Bind("Amount,Date,PaymentNo,Destination,FullName,MembershipFeeId")] TransferPayment transferPayment)
        {
            if (ModelState.IsValid)
            {
                var fee = await _context.MembershipFees.SingleOrDefaultAsync(f => f.Id == transferPayment.MembershipFeeId);
                var payments = await _context.Payments.Where(p => p.MembershipFeeForeignKey == fee.Id && p.Status == PaymentStatus.Pending).OrderBy(p => p.PaymentDeadline).ToListAsync();
                double amount = transferPayment.Amount;

                if (amount > 0)
                {
                    for (int i = 0; i < payments.Count; i++)
                    {
                        double depOrDebt = payments[i].DepositOrDebt == null ? 0 : (double)payments[i].DepositOrDebt;
                        if (depOrDebt <= 0)
                        {
                            if (amount >= payments[i].Amount)
                            {
                                payments[i].Status = PaymentStatus.Payed;
                                payments[i].TransferPaymentForeignKey = transferPayment.Id;
                                payments[i].PayedOn = transferPayment.Date;
                                payments[i].DepositOrDebt = 0;


                                _context.Update(payments[i]);

                                amount -= payments[i].Amount;

                                if (amount == 0)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                if (payments[i].DepositOrDebt == null)
                                {
                                    payments[i].DepositOrDebt = amount;
                                }
                                else
                                {
                                    payments[i].DepositOrDebt += amount;
                                }
                                _context.Update(payments[i]);
                                break;
                            }
                        }
                        else
                        {
                            if (amount >= payments[i].Amount - depOrDebt)
                            {
                                payments[i].Status = PaymentStatus.Payed;
                                payments[i].TransferPaymentForeignKey = transferPayment.Id;
                                payments[i].PayedOn = transferPayment.Date;

                                amount -= payments[i].Amount - (double)payments[i].DepositOrDebt;
                                payments[i].DepositOrDebt = 0;

                                _context.Update(payments[i]);

                                

                                if (amount == 0)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                payments[i].DepositOrDebt += amount;

                                _context.Update(payments[i]);
                                break;
                            }
                        }
                    }
                }

                fee.LeftOver -= transferPayment.Amount;
                _context.Update(fee);
                transferPayment.Id = Guid.NewGuid().ToString();
                _context.Add(transferPayment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","MembershipFees");
            }
            ViewData["MembershipFeeId"] = new SelectList(_context.MembershipFees, "Id", "Id", transferPayment.MembershipFeeId);
            return PartialView("_CreateTransferForFee",transferPayment);
        }


        // GET: TransferPayments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferPayment = await _context.TransferPayments.SingleOrDefaultAsync(m => m.Id == id);
            if (transferPayment == null)
            {
                return NotFound();
            }
            ViewData["MembershipFeeId"] = new SelectList(_context.MembershipFees, "Id", "Id", transferPayment.MembershipFeeId);
            return View(transferPayment);
        }

        // POST: TransferPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Amount,Date,PaymentNo,Destination,FullName,MembershipFeeId")] TransferPayment transferPayment)
        {
            if (id != transferPayment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transferPayment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransferPaymentExists(transferPayment.Id))
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
            ViewData["MembershipFeeId"] = new SelectList(_context.MembershipFees, "Id", "Id", transferPayment.MembershipFeeId);
            return View(transferPayment);
        }

        // GET: TransferPayments/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transferPayment = await _context.TransferPayments.SingleOrDefaultAsync(m => m.Id == id);
            if (transferPayment == null)
            {
                return NotFound();
            }

            return View(transferPayment);
        }

        // POST: TransferPayments/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var transferPayment = await _context.TransferPayments.SingleOrDefaultAsync(m => m.Id == id);
            _context.TransferPayments.Remove(transferPayment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool TransferPaymentExists(string id)
        {
            return _context.TransferPayments.Any(e => e.Id == id);
        }
    }
}
