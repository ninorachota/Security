using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwissBank.Data;
using SwissBank.Data.Models;
using SwissBank.Services;

namespace SwissBank.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly UserService _userService;
        private readonly ApplicationDbContext _context;

        public TransactionsController(UserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transactionses.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactionses
                .Where(x => x.Sender.Id.Equals(_userService.GetCurrentUserId()))
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transactions == null)
            {
                return NotFound();
            }

            return View(transactions);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            return View(new Transactions() { Sender = _userService.GetCurrentIdentityUser().Result });
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,Reseaver")] Transactions transactions)
        {
            if (ModelState.IsValid)
            {
                transactions.DateTime = DateTime.Now;
                transactions.Sender = _userService.GetCurrentIdentityUser().Result;
                transactions.Reseaver = _userService.GetIdentityUserById(transactions.Reseaver.Id);
                transactions.Sender.Monney = transactions.Sender.Monney - transactions.Amount;
                transactions.Reseaver.Monney = transactions.Reseaver.Monney + transactions.Amount;
                _context.Add(transactions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transactions);
        }
        
        private bool TransactionsExists(int id)
        {
            return _context.Transactionses.Any(e => e.Id == id);
        }
    }
}
