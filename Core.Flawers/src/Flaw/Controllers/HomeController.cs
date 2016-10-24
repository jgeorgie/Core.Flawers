using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Flaw.Data;
using Flaw.Models;
using Microsoft.EntityFrameworkCore;

namespace Flaw.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SearchFees(string FirstName, string LastName, string MiddleName)
        {
            List<MembershipFee> fees = new List<MembershipFee>();
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
            {
                fees = await _context.MembershipFees.Where(m => m.FirstName.Contains(FirstName) && m.LastName.Contains(LastName)).ToListAsync();
            }
            if (!string.IsNullOrEmpty(MiddleName))
            {
                fees = await _context.MembershipFees.Where(m => m.MiddleName.Contains(MiddleName)).ToListAsync();
            }

            if (fees.Count != 0)
            {
                return PartialView("_searchFees", fees);
            }
            else
            {
                return NotFound();
            }
            
        }

        public async Task<IActionResult> SearchCashes(string FirstName, string LastName, string MiddleName)
        {
            List<CashModel> cashes = new List<CashModel>();
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
            {
                cashes = await _context.CashModel.Where(m => m.FullName.Contains(FirstName)&&m.FullName.Contains(LastName)).ToListAsync();
            }

            if (cashes.Count != 0)
            {
                return PartialView("_searchCashes", cashes);
            }
            else
            {
               return NotFound();
            }
            
        }


        public async Task<IActionResult> SearchPayments(string FirstName, string LastName, string MiddleName)
        {
            List<TransferPayment> payments = new List<TransferPayment>();
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
            {
                payments = await _context.TransferPayments.Where(m => m.FullName.Contains(FirstName) && m.FullName.Contains(LastName)).ToListAsync();
            }
            if (!string.IsNullOrEmpty(MiddleName))
            {
                payments = await _context.TransferPayments.Where(m => m.FullName.Contains(MiddleName)).ToListAsync();
            }

            if (payments.Count != 0)
            {
                return PartialView("_searchPayments", payments);
            }
            else
            {
                return NotFound();
            }
            
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
