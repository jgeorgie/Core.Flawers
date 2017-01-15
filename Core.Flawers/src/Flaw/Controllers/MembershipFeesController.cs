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
using System.Collections.Generic;
using Flaw.Helpers;

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
        public async Task<IActionResult> Index(
            bool Returned,
            double LeftOverFrom,
            double LeftOverTo,
            string FirstName,
            string LastName,
            int? page,
            int LicenseNumber = -1,
            string Penalty = "",
            string currentState = "-1",
            string PrivilegeType = "-1"
            )

        {
            var model = _context.MembershipFees.AsQueryable();
            ViewData["PrivilegeTypeFilterParam"] = PrivilegeType;
            ViewData["currentStateFilterParam"] = currentState;
            ViewData["LeftOverFromFilterParam"] = LeftOverFrom;
            ViewData["LeftOverToFilterParam"] = LeftOverTo;
            ViewData["PenaltyFilterParam"] = Penalty;
            ViewData["ReturnedFilterParam"] = Returned.ToString().ToLower();
            ViewData["LicenseNumber"] = LicenseNumber;
            ViewData["FirstName"] = FirstName;
            ViewData["LastName"] = LastName;

            if (LicenseNumber != -1)
            {
                model = model.Where(m => m.LicenseNumber == LicenseNumber);
            }

            if (FirstName != null)
            {
                model = model.Where(m => m.FirstName.ToLower().Contains(FirstName.ToLower()));
            }

            if (LastName != null)
            {
                model = model.Where(m => m.LastName.ToLower().Contains(LastName.ToLower()));
            }


            if (currentState != "-1")
            {
                switch (int.Parse(currentState))
                {
                    case (int)FeeState.Active:
                        model = model.Where(m => m.CurrentState == FeeState.Active);
                        break;
                    case (int)FeeState.Pause:
                        model = model.Where(m => m.CurrentState == FeeState.Pause);
                        break;
                    case (int)FeeState.Finish:
                        model = model.Where(m => m.CurrentState == FeeState.Finish);
                        break;
                    default:
                        break;
                }
            }

            if (PrivilegeType != "-1")
            {
                model = model.Where(m => m.PrivilegeType == PrivilegeType);
            }

            if (LeftOverFrom != 0)
            {
                model = model.Where(m => m.LeftOver >= LeftOverFrom);
            }

            if (LeftOverTo != 0)
            {
                model = model.Where(m => m.LeftOver <= LeftOverTo);
            }

            //if (Penalty != null)
            //{
            //    //TODO:Something
            //}

            if (Returned)
            {
                model = model.Where(m => m.Reactiveted != null);
            }
            var p = await PaginatedList<MembershipFee>.CreateAsync(model.AsNoTracking(), page ?? 1, 20);
            var privileges = _context.Privileges.ToList();
            ViewBag.PrivilegeType = new SelectList(privileges, "Type", "Type");

            return View(p);
        }


        public async Task<IActionResult> AddPayment(int id)
        {
            var fee = await _context.MembershipFees.AsNoTracking().SingleOrDefaultAsync(f => f.Id == id);
            if (fee.CurrentState != FeeState.Active)
            {
                return BadRequest();
            }
            return View();
        }


        public IActionResult CheckPrivileges()
        {
            var privileges =
                _context.PrivilegeModels.AsNoTracking().Where(p => (p.End - DateTime.Now).TotalDays <= 3).ToList();
            return Json((privileges.Count != 0).ToString());
        }


        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AmountChangeModels(int id)
        {
            var changes = await _context.FeeAmountChangeModels.AsNoTracking().Where(c => c.MembershipFeeForeignKey == id).ToListAsync();
            if (changes.Count != 0)
            {
                return PartialView("_AmountChangeModels", changes);
            }
            else
            {
                return NotFound();
            }

        }

        public async Task<IActionResult> GetPauseReactivateInfo(int id)
        {
            var stateChanges = await _context.FeeStateChanges.AsNoTracking().Where(f => f.MembershipFeeForeignKey == id).ToListAsync();
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
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var membershipFee = await _context.MembershipFees.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);

            if (membershipFee == null)
            {
                return NotFound();
            }

            return View(membershipFee);
        }

        // GET: MembershipFees/Create
        public IActionResult Create()
        {
            var privileges = _context.Privileges.ToList();
            ViewBag.PrivilegeType = new SelectList(privileges, "Type", "Type");
            return View();
        }

        // POST: MembershipFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AmountWithDiscount,FirstName,LastName,MiddleName,RealAmount,Start,Periodicity,ActivePrivilegeEnd,ActivePrivilegeStart,PrivilegeType,LicenseNumber")] MembershipFee membershipFee)
        {
            //var rand = new Random();
            //var privileges = await _context.Privileges.ToListAsync();
            //for (int i = 1; i < 500; i++)
            //{
            //    var start = DateTime.Now.AddDays(rand.Next(1, 29)).AddMonths(rand.Next(1, 12));
            //    var fee = new MembershipFee()
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        RealAmount = i * 10000,
            //        AmountWithDiscount = i * 5000,
            //        CurrentState = rand.Next(2) == 1 ? FeeState.Active : FeeState.Pause,
            //        FirstName = "Firstname" + i,
            //        LastName = "LastName" + i,
            //        MiddleName = "MiddleName" + i,
            //        Start = start,
            //        End = start.AddMonths(12),
            //        LeftOver = i * 10000,
            //        MonthlyPay = i * 10000 / 12,
            //        Periodicity = FeePeriodicity.Year,
            //        PrivilegeType = rand.Next(0, 2) == 1 ? privileges[1].Type : privileges[0].Type,
            //        ActivePrivilegeStart = start,
            //        ActivePrivilegeEnd = start.AddMonths(12),
            //        ActivePrivilegeNo = rand.Next(),
            //    };
            //    _context.Add(fee);
            //}

            //for (int i = 0; i < 50; i++)
            //{
            //    var start = DateTime.Now.AddDays(rand.Next(1, 29)).AddMonths(rand.Next(1, 12));
            //    var payments = new TransferPayment()
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Amount = i * 5100,
            //        Date = DateTime.Now.AddDays(i * 3),
            //        FullName = "FirstName" + i * 10 + " LastName" + i * 10,
            //        Destination = "chem manum",
            //        PaymentNo = (i * 1000).ToString(),
            //    };
            //    _context.Add(payments);
            //}


            //for (int i = 0; i < 100; i++)
            //{
            //    var cashes = new CashModel()
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        FullName = "FirstName" + i * 10 + " LastName" + i * 10,
            //        AccountingPass = (i * 123).ToString(),
            //        Amount = i * 4320,
            //        Date = DateTime.Now.AddDays(i * 2),
            //        Destination = "chem manum asi",
            //        OrdersNumber = (i * 10000 + i * 378).ToString(),
            //        Type = rand.Next(0, 2) == 1 ? BargainType.CashIn : BargainType.CashOut,
            //        //?ccount ="asfasfasfasfas"
            //    };
            //    _context.Add(cashes);
            //}

            //await _context.SaveChangesAsync();

            if (ModelState.IsValid)
            {
                membershipFee.AmountWithDiscount = membershipFee.RealAmount;
                membershipFee.LeftOver = membershipFee.AmountWithDiscount;

                membershipFee.End = membershipFee.Periodicity == FeePeriodicity.Year
                    ? membershipFee.Start.AddMonths(12)
                    : membershipFee.Start.AddMonths(1);

                if (membershipFee.Periodicity != FeePeriodicity.Month)
                {
                    membershipFee.MonthlyPay = Math.Floor(membershipFee.RealAmount / 12);
                }

                if (!string.IsNullOrEmpty(membershipFee.PrivilegeType) && membershipFee.ActivePrivilegeStart != null &&
                    membershipFee.ActivePrivilegeEnd > membershipFee.ActivePrivilegeStart)
                {
                    var privilige = _context.Privileges.AsNoTracking().FirstOrDefault(p => p.Type == membershipFee.PrivilegeType);

                    var priviligeModel = new PrivilegeModel()
                    {
                        Start = membershipFee.ActivePrivilegeStart.Value,
                        End = membershipFee.ActivePrivilegeEnd.Value,
                        Type = membershipFee.PrivilegeType + $"({privilige.Discount})",
                        MembershipFeeFoeignKey = membershipFee.Id
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

                _context.Add(membershipFee);
                await _context.SaveChangesAsync();
                SiteHelpers.AddPaymentsForFee(membershipFee.Id, _context);
                return RedirectToAction("Index");
            }

            return View(membershipFee);
        }

        public async Task<IActionResult> GetPendingPayments(int id)
        {
            var payments =
                await _context.Payments.AsNoTracking()
                    .Where(p => p.MembershipFeeForeignKey == id)
                    .OrderBy(p => p.PaymentDeadline)
                    .ToListAsync();
            if (payments.Count != 0)
            {
                return PartialView("_PendingPayments", payments);
            }
            return BadRequest();
        }

        public async Task<IActionResult> GetTransferPayments(int id)
        {
            var transfer =
                await _context.TransferPayments.AsNoTracking().Where(t => t.MembershipFeeId == id).ToListAsync();

            if (transfer.Count != 0)
            {
                return PartialView("_Transfers", transfer);
            }
            return NotFound();
        }

        public async Task<IActionResult> GetCashPayments(int id)
        {
            var transfer = await _context.CashModel.AsNoTracking().Where(c => c.MembershipFeeId == id).ToListAsync();

            if (transfer.Count != 0)
            {
                return PartialView("_CashPayments", transfer);
            }

            return NotFound();
        }


        // GET: MembershipFees/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var membershipFee = await _context.MembershipFees.SingleOrDefaultAsync(f => f.Id == id);
            if (id == 0)
            {
                return NotFound();
            }

            if (membershipFee == null)
            {
                return NotFound();
            }

            if (membershipFee.CurrentState == FeeState.Finish)
            {
                return RedirectToAction("Index", "MembershipFees");
            }

            return View(membershipFee);
        }


        public async Task<IActionResult> GetPriviliges(int id)
        {
            var priviliges =
                await _context.PrivilegeModels.AsNoTracking().Where(p => p.MembershipFeeFoeignKey == id).ToListAsync();
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,AmountWithDiscount,CurrentState,FirstName,LastName,MiddleName,RealAmount,Start,End,LeftOver,Periodicity,ActivePrivilegeEnd,ActivePrivilegeStart,PrivilegeType,LicenseNumber")] MembershipFee membershipFee)
        {
            if (id != membershipFee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var previousModel =
                    _context.MembershipFees.AsNoTracking().SingleOrDefault(m => m.Id == membershipFee.Id);
                membershipFee.LicenseNumber = previousModel.LicenseNumber;
                membershipFee.ActivePrivilegeEnd = previousModel.ActivePrivilegeEnd;
                membershipFee.ActivePrivilegeStart = previousModel.ActivePrivilegeEnd;
                membershipFee.LeftOver = previousModel.LeftOver;
                membershipFee.PrivilegeType = previousModel.PrivilegeType;
                membershipFee.Periodicity = previousModel.Periodicity;
                membershipFee.MonthlyPay = previousModel.MonthlyPay;

                if (Math.Abs(previousModel.RealAmount - membershipFee.RealAmount) > 10 && membershipFee.End < DateTime.Now)
                {
                    if (HttpContext.User.Identity.Name != "admin@admin.am")
                    {
                        membershipFee.RealAmount = previousModel.RealAmount;
                        membershipFee.AmountWithDiscount = previousModel.AmountWithDiscount;
                    }
                    else
                    {
                        var privelegeModel =
                            await _context.PrivilegeModels.AsNoTracking()
                                .SingleOrDefaultAsync(p => p.MembershipFeeFoeignKey == membershipFee.Id);
                        if (privelegeModel == null)
                        {
                            membershipFee.AmountWithDiscount = membershipFee.RealAmount;
                        }
                        else
                        {
                            var privelege =
                                await
                                    _context.Privileges.FirstOrDefaultAsync(
                                        p =>
                                            p.Type ==
                                            privelegeModel.Type.Substring(0, privelegeModel.Type.IndexOf('(')));
                            var fullDays = (membershipFee.End - membershipFee.Start).TotalDays;
                            var discountDays = (privelegeModel.End - privelegeModel.Start).TotalDays;
                            double priceWithDiscount = membershipFee.RealAmount - (membershipFee.RealAmount * privelege.Discount / 100);
                            membershipFee.AmountWithDiscount = ((fullDays - discountDays) *
                                                                (membershipFee.RealAmount / fullDays)) +
                                                               (discountDays * priceWithDiscount / fullDays);

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
                            payment.DepositOrDebt = -payment.Amount;
                            newLeftOver += payment.Amount;
                            _context.Update(payment);
                        }
                        membershipFee.LeftOver = Math.Floor(newLeftOver);

                        var feeAmountChange = new FeeAmountChangeModel()
                        {
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
                        //var payment =
                        //    await
                        //        _context.Payments.SingleOrDefaultAsync(
                        //            p =>
                        //                p.MembershipFeeForeignKey == membershipFee.Id &&
                        //                p.PaymentDeadline.Month == membershipFee.Paused.Value.AddMonths(1).Month);
                        var d = membershipFee.Paused.Value.AddMonths(1);
                        var nextPayDate = new DateTime(d.Year, d.Month, 14);
                        var payments =
                            await
                                _context.Payments.Where(
                                        p =>
                                            p.MembershipFeeForeignKey == membershipFee.Id &&
                                            p.PaymentDeadline >= nextPayDate)
                                    .OrderBy(p => p.PaymentDeadline)
                                    .ToListAsync();

                        var payment =
                            payments.SingleOrDefault(
                                p => p.PaymentDeadline == nextPayDate.AddDays(1));

                        //double deposit = 0;
                        foreach (var p in payments)
                        {
                            if (p.Id == payment.Id)
                            {
                                p.Amount = Math.Floor((membershipFee.MonthlyPay / days) * (DateTime.Now.Day));
                                double difference = membershipFee.MonthlyPay - p.Amount;
                                p.DepositOrDebt += difference;
                                _context.Update(p);
                            }
                            else
                            {
                                if (p.Status == PaymentStatus.Payed)
                                {
                                    p.DepositOrDebt = -p.Amount;
                                    payment.DepositOrDebt += p.Amount;
                                    _context.Update(payment);
                                }
                                else if (Math.Abs(p.DepositOrDebt.Value) < Math.Abs(p.Amount))
                                {
                                    payment.DepositOrDebt += Math.Abs(p.Amount) - Math.Abs(p.DepositOrDebt.Value);
                                    p.DepositOrDebt = -p.Amount;
                                    _context.Update(payment);
                                }
                                p.Status = PaymentStatus.Paused;
                            }
                            _context.Update(p);


                        }

                        //_context.Update(payment);
                    }
                    else if (previousModel.CurrentState == FeeState.Pause && membershipFee.CurrentState == FeeState.Active)
                    {
                        membershipFee.Paused = previousModel.Paused;
                        membershipFee.Reactiveted = DateTime.Now;
                        membershipFee.TotalDaysPaused += (int)(DateTime.Now - previousModel.Paused.Value).TotalDays;
                        int days = DateTime.DaysInMonth(membershipFee.Reactiveted.Value.Year, membershipFee.Reactiveted.Value.Month);
                        int howManyDays = (int)(membershipFee.Paused.Value - membershipFee.Start).TotalDays;
                        int fullDays = (int)(membershipFee.End - membershipFee.Start).TotalDays;

                        //var d = membershipFee.Paused.Value.AddMonths(1);
                        //var nextPayDate = new DateTime(d.Year, d.Month, 14);

                        //var lostPayments =
                        //    await
                        //        _context.Payments.Where(
                        //            p =>
                        //                p.MembershipFeeForeignKey == membershipFee.Id &&
                        //                p.PaymentDeadline > membershipFee.Paused.Value.AddMonths(1) &&
                        //                p.PaymentDeadline <= membershipFee.Reactiveted.Value).ToListAsync();


                        var payments =
                            await
                                _context.Payments.Where(
                                    p =>
                                        p.MembershipFeeForeignKey == membershipFee.Id &&
                                        p.PaymentDeadline > membershipFee.Reactiveted.Value &&
                                        p.Status == PaymentStatus.Paused).ToListAsync();

                        foreach (var p in payments)
                        {
                            if (p.PaymentDeadline.Month == membershipFee.Reactiveted.Value.AddMonths(1).Month)
                            {
                                p.Amount = Math.Floor((membershipFee.MonthlyPay / days) * (days - DateTime.Now.Day));
                                membershipFee.LeftOver -= membershipFee.MonthlyPay - p.Amount;
                            }
                            p.Status = PaymentStatus.Pending;
                            _context.Update(p);
                        }
                        await _context.SaveChangesAsync();
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
                            SiteHelpers.ReCountPayments(membershipFee.Id, cash, null, _context);
                        }

                        foreach (var transfer in transferPayments)
                        {
                            SiteHelpers.ReCountPayments(membershipFee.Id, null, transfer, _context);
                        }

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
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var membershipFee = await _context.MembershipFees.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membershipFee = await _context.MembershipFees.SingleOrDefaultAsync(m => m.Id == id);
            _context.MembershipFees.Remove(membershipFee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult ExportToExcel(
            bool Returned,
            double LeftOverFrom,
            double LeftOverTo,
            string FirstName,
            string LastName,
            int LicenseNumber = -1,
            string Penalty = "",
            string currentState = "-1",
            string PrivilegeType = "-1"
            )
        {
            var membershipFees = _context.MembershipFees.AsNoTracking().AsQueryable();

            if (LicenseNumber != -1)
            {
                membershipFees = membershipFees.Where(m => m.LicenseNumber == LicenseNumber);
            }

            if (FirstName != null)
            {
                membershipFees = membershipFees.Where(m => m.FirstName.ToLower().Contains(FirstName.ToLower()));
            }

            if (LastName != null)
            {
                membershipFees = membershipFees.Where(m => m.LastName.ToLower().Contains(LastName.ToLower()));
            }


            if (currentState != "-1")
            {
                switch (int.Parse(currentState))
                {
                    case (int)FeeState.Active:
                        membershipFees = membershipFees.Where(m => m.CurrentState == FeeState.Active);
                        break;
                    case (int)FeeState.Pause:
                        membershipFees = membershipFees.Where(m => m.CurrentState == FeeState.Pause);
                        break;
                    case (int)FeeState.Finish:
                        membershipFees = membershipFees.Where(m => m.CurrentState == FeeState.Finish);
                        break;
                    default:
                        break;
                }
            }
            if (PrivilegeType != "-1")
            {
                membershipFees = membershipFees.Where(m => m.PrivilegeType == PrivilegeType);
            }

            if (Returned)
            {
                membershipFees = membershipFees.Where(m => m.Reactiveted != null);
            }

            if (LeftOverFrom != 0)
            {
                membershipFees = membershipFees.Where(m => m.LeftOver >= LeftOverFrom);
            }

            if (LeftOverTo != 0)
            {
                membershipFees = membershipFees.Where(m => m.LeftOver <= LeftOverTo);
            }

            //if (Penalty != null)
            //{
            //    //TODO:Something
            //}
            var sb = new StringBuilder();

            sb.Append("Անուն,Ազգանուն,Հայրանուն,Անդամավճարի չափ,Զեղչված չափ,Սկիզբ,Ավարտ,Պարտք,Ընթացիկ վիճակ\n");

            foreach (var fee in membershipFees)
            {
                sb.AppendFormat(
                    $"\"{fee.FirstName}\",\"{fee.LastName}\",\"{fee.MiddleName}\",\"{fee.RealAmount}\",\"{fee.AmountWithDiscount}\",\"{fee.Start.ToString("MM-dd-yyyy")}\",\"{fee.End.ToString("MM-dd-yyyy")}\",\"{fee.LeftOver}\",\"{fee.CurrentState}\"");
                sb.Append("\n");
            }

            var data = Encoding.UTF8.GetBytes(sb.ToString());
            var result = Encoding.UTF8.GetPreamble().Concat(data).ToArray();

            return File(result, "application/csv", "MembershipFeesList.csv");
        }


        private bool MembershipFeeExists(int id)
        {
            return _context.MembershipFees.Any(e => e.Id == id);
        }

    }
}
