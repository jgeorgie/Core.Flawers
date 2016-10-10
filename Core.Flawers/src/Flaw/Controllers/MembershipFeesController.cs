using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Flaw.Data;
using Flaw.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Flaw.Controllers
{
    [Authorize]
    public class MembershipFeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembershipFeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MembershipFees
        public async Task<IActionResult> Index()
        {
            return View(await _context.MembershipFees.ToListAsync());
        }

        public IActionResult AddPayment(string id)
        {
            return View();
        }


        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AmountChangeModels(string id)
        {
            var changes = await _context.FeeAmountChangeModels.Where(c => c.MembershipFeeForeignKey == id).ToListAsync();
            return View(changes);
        }

        // GET: MembershipFees/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipFee = await _context.MembershipFees.SingleOrDefaultAsync(m => m.Id == id);

            if (membershipFee == null)
            {
                return NotFound();
            }

            return View(membershipFee);
        }

        public IActionResult CountDebt(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipFee = _context.MembershipFees.SingleOrDefault(f => f.Id == id);

            if (membershipFee.CurrentState != FeeState.Pause && membershipFee.CurrentState != FeeState.Finish)
            {
                var priveleges = _context.Privileges.Where(p => p.MembershipFeeForeignKey == membershipFee.Id && p.Start <= DateTime.Now.Date).ToList();
                double dept = 0;
                double totalDays = (membershipFee.End - membershipFee.Start).TotalDays;
                double currentDays = (DateTime.Now.Date - membershipFee.Start).TotalDays + 1;
                double daysLeft = currentDays;

                foreach (var privelege in priveleges)
                {
                    double privelegeDays = privelege.End <= DateTime.Now
                        ? (privelege.End - privelege.Start).TotalDays : (DateTime.Now.Date - privelege.Start).TotalDays + 1;
                    double amountWithPriv = membershipFee.RealAmount - (membershipFee.RealAmount * privelege.Discount) / 100;
                    dept += privelegeDays * (amountWithPriv / totalDays);
                    daysLeft -= privelegeDays;
                }
                dept += daysLeft * (membershipFee.RealAmount / totalDays);
                membershipFee.currentDebt = dept;
                try
                {
                    _context.Update(membershipFee);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipFeeExists(membershipFee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return Json(membershipFee.currentDebt);
        }

        // GET: MembershipFees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MembershipFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AmountWithDiscount,CurrentState,FirstName,LastName,MiddleName,RealAmount,Start,LeftOver,Periodicity,Deposit")] MembershipFee membershipFee)
        {
            if (ModelState.IsValid)
            {
                membershipFee.Id = Guid.NewGuid().ToString();
                membershipFee.AmountWithDiscount = membershipFee.RealAmount;
                membershipFee.LeftOver = membershipFee.AmountWithDiscount;
                membershipFee.MonthlyPay = Math.Floor(membershipFee.RealAmount / 12);

                if (membershipFee.Deposit != null)
                {
                    membershipFee.LeftOver -= (double)membershipFee.Deposit;
                }

                if (membershipFee.Periodicity == FeePeriodicity.Year)
                {
                    membershipFee.End = membershipFee.Start.AddMonths(12);
                    int month = membershipFee.Start.Month;
                    for (int i = 1; i <= 12; i++)
                    {
                        var payment = new PendingPaymentModel()
                        {
                            MembershipFeeForeignKey = membershipFee.Id,
                            Amount = Math.Floor(membershipFee.RealAmount / 12),
                            Id = Guid.NewGuid().ToString(),
                            PaymentDeadline = month + i <= 12 ? new DateTime(DateTime.Now.Year, month + i, 15) : new DateTime(DateTime.Now.Year + 1, month + i - 12, 15),
                            Status = PaymentStatus.Pending
                        };

                        _context.Payments.Add(payment);
                    }
                }

                _context.Add(membershipFee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(membershipFee);
        }

        public async Task<IActionResult> GetPendingPayments(string id)
        {
            var payments = await _context.Payments.Where(p => p.MembershipFeeForeignKey == id).OrderBy(p => p.PaymentDeadline).ToListAsync();
            if (payments.Count != 0)
            {
                return PartialView("_PendingPayments", payments);
            }
            return BadRequest();
        }

        public async Task<IActionResult> GetTransferPayments(string id)
        {
            var transfer = await _context.TransferPayments.Where(t => t.MembershipFeeId == id).ToListAsync();

            if (transfer.Count != 0)
            {
                return PartialView("_Transfers", transfer);
            }
            return BadRequest();
        }

        public async Task<IActionResult> GetCashPayments(string id)
        {
            var transfer = await _context.CashModel.Where(c => c.MembershipFeeId == id).ToListAsync();

            if (transfer.Count != 0)
            {
                return PartialView("_CashPayments", transfer);
            }

            return BadRequest();
        }


        // GET: MembershipFees/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipFee = await _context.MembershipFees.SingleOrDefaultAsync(m => m.Id == id);

            if (membershipFee == null)
            {
                return NotFound();
            }
            return View(membershipFee);
        }

        // POST: MembershipFees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,AmountWithDiscount,CurrentState,End,FullName,RealAmount,Start,LeftOver,Periodicity")] MembershipFee membershipFee)
        {
            if (id != membershipFee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var previousModel = _context.MembershipFees.Where(m => m.Id == membershipFee.Id).AsNoTracking().SingleOrDefault();
                if (previousModel.RealAmount != membershipFee.RealAmount)
                {
                    if (HttpContext.User.Identity.Name != "admin@admin.am")
                    {
                        membershipFee.RealAmount = previousModel.RealAmount;
                        membershipFee.AmountWithDiscount = previousModel.AmountWithDiscount;
                    }
                    else
                    {
                        var privelege = _context.Privileges.Where(p => p.MembershipFeeForeignKey == membershipFee.Id).SingleOrDefault();
                        if (privelege == null)
                        {
                            membershipFee.RealAmount = membershipFee.AmountWithDiscount;
                        }
                        else
                        {
                            var fullDays = (membershipFee.End - membershipFee.Start).TotalDays;
                            var discountDays = (privelege.End - privelege.Start).TotalDays;
                            double PriceWithDiscount = membershipFee.RealAmount - (membershipFee.RealAmount * privelege.Discount / 100);
                            membershipFee.AmountWithDiscount = ((fullDays - discountDays) * (membershipFee.RealAmount / fullDays)) + (discountDays * PriceWithDiscount / fullDays);

                            membershipFee.LeftOver = membershipFee.AmountWithDiscount;
                            var transfers = await _context.TransferPayments.Where(t => t.MembershipFeeId == privelege.MembershipFeeForeignKey).ToListAsync();
                            var cashs = await _context.CashModel.Where(c => c.MembershipFeeId == privelege.MembershipFeeForeignKey).ToListAsync();

                            foreach (var t in transfers)
                            {
                                membershipFee.LeftOver -= t.Amount;
                            }
                            foreach (var c in cashs)
                            {
                                membershipFee.LeftOver -= c.Amount;
                            }
                            //_context.Update(membershipFee);
                        }

                        var feeAmountChange = new FeeAmountChangeModel()
                        {
                            id = Guid.NewGuid().ToString(),
                            ChangeDate = DateTime.Now.Date,
                            MembershipFeeForeignKey = membershipFee.Id,
                            NewAmount = membershipFee.RealAmount,
                            OldAmount = previousModel.RealAmount
                        };
                        _context.Add(feeAmountChange);
                    }
                }


                if (membershipFee.CurrentState != previousModel.CurrentState)
                {
                    if (membershipFee.CurrentState == FeeState.Pause)
                    {
                        membershipFee.Paused = DateTime.Now;
                        double howManyDays = (membershipFee.Paused.Value - membershipFee.Start).TotalDays;
                        double fullDays = (membershipFee.End - membershipFee.Start).TotalDays;
                        membershipFee.currentDebt = (membershipFee.AmountWithDiscount / fullDays) * howManyDays;
                    }
                    else if (previousModel.CurrentState == FeeState.Pause && membershipFee.CurrentState == FeeState.Active)
                    {
                        membershipFee.Reactiveted = DateTime.Now;
                    }
                }


                try
                {
                    _context.Update(membershipFee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembershipFeeExists(membershipFee.Id))
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
            return View(membershipFee);
        }


        // GET: MembershipFees/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membershipFee = await _context.MembershipFees.SingleOrDefaultAsync(m => m.Id == id);
            if (membershipFee == null)
            {
                return NotFound();
            }

            return View(membershipFee);
        }

        // POST: MembershipFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var membershipFee = await _context.MembershipFees.SingleOrDefaultAsync(m => m.Id == id);
            _context.MembershipFees.Remove(membershipFee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool MembershipFeeExists(string id)
        {
            return _context.MembershipFees.Any(e => e.Id == id);
        }
    }
}