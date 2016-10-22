using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Flaw.Data;
using Flaw.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

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


        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AmountChangeModels(string id)
        {
            var changes = await _context.FeeAmountChangeModels.Where(c => c.MembershipFeeForeignKey == id).ToListAsync();
            if (changes.Count != 0)
            {
                return PartialView("_AmountChangeModels", changes);
            }
            else
            {
                return NotFound();
            }

        }

        public async Task<IActionResult> GetPauseReactivateInfo(string id)
        {
            var stateChanges = await _context.FeeStateChanges.Where(f => f.MembershipFeeForeignKey == id).ToListAsync();
            if (stateChanges.Count != 0)
            {
                return PartialView("_FeeStateChanges", stateChanges);
            }
            else
            {
                return NotFound();
            }
            

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

        //public IActionResult CountDebt(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var membershipFee = _context.MembershipFees.SingleOrDefault(f => f.Id == id);

        //    if (membershipFee.CurrentState != FeeState.Pause && membershipFee.CurrentState != FeeState.Finish)
        //    {
        //        var priveleges = _context.Privileges.Where(p => p.MembershipFeeForeignKey == membershipFee.Id && p.Start <= DateTime.Now.Date).ToList();
        //        double dept = 0;
        //        double totalDays = (membershipFee.End - membershipFee.Start).TotalDays;
        //        double currentDays = (DateTime.Now.Date - membershipFee.Start).TotalDays + 1;
        //        double daysLeft = currentDays;

        //        foreach (var privelege in priveleges)
        //        {
        //            double privelegeDays = privelege.End <= DateTime.Now
        //                ? (privelege.End - privelege.Start).TotalDays : (DateTime.Now.Date - privelege.Start).TotalDays + 1;
        //            double amountWithPriv = membershipFee.RealAmount - (membershipFee.RealAmount * privelege.Discount) / 100;
        //            dept += privelegeDays * (amountWithPriv / totalDays);
        //            daysLeft -= privelegeDays;
        //        }
        //        dept += daysLeft * (membershipFee.RealAmount / totalDays);
        //        membershipFee.currentDebt = dept;
        //        try
        //        {
        //            _context.Update(membershipFee);
        //            _context.SaveChanges();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MembershipFeeExists(membershipFee.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //    }

        //    return Json(membershipFee.currentDebt);
        //}

        // GET: MembershipFees/Create
        public IActionResult Create()
        {
            var privileges = _context.Privileges.ToList();
            //List<string> privilegeTypes = new List<string>();

            //foreach (var priv in privileges)
            //{
            //    privilegeTypes.Add(priv.Type);
            //}
            ViewBag.PrivilegeType = new SelectList(privileges, "Type", "Type");
            return View();
        }

        // POST: MembershipFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AmountWithDiscount,CurrentState,FirstName,LastName,MiddleName,RealAmount,Start,LeftOver,Periodicity,ActivePrivilegeEnd,ActivePrivilegeStart,PrivilegeType,ActivePrivilegeNo")] MembershipFee membershipFee)
        {
            if (ModelState.IsValid)
            {
                membershipFee.Id = Guid.NewGuid().ToString();
                membershipFee.AmountWithDiscount = membershipFee.RealAmount;
                membershipFee.LeftOver = membershipFee.AmountWithDiscount;

                membershipFee.End = membershipFee.Periodicity == FeePeriodicity.Year ? membershipFee.Start.AddMonths(12) : membershipFee.Start.AddMonths(1);
                if (membershipFee.Periodicity != FeePeriodicity.Month)
                {
                    membershipFee.MonthlyPay = Math.Floor(membershipFee.RealAmount / 12);
                }

                if (!string.IsNullOrEmpty(membershipFee.PrivilegeType) && membershipFee.ActivePrivilegeStart != null && membershipFee.ActivePrivilegeEnd != null)
                {
                    var privilige = _context.Privileges.FirstOrDefault(p => p.Type == membershipFee.PrivilegeType);

                    var priviligeModel = new PrivilegeModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Start = (DateTime)membershipFee.ActivePrivilegeStart,
                        End = (DateTime)membershipFee.ActivePrivilegeEnd,
                        Type = membershipFee.PrivilegeType + $"({privilige.Discount})",
                        MembershipFeeFoeignKey = membershipFee.Id,
                        PrivilegeNumber = GetHashCode(),
                    };
                    _context.Add(priviligeModel);

                    var fullDays = (int)(membershipFee.End - membershipFee.Start).TotalDays;
                    var discountDays = (int)(priviligeModel.End - priviligeModel.Start).TotalDays;

                    double priceWithDiscount =
                        Math.Floor(membershipFee.RealAmount - (membershipFee.RealAmount * privilige.Discount / 100));

                    membershipFee.AmountWithDiscount =
                        Math.Floor(((fullDays - discountDays) * (membershipFee.RealAmount / fullDays)) +
                                   (discountDays * priceWithDiscount / fullDays));

                    membershipFee.MonthlyPay = Math.Floor(membershipFee.AmountWithDiscount / 12);
                    membershipFee.LeftOver = membershipFee.AmountWithDiscount;
                }


                if (membershipFee.Periodicity == FeePeriodicity.Year)
                {
                    int month = membershipFee.Start.Month;
                    for (int i = 1; i <= 12; i++)
                    {
                        PendingPaymentModel payment;
                        if (i == 1 && membershipFee.Start.Day > 1)
                        {
                            int days = DateTime.DaysInMonth(membershipFee.Start.Year, membershipFee.Start.Month);
                            double differense = Math.Floor((membershipFee.Start.Day - 1) * membershipFee.MonthlyPay / days);
                            membershipFee.LeftOver -= differense;
                            payment = new PendingPaymentModel()
                            {
                                MembershipFeeForeignKey = membershipFee.Id,
                                Amount = membershipFee.MonthlyPay - differense,
                                Id = Guid.NewGuid().ToString(),
                                PaymentDeadline =
                                   month + i <= 12
                                       ? new DateTime(DateTime.Now.Year, month + i, 15)
                                       : new DateTime(DateTime.Now.Year + 1, month + i - 12, 15),
                                Status = PaymentStatus.Pending
                            };
                        }
                        else
                        {
                            payment = new PendingPaymentModel()
                            {
                                MembershipFeeForeignKey = membershipFee.Id,
                                Amount = membershipFee.MonthlyPay,
                                Id = Guid.NewGuid().ToString(),
                                PaymentDeadline =
                                    month + i <= 12
                                        ? new DateTime(DateTime.Now.Year, month + i, 15)
                                        : new DateTime(DateTime.Now.Year + 1, month + i - 12, 15),
                                Status = PaymentStatus.Pending
                            };
                        }

                        _context.Payments.Add(payment);

                    }
                }
                else
                {
                    membershipFee.End = membershipFee.Start.AddMonths(1);
                    int month = membershipFee.Start.AddMonths(1).Month;
                    var payment = new PendingPaymentModel()
                    {
                        MembershipFeeForeignKey = membershipFee.Id,
                        Amount = Math.Floor(membershipFee.RealAmount / 12),
                        Id = Guid.NewGuid().ToString(),
                        PaymentDeadline = new DateTime(DateTime.Now.Year, month, 15),
                        Status = PaymentStatus.Pending
                    };

                    _context.Payments.Add(payment);
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
            var membershipFee = await _context.MembershipFees.SingleOrDefaultAsync(f => f.Id == id);
            if (membershipFee.CurrentState == FeeState.Finish)
            {
                return RedirectToAction("Index", "MembershipFees");
            }
            if (id == null)
            {
                return NotFound();
            }


            if (membershipFee == null)
            {
                return NotFound();
            }
            return View(membershipFee);
        }


        public async Task<IActionResult> GetPriviliges(string id)
        {
            var priviliges = await _context.PrivilegeModels.Where(p => p.MembershipFeeFoeignKey == id).ToListAsync();
            if (priviliges.Count != 0)
            {
                return PartialView("_PriviligeList", priviliges);
            }
            else
            {
                return NotFound();
            }

        }



        //POST: MembershipFees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,AmountWithDiscount,CurrentState,FirstName,LastName,MiddleName,RealAmount,Start,End,LeftOver,Periodicity,ActivePrivilegeEnd,ActivePrivilegeStart,PrivilegeType,ActivePrivilegeNo")] MembershipFee membershipFee)
        {
            if (id != membershipFee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var previousModel = _context.MembershipFees.Where(m => m.Id == membershipFee.Id).AsNoTracking().SingleOrDefault();
                membershipFee.ActivePrivilegeNo = previousModel.ActivePrivilegeNo;
                membershipFee.ActivePrivilegeEnd = previousModel.ActivePrivilegeEnd;
                membershipFee.ActivePrivilegeStart = previousModel.ActivePrivilegeEnd;
                membershipFee.LeftOver = previousModel.LeftOver;
                membershipFee.PrivilegeType = previousModel.PrivilegeType;
                membershipFee.Periodicity = previousModel.Periodicity;
                membershipFee.MonthlyPay = previousModel.MonthlyPay;

                if (previousModel.RealAmount != membershipFee.RealAmount)
                {
                    if (HttpContext.User.Identity.Name != "admin@admin.am")
                    {
                        membershipFee.RealAmount = previousModel.RealAmount;
                        membershipFee.AmountWithDiscount = previousModel.AmountWithDiscount;
                    }
                    else
                    {
                        var privelegeModel = await _context.PrivilegeModels.Where(p => p.MembershipFeeFoeignKey == membershipFee.Id).SingleOrDefaultAsync();
                        if (privelegeModel == null)
                        {
                            membershipFee.AmountWithDiscount = membershipFee.RealAmount;
                        }
                        else
                        {
                            var privelege = _context.Privileges.FirstOrDefault(p => p.Type == privelegeModel.Type);
                            var fullDays = (membershipFee.End - membershipFee.Start).TotalDays;
                            var discountDays = (privelegeModel.End - privelegeModel.Start).TotalDays;
                            double PriceWithDiscount = membershipFee.RealAmount - (membershipFee.RealAmount * privelege.Discount / 100);
                            membershipFee.AmountWithDiscount = ((fullDays - discountDays) * (membershipFee.RealAmount / fullDays)) + (discountDays * PriceWithDiscount / fullDays);

                            membershipFee.LeftOver = membershipFee.AmountWithDiscount;
                            var transfers = await _context.TransferPayments.Where(t => t.MembershipFeeId == privelegeModel.MembershipFeeFoeignKey).ToListAsync();
                            var cashs = await _context.CashModel.Where(c => c.MembershipFeeId == privelegeModel.MembershipFeeFoeignKey).ToListAsync();

                            foreach (var t in transfers)
                            {
                                membershipFee.LeftOver -= t.Amount;
                            }
                            foreach (var c in cashs)
                            {
                                membershipFee.LeftOver -= c.Amount;
                            }

                        }
                        var pendingPayments =
                               await
                                   _context.Payments.Where(p => p.MembershipFeeForeignKey == membershipFee.Id && p.PaymentDeadline >= DateTime.Now)
                                       .ToListAsync();

                        membershipFee.MonthlyPay = Math.Floor(membershipFee.AmountWithDiscount / 12);
                        double newLeftOver = 0;
                        foreach (var payment in pendingPayments)
                        {
                            if (payment.PaymentDeadline.Month == DateTime.Now.AddMonths(1).Month)
                            {
                                int days = DateTime.DaysInMonth(payment.PaymentDeadline.Year,
                                    payment.PaymentDeadline.Month);
                                payment.Amount = ((previousModel.AmountWithDiscount / (12 * days)) * DateTime.Now.Day) +
                                                 (membershipFee.AmountWithDiscount / (12 * days) * (days - DateTime.Now.Day));
                            }
                            else
                            {
                                payment.Amount = membershipFee.MonthlyPay;
                            }
                            newLeftOver += payment.Amount;
                            _context.Update(payment);
                        }
                        membershipFee.LeftOver = Math.Floor(newLeftOver);

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
                    var stateChangeModel = new FeeStateChangeModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        MembershipFeeForeignKey = membershipFee.Id,
                        ChangeDate = DateTime.Now,
                        PreviousState = previousModel.CurrentState,
                        NewState = membershipFee.CurrentState
                    };
                    _context.Add(stateChangeModel);

                    if (membershipFee.CurrentState == FeeState.Pause)
                    {

                        membershipFee.Paused = DateTime.Now;
                        int howManyDays = (int)(membershipFee.Paused.Value - membershipFee.Start).TotalDays;
                        int fullDays = (int)(membershipFee.End - membershipFee.Start).TotalDays;
                        double paymentsSum = 0;
                        var cashPayments =
                            await
                              _context.CashModel.Where(c => c.MembershipFeeId == membershipFee.Id)
                                .ToListAsync();
                        var transferPayments =
                            await
                                _context.TransferPayments.Where(t => t.MembershipFeeId == membershipFee.Id)
                                    .ToListAsync();

                        foreach (var cash in cashPayments)
                        {
                            paymentsSum += cash.Amount;
                        }

                        foreach (var transfer in transferPayments)
                        {
                            paymentsSum += transfer.Amount;
                        }
                        membershipFee.currentDebt = (membershipFee.AmountWithDiscount / fullDays) * howManyDays - paymentsSum;
                        int days = DateTime.DaysInMonth(membershipFee.Paused.Value.Year, membershipFee.Paused.Value.Month);
                        var payment =
                            await
                                _context.Payments.SingleOrDefaultAsync(
                                    p =>
                                        p.MembershipFeeForeignKey == membershipFee.Id &&
                                        p.PaymentDeadline.Month == membershipFee.Paused.Value.Month + 1);
                        payment.Amount = Math.Floor((previousModel.MonthlyPay / days) * (DateTime.Now.Day));
                        _context.Update(payment);
                    }
                    else if (previousModel.CurrentState == FeeState.Pause && membershipFee.CurrentState == FeeState.Active)
                    {
                        membershipFee.Paused = previousModel.Paused;
                        membershipFee.Reactiveted = DateTime.Now;
                        membershipFee.TotalDaysPaused += (int)(DateTime.Now - previousModel.Paused.Value).TotalDays;
                        int days = DateTime.DaysInMonth(membershipFee.Reactiveted.Value.Year, membershipFee.Reactiveted.Value.Month);
                        int howManyDays = (int)(membershipFee.Paused.Value - membershipFee.Start).TotalDays;
                        int fullDays = (int)(membershipFee.End - membershipFee.Start).TotalDays;
                        //membershipFee.currentDebt = (membershipFee.AmountWithDiscount / fullDays);
                        var payment =
                           await
                               _context.Payments.SingleOrDefaultAsync(
                                   p =>
                                       p.MembershipFeeForeignKey == membershipFee.Id &&
                                       p.PaymentDeadline.Month == membershipFee.Reactiveted.Value.Month + 1);
                        payment.Amount = Math.Floor((previousModel.MonthlyPay / days) * (days - DateTime.Now.Day));
                        membershipFee.LeftOver -= previousModel.MonthlyPay - payment.Amount;
                        _context.Update(payment);
                    }
                    else if (membershipFee.CurrentState == FeeState.Finish)
                    {
                        if (previousModel.End != DateTime.Now.Date)
                        {
                            membershipFee.End = DateTime.Now.Date;
                            int days = DateTime.DaysInMonth(membershipFee.End.Year, membershipFee.End.Month);
                            var pendingPayments = _context.Payments.Where(p => p.PaymentDeadline > DateTime.Now);
                            foreach (var payment in pendingPayments)
                            {
                                if (payment.PaymentDeadline.Month == DateTime.Now.Month + 1)
                                {
                                    payment.Amount = Math.Floor((previousModel.MonthlyPay / days) * DateTime.Now.Day);
                                }
                                else
                                {
                                    payment.Status = PaymentStatus.Cancelled;
                                }
                                _context.Update(payment);
                            }
                        }
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

        public async Task ExportToExcel()
        {
            var membershipFees = await _context.MembershipFees.ToListAsync();
            var sb = new StringBuilder();
            //Type t = membershipFees[0].GetType();
            //PropertyInfo[] pi = t.GetProperties();

            sb.Append("?????,????????,?????????,??????????? ???,??????? ???,?????,?????,?????,??????? ?????\n");

            foreach (var fee in membershipFees)
            {
                sb.AppendFormat(
                    $"\"{fee.FirstName}\",\"{fee.LastName}\",\"{fee.MiddleName}\",\"{fee.RealAmount}\",\"{fee.AmountWithDiscount}\",\"{fee.Start.ToString("MM-dd-yyyy")}\",\"{fee.End.ToString("MM-dd-yyyy")}\",\"{fee.LeftOver}\",\"{fee.CurrentState}\"");
                sb.Append("\n");
            }

            var bytes = Encoding.GetEncoding(1252).GetBytes(sb.ToString());
            string csv = Encoding.UTF8.GetString(bytes);
            Response.Clear();
            Response.Headers.Add("content-disposition", "attachment;filename=MembershipFeesList.csv");
            Response.ContentType = "text/csv";
            await Response.WriteAsync(csv, Encoding.UTF8);
        }


        private bool MembershipFeeExists(string id)
        {
            return _context.MembershipFees.Any(e => e.Id == id);
        }
    }
}
