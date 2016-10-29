using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Flaw.Data;
using Flaw.Models;
using System.Xml.Serialization;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Flaw.Controllers
{
    [Authorize]
    public class CashModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CashModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CashModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.CashModel.ToListAsync());
        }

        // GET: CashModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashModel = await _context.CashModel.SingleOrDefaultAsync(m => m.Id == id);
            if (cashModel == null)
            {
                return NotFound();
            }

            return View(cashModel);
        }


        public void ExportToXML(string id)
        {
            var cashModel = _context.CashModel.SingleOrDefault(c => c.Id == id);

            var xs = new XmlSerializer(cashModel.GetType());
            HttpContext.Response.ContentType = "text/xml";

            xs.Serialize(HttpContext.Response.Body, cashModel);
        }



        // GET: CashModels/Create
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateForFee(string Id)
        {
            ViewBag.FeeId = Id;
            return PartialView("_CreateForFee");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateForFee([Bind("Id,OrdersNumber,Date,AccountingPass,Amount,Destination,FullName,Type,Account,MembershipFeeId")] CashModel cashModel)
        {
            if (ModelState.IsValid)
            {
                var fee = _context.MembershipFees.SingleOrDefault(f => f.Id == cashModel.MembershipFeeId);
                fee.LeftOver -= cashModel.Amount;
                var payments = await _context.Payments.Where(p => p.MembershipFeeForeignKey == cashModel.MembershipFeeId && p.Status == PaymentStatus.Pending).OrderBy(p => p.PaymentDeadline).ToListAsync();

                double amount = cashModel.Amount;

                if (amount > 0)
                {
                    for (int i = 0; i < payments.Count; i++)
                    {
                        //double depOrDebt = payments[i].DepositOrDebt == null ? 0 : (double)payments[i].DepositOrDebt;
                        //if (depOrDebt <= 0)
                        //{
                        if (amount >= payments[i].Amount)
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
                        //        payments[i].CashPaymentForeignKey = cashModel.Id;
                        //        payments[i].PayedOn = cashModel.Date;

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
                }

                _context.Update(fee);
                cashModel.Id = Guid.NewGuid().ToString();
                _context.Add(cashModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "MembershipFees");
            }
            return View("_CreateForFee", cashModel);
        }


        // POST: CashModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrdersNumber,Date,AccountingPass,Amount,Destination,FullName,Type,Account")] CashModel cashModel)
        {
            if (ModelState.IsValid)
            {
                cashModel.Id = Guid.NewGuid().ToString();
                _context.Add(cashModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cashModel);
        }

        // GET: CashModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashModel = await _context.CashModel.SingleOrDefaultAsync(m => m.Id == id);
            @ViewBag.FeeId = cashModel.MembershipFeeId;
            if (cashModel == null)
            {
                return NotFound();
            }
            return View(cashModel);
        }

        // POST: CashModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,AccountingPass,OrdersNumber,Amount,Destination,Date,FullName,Account")] CashModel cashModel)
        {
            if (id != cashModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var previousModel = await _context.CashModel.AsNoTracking().SingleOrDefaultAsync(c => c.Id == id);
                cashModel.MembershipFeeId = previousModel.MembershipFeeId;

                if (previousModel.Amount != cashModel.Amount)
                {
                    var fee = await _context.MembershipFees.SingleOrDefaultAsync(f => f.Id == cashModel.MembershipFeeId);
                    double difference = previousModel.Amount - cashModel.Amount;
                    fee.LeftOver += difference;
                    var payments =
                        await
                            _context.Payments.Where
                            (p => p.MembershipFeeForeignKey == cashModel.MembershipFeeId &&
                               p.Status == PaymentStatus.Pending)
                                .OrderBy(d => d.PaymentDeadline)
                                    .ToListAsync();
                    payments[0].DepositOrDebt -= difference;
                    _context.Update(fee);
                    _context.Update(payments[0]);
                }

                try
                {
                    _context.Update(cashModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CashModelExists(cashModel.Id))
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
            return View(cashModel);
        }

        // GET: CashModels/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashModel = await _context.CashModel.SingleOrDefaultAsync(m => m.Id == id);
            if (cashModel == null)
            {
                return NotFound();
            }

            return View(cashModel);
        }

        // POST: CashModels/Delete/5
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var cashModel = await _context.CashModel.SingleOrDefaultAsync(m => m.Id == id);
            _context.CashModel.Remove(cashModel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CashModelExists(string id)
        {
            return _context.CashModel.Any(e => e.Id == id);
        }
    }
}
