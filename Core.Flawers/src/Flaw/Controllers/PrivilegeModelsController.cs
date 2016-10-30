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
    public class PrivilegeModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrivilegeModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PrivilegeModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PrivilegeModels.Include(p => p.Fee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PrivilegeModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilegeModel = await _context.PrivilegeModels.SingleOrDefaultAsync(m => m.Id == id);
            if (privilegeModel == null)
            {
                return NotFound();
            }

            return View(privilegeModel);
        }

        // GET: PrivilegeModels/Create
        public IActionResult Create(string feeId)
        {
            //ViewData["MembershipFeeFoeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id");
            ViewBag.FeeId = feeId;
            var privileges = _context.Privileges.ToList();
            ViewBag.PrivilegeType = new SelectList(privileges, "Type", "Type");
            return View();
        }

        // POST: PrivilegeModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,End,PrivilegeNumber,MembershipFeeFoeignKey,Start,Type")] PrivilegeModel privilegeModel)
        {
            if (ModelState.IsValid && privilegeModel.Start > DateTime.Now && privilegeModel.End > privilegeModel.Start)
            {
                var privilige = _context.Privileges.FirstOrDefault(p => p.Type == privilegeModel.Type);
                var membershipFee =
                    await
                        _context.MembershipFees.SingleOrDefaultAsync(f => f.Id == privilegeModel.MembershipFeeFoeignKey);
                bool privExist =
                    _context.PrivilegeModels.FirstOrDefault(
                        p => p.MembershipFeeFoeignKey == membershipFee.Id && p.Type == privilegeModel.Type) != null;

                if (!privExist)
                {
                    var transfers =
                   await
                       _context.TransferPayments.Where(t => t.MembershipFeeId == privilegeModel.MembershipFeeFoeignKey)
                           .ToListAsync();
                    var cashs =
                        await
                            _context.CashModel.Where(c => c.MembershipFeeId == privilegeModel.MembershipFeeFoeignKey)
                                .ToListAsync();

                    var d = DateTime.Now.AddMonths(1);
                    var newDate = new DateTime(d.Year, d.Month, 15);
                    var payments =
                        await
                            _context.Payments.Where(
                                    p => p.MembershipFeeForeignKey == membershipFee.Id && p.PaymentDeadline >= newDate)
                                .OrderBy(p => p.PaymentDeadline)
                                .ToListAsync();

                    var fullDays = (int)(membershipFee.End - membershipFee.Start).TotalDays;
                    var daysLeft = fullDays - (DateTime.Now - membershipFee.Start).TotalDays;
                    var discountDays = (int)(privilegeModel.End - privilegeModel.Start).TotalDays;

                    double fullPriceWithDiscount =
                        Math.Floor(membershipFee.AmountWithDiscount -
                                   (membershipFee.AmountWithDiscount * privilige.Discount / 100));

                    double priceWithDiscount =
                        Math.Floor(((fullDays - discountDays) * (membershipFee.RealAmount / fullDays)) +
                                   (discountDays * fullPriceWithDiscount / fullDays));

                    membershipFee.AmountWithDiscount =
                        Math.Floor((DateTime.Now - membershipFee.Start).TotalDays * membershipFee.RealAmount / fullDays) +
                        Math.Floor(((daysLeft - discountDays) * (membershipFee.RealAmount / fullDays)) +
                                   Math.Floor(discountDays * fullPriceWithDiscount / fullDays));

                    //membershipFee.MonthlyPay = Math.Floor(membershipFee.AmountWithDiscount / 12);
                    membershipFee.LeftOver = membershipFee.AmountWithDiscount;
                    double totalPayed = 0;
                    foreach (var t in transfers)
                    {
                        membershipFee.LeftOver -= t.Amount;
                        totalPayed += t.Amount;
                    }

                    foreach (var c in cashs)
                    {
                        membershipFee.LeftOver -= c.Amount;
                        totalPayed += c.Amount;
                    }

                    double leftToPay = membershipFee.LeftOver - totalPayed;
                    membershipFee.MonthlyPay = Math.Floor(leftToPay / payments.Count);
                    foreach (var pendingPaymentModel in payments)
                    {
                        pendingPaymentModel.Amount = membershipFee.MonthlyPay;
                        pendingPaymentModel.DepositOrDebt = -membershipFee.MonthlyPay;
                        _context.Update(pendingPaymentModel);
                    }
                    _context.Update(membershipFee);

                    privilegeModel.Id = Guid.NewGuid().ToString();
                    _context.Add(privilegeModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "MembershipFees");
                }

            }
            var privileges = _context.Privileges.ToList();
            ViewBag.PrivilegeType = new SelectList(privileges, "Type", "Type");
            return View(privilegeModel);
        }

        // GET: PrivilegeModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilegeModel = await _context.PrivilegeModels.SingleOrDefaultAsync(m => m.Id == id);
            if (privilegeModel == null)
            {
                return NotFound();
            }
            //ViewData["MembershipFeeFoeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id", privilegeModel.MembershipFeeFoeignKey);
            return View(privilegeModel);
        }

        // POST: PrivilegeModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,End,MembershipFeeFoeignKey,PrivilegeNumber,Start,Type")] PrivilegeModel privilegeModel)
        {
            if (id != privilegeModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(privilegeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrivilegeModelExists(privilegeModel.Id))
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
            ViewData["MembershipFeeFoeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id", privilegeModel.MembershipFeeFoeignKey);
            return View(privilegeModel);
        }

        // GET: PrivilegeModels/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilegeModel = await _context.PrivilegeModels.SingleOrDefaultAsync(m => m.Id == id);
            if (privilegeModel == null)
            {
                return NotFound();
            }

            return View(privilegeModel);
        }

        // POST: PrivilegeModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "SuperAdmin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var privilegeModel = await _context.PrivilegeModels.SingleOrDefaultAsync(m => m.Id == id);
            _context.PrivilegeModels.Remove(privilegeModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PrivilegeModelExists(string id)
        {
            return _context.PrivilegeModels.Any(e => e.Id == id);
        }

        private async Task ReCountPayments(string feeId, CashModel cashModel, TransferPayment transferPayment)
        {
            //var fee = await _context.MembershipFees.SingleOrDefaultAsync(f => f.Id == feeId);
            var payments =
                await
                    _context.Payments.Where(p => p.MembershipFeeForeignKey == feeId && p.Status != PaymentStatus.Paused)
                        .OrderBy(p => p.PaymentDeadline)
                        .ToListAsync();

            foreach (var payment in payments)
            {
                payment.Status = PaymentStatus.Pending;
                payment.DepositOrDebt = -payment.Amount;
            }

            if (cashModel != null)
            {
                double amount = cashModel.Amount;
                for (int i = 0; i < payments.Count; i++)
                {
                    //double depOrDebt = payments[i].DepositOrDebt == null ? 0 : (double)payments[i].DepositOrDebt;
                    //if (depOrDebt <= 0)
                    //{
                    if (amount >= Math.Abs(payments[i].DepositOrDebt.Value))
                    {
                        payments[i].Status = PaymentStatus.Payed;
                        payments[i].CashPaymentForeignKey = cashModel.Id;
                        payments[i].PayedOn = cashModel.Date;
                        amount -= Math.Abs(payments[i].DepositOrDebt.Value);
                        payments[i].DepositOrDebt = 0;

                        _context.Update(payments[i]);

                        if (amount == 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        //    if (payments[i].DepositOrDebt == null)
                        //    {
                        //        payments[i].DepositOrDebt = amount;
                        //    }
                        //    else
                        //{ }
                        payments[i].DepositOrDebt += amount;

                        _context.Update(payments[i]);
                        break;
                    }
                    //}
                    //else
                    //{
                    //    if (amount >= payments[i].Amount - Math.Abs(depOrDebt))
                    //    {
                    //        payments[i].Status = PaymentStatus.Payed;
                    //        payments[i].CashPaymentForeignKey = cashModel.Id;
                    //        payments[i].PayedOn = cashModel.Date;

                    //        amount -= payments[i].Amount - Math.Abs((double)payments[i].DepositOrDebt);
                    //        payments[i].DepositOrDebt = 0;

                    //        _context.Update(payments[i]);

                    //        if (amount == 0)
                    //        {
                    //            break;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        payments[i].DepositOrDebt += amount;

                    //        _context.Update(payments[i]);
                    //        break;
                    //    }
                    //}
                }
                await _context.SaveChangesAsync();
            }
            else if (transferPayment != null)
            {
                double amount = transferPayment.Amount;

                for (int i = 0; i < payments.Count; i++)
                {
                    //double depOrDebt = payments[i].DepositOrDebt == null ? 0 : (double)payments[i].DepositOrDebt;
                    //if (depOrDebt <= 0)
                    //{
                    if (amount >= Math.Abs(payments[i].DepositOrDebt.Value))
                    {
                        payments[i].Status = PaymentStatus.Payed;
                        payments[i].TransferPaymentForeignKey = transferPayment.Id;
                        payments[i].PayedOn = transferPayment.Date;
                        amount -= Math.Abs(payments[i].DepositOrDebt.Value);
                        payments[i].DepositOrDebt = 0;

                        _context.Update(payments[i]);


                        if (amount == 0)
                        {
                            break;
                        }
                    }
                    else
                    {
                        //if (payments[i].DepositOrDebt == null)
                        //{
                        //    payments[i].DepositOrDebt = amount;
                        //}
                        //else
                        //{ }
                        payments[i].DepositOrDebt += amount;

                        _context.Update(payments[i]);
                        break;
                    }
                    //}
                    //else
                    //{
                    //    if (amount >= payments[i].Amount - depOrDebt)
                    //    {
                    //        payments[i].Status = PaymentStatus.Payed;
                    //        payments[i].TransferPaymentForeignKey = transferPayment.Id;
                    //        payments[i].PayedOn = transferPayment.Date;

                    //        amount -= payments[i].Amount - (double)payments[i].DepositOrDebt;
                    //        payments[i].DepositOrDebt = 0;

                    //        _context.Update(payments[i]);



                    //        if (amount == 0)
                    //        {
                    //            break;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        payments[i].DepositOrDebt += amount;

                    //        _context.Update(payments[i]);
                    //        break;
                    //    }
                    //}
                }
                await _context.SaveChangesAsync();
            }


        }
    }
}
