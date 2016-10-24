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
            if (ModelState.IsValid)
            {
                var privilige = _context.Privileges.FirstOrDefault(p => p.Type == privilegeModel.Type);
                var membershipFee =
                    await
                        _context.MembershipFees.SingleOrDefaultAsync(f => f.Id == privilegeModel.MembershipFeeFoeignKey);
                //var priviligeModel = new PrivilegeModel()
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    Start = (DateTime)membershipFee.ActivePrivilegeStart,

                //    End = (DateTime)membershipFee.ActivePrivilegeEnd,
                //    Type = membershipFee.PrivilegeType + $"({privilige.Discount})",
                //    MembershipFeeFoeignKey = membershipFee.Id,
                //    PrivilegeNumber = (long)privilegeNumber
                //};
                //_context.Add(priviligeModel);

                var fullDays = (int)(membershipFee.End - membershipFee.Start).TotalDays;
                var discountDays = (int)(privilegeModel.End - privilegeModel.Start).TotalDays;

                double priceWithDiscount =
                    Math.Floor(membershipFee.AmountWithDiscount - (membershipFee.RealAmount * privilige.Discount / 100));

                membershipFee.AmountWithDiscount =
                    Math.Floor(((fullDays - discountDays) * (membershipFee.AmountWithDiscount / fullDays)) +
                               (discountDays * priceWithDiscount / fullDays));

                membershipFee.MonthlyPay = Math.Floor(membershipFee.AmountWithDiscount / 12);
                membershipFee.LeftOver = membershipFee.AmountWithDiscount;

                privilegeModel.Id = Guid.NewGuid().ToString();
                _context.Add(privilegeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","MembershipFees");
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
    }
}
