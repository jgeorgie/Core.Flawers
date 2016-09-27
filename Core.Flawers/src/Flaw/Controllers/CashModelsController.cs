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
            var cashModel = _context.CashModel.Where(c => c.Id == id).SingleOrDefault();
            //XmlRootAttribute root = new XmlRootAttribute("response");

            var xs = new XmlSerializer(cashModel.GetType());
            HttpContext.Response.ContentType = "text/xml";

            xs.Serialize(HttpContext.Response.Body, cashModel);
        }



        // GET: CashModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CashModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrdersNumber,Date,AccountingPass,Amount,Destination,FullName,Type,Аccount")] CashModel cashModel)
        {
            if (ModelState.IsValid)
            {
                if (cashModel.Type == BargainType.CashIn)
                {
                    //TODO: MembershipFeeFoerignKey
                    var payment = new PaymentModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Amount = cashModel.Amount,
                        Date = cashModel.Date,
                        Type = PaymentType.Cash,
                        CashPaymentId = cashModel.Id,
                        //MembershipFeeForeignKey=cashModel.
                    };
                }
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
        public async Task<IActionResult> Edit(string id, [Bind("Id,AccountingPass,Amount,Destination,FullName,Type,Аccount")] CashModel cashModel)
        {
            if (id != cashModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
