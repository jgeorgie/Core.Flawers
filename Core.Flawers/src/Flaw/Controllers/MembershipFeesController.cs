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
            var privileges = _context.Privileges.ToList();
            ViewBag.PrivilegeType = new SelectList(privileges, "Type", "Type");

            return View(await _context.MembershipFees.ToListAsync());
        }




        public IActionResult AddPayment(string id)
        {
            return View();
        }


        public IActionResult CheckPrivileges()
        {
            var privileges =
                _context.PrivilegeModels.Where(p => (p.End - DateTime.Now).TotalDays <= 3).ToList();
            return Json((privileges.Count != 0).ToString());
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
            ViewBag.PrivilegeType = new SelectList(privileges, "Type", "Type");
            return View();
        }

        // POST: MembershipFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AmountWithDiscount,CurrentState,FirstName,LastName,MiddleName,RealAmount,Start,LeftOver,Periodicity,ActivePrivilegeEnd,ActivePrivilegeStart,PrivilegeType,ActivePrivilegeNo")] MembershipFee membershipFee, long? privilegeNumber)
        {
            //var rand = new Random();
            //var privileges = await _context.Privileges.ToListAsync();
            //for (int i = 0; i < 500; i++)
            //{
            //    var start = DateTime.Now.AddDays(rand.Next(1, 29)).AddMonths(rand.Next(1, 12));
            //    var fee = new MembershipFee()
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        RealAmount = i * 10000,
            //        AmountWithDiscount = i * 5000,
            //        CurrentState = rand.Next(2) == 1 ? FeeState.Active : FeeState.Pause,
            //        FirstName = ",
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
                membershipFee.Id = Guid.NewGuid().ToString();
                membershipFee.AmountWithDiscount = membershipFee.RealAmount;
                membershipFee.LeftOver = membershipFee.AmountWithDiscount;

                membershipFee.End = membershipFee.Periodicity == FeePeriodicity.Year ? membershipFee.Start.AddMonths(12) : membershipFee.Start.AddMonths(1);
                if (membershipFee.Periodicity != FeePeriodicity.Month)
                {
                    membershipFee.MonthlyPay = Math.Floor(membershipFee.RealAmount / 12);
                }

                if (!string.IsNullOrEmpty(membershipFee.PrivilegeType) && membershipFee.ActivePrivilegeStart != null && membershipFee.ActivePrivilegeEnd != null && membershipFee.ActivePrivilegeEnd > membershipFee.ActivePrivilegeStart)
                {
                    var privilige = _context.Privileges.FirstOrDefault(p => p.Type == membershipFee.PrivilegeType);

                    var priviligeModel = new PrivilegeModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Start = (DateTime)membershipFee.ActivePrivilegeStart,

                        End = (DateTime)membershipFee.ActivePrivilegeEnd,
                        Type = membershipFee.PrivilegeType + $"({privilige.Discount})",
                        MembershipFeeFoeignKey = membershipFee.Id,
                        PrivilegeNumber = (long)privilegeNumber
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
                                Amount = (membershipFee.MonthlyPay - differense),
                                Id = Guid.NewGuid().ToString(),
                                PaymentDeadline =
                                   month + i <= 12
                                       ? new DateTime(DateTime.Now.Year, month + i, 15)
                                       : new DateTime(DateTime.Now.Year + 1, month + i - 12, 15),
                                Status = PaymentStatus.Pending,
                                DepositOrDebt = -(membershipFee.MonthlyPay - differense)
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
                                Status = PaymentStatus.Pending,
                                DepositOrDebt = -membershipFee.MonthlyPay
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

        public ActionResult Filter(string State, string Privilegee, bool Returned, double from, double to, string Penalty)
        {
            var model = _context.MembershipFees.ToList();
            if (State != "-1")
            {
                switch (int.Parse(State))
                {
                    case (int)FeeState.Active:
                        model = model.Where(m => m.CurrentState == FeeState.Active).ToList();
                        break;
                    case (int)FeeState.Pause:
                        model = model.Where(m => m.CurrentState == FeeState.Pause).ToList();
                        break;
                    case (int)FeeState.Finish:
                        model = model.Where(m => m.CurrentState == FeeState.Finish).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (Privilegee != "-1")
            {
                model = model.Where(m => m.PrivilegeType == Privilegee).ToList();
            }
            //if (Returned)
            //{
            //    model =model.Where(m=>m.FeeStateChanges.Contains()
            //}
            if (from != 0)
            {
                model = model.Where(m => m.LeftOver >= from).ToList();
            }

            if (to != 0)
            {
                model = model.Where(m => m.LeftOver <= to).ToList();
            }

            if (Penalty != null)
            {
                //TODO:Something
            }
            return PartialView("_FiltredList", model);
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

                        double deposit = 0;
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
                            await ReCountPayments(membershipFee.Id, cash, null);
                        }

                        foreach (var transfer in transferPayments)
                        {
                            await ReCountPayments(membershipFee.Id, null, transfer);
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

        public async Task<IActionResult> ExportToExcel(string State, string Privilegee, bool? Returned, double from, double to, string Penalty)
        {
            var membershipFees = await _context.MembershipFees.ToListAsync();

            if (State != "-1")
            {
                switch (int.Parse(State))
                {
                    case (int)FeeState.Active:
                        membershipFees = membershipFees.Where(m => m.CurrentState == FeeState.Active).ToList();
                        break;
                    case (int)FeeState.Pause:
                        membershipFees = membershipFees.Where(m => m.CurrentState == FeeState.Pause).ToList();
                        break;
                    case (int)FeeState.Finish:
                        membershipFees = membershipFees.Where(m => m.CurrentState == FeeState.Finish).ToList();
                        break;
                    default:
                        break;
                }
            }
            if (Privilegee != "-1")
            {
                membershipFees = membershipFees.Where(m => m.PrivilegeType == Privilegee).ToList();
            }
            //if (Returned)
            //{
            //    model =model.Where(m=>m.FeeStateChanges.Contains()
            //}
            if (from != 0)
            {
                membershipFees = membershipFees.Where(m => m.LeftOver >= from).ToList();
            }

            if (to != 0)
            {
                membershipFees = membershipFees.Where(m => m.LeftOver <= to).ToList();
            }

            if (Penalty != null)
            {
                //TODO:Something
            }


            var sb = new StringBuilder();
            //Type t = membershipFees[0].GetType();
            //PropertyInfo[] pi = t.GetProperties();

            sb.Append("Անուն,Ազգանուն,Հայրանուն,Անդամավճարի չափ,Զեղչված չափ,Սկիզբ,Ավարտ,Պարտք,Ընթացիկ վիճակ\n");

            foreach (var fee in membershipFees)
            {
                sb.AppendFormat(
                    $"\"{fee.FirstName}\",\"{fee.LastName}\",\"{fee.MiddleName}\",\"{fee.RealAmount}\",\"{fee.AmountWithDiscount}\",\"{fee.Start.ToString("MM-dd-yyyy")}\",\"{fee.End.ToString("MM-dd-yyyy")}\",\"{fee.LeftOver}\",\"{fee.CurrentState}\"");
                sb.Append("\n");
            }

            var data = Encoding.UTF8.GetBytes(sb.ToString());
            var result = Encoding.UTF8.GetPreamble().Concat(data).ToArray();
            //Response.Clear();
            //Response.Headers.Add("content-disposition", "attachment;filename=MembershipFeesList.csv");
            //Response.ContentType = "text/csv";
            //await Response.WriteAsync(res);
            return File(result, "application/csv", "MembershipFeesList.csv");
        }


        private bool MembershipFeeExists(string id)
        {
            return _context.MembershipFees.Any(e => e.Id == id);
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
