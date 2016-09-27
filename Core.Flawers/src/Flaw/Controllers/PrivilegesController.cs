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
    [Authorize(Roles ="SuperAdmin")]
    public class PrivilegesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrivilegesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Privileges
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Privileges.Include(p => p.Fee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Privileges/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = await _context.Privileges.SingleOrDefaultAsync(m => m.Id == id);
            if (privilege == null)
            {
                return NotFound();
            }

            return View(privilege);
        }

        // GET: Privileges/Create
        public IActionResult Create(string feeId)
        {
            //ViewData["MembershipFeeForeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id");
            ViewBag.MembershipFeeForeignKey = feeId;
            return View();
        }

        // POST: Privileges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Discount,End,MembershipFeeForeignKey,Start")] Privilege privilege)
        {
            if (ModelState.IsValid)
            {
                privilege.Id = Guid.NewGuid().ToString();
                _context.Add(privilege);

                var fee = _context.MembershipFees.Where(f => f.Id == privilege.MembershipFeeForeignKey).SingleOrDefault();
                var fullDays = (fee.End - fee.Start).TotalDays;
                var discountDays = (privilege.End - privilege.Start).TotalDays;
                double PriceWithDiscount = fee.RealAmount - (fee.RealAmount * privilege.Discount / 100);
                fee.AmountWithDiscount = ((fullDays - discountDays) * (fee.RealAmount / fullDays)) + (discountDays * PriceWithDiscount / fullDays);
                _context.Entry(fee).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["MembershipFeeForeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id", privilege.MembershipFeeForeignKey);
            return View(privilege);
        }

        // GET: Privileges/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = await _context.Privileges.SingleOrDefaultAsync(m => m.Id == id);
            if (privilege == null)
            {
                return NotFound();
            }
            ViewData["MembershipFeeForeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id", privilege.MembershipFeeForeignKey);
            return View(privilege);
        }

        // POST: Privileges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Description,Discount,End,MembershipFeeForeignKey,Start")] Privilege privilege)
        {
            if (id != privilege.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(privilege);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrivilegeExists(privilege.Id))
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
            ViewData["MembershipFeeForeignKey"] = new SelectList(_context.MembershipFees, "Id", "Id", privilege.MembershipFeeForeignKey);
            return View(privilege);
        }

        // GET: Privileges/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var privilege = await _context.Privileges.SingleOrDefaultAsync(m => m.Id == id);
            if (privilege == null)
            {
                return NotFound();
            }

            return View(privilege);
        }

        // POST: Privileges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var privilege = await _context.Privileges.SingleOrDefaultAsync(m => m.Id == id);
            _context.Privileges.Remove(privilege);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PrivilegeExists(string id)
        {
            return _context.Privileges.Any(e => e.Id == id);
        }
    }
}
