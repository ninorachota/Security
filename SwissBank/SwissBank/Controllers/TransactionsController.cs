using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SwissBank.Areas.Identity.Pages.Account;
using SwissBank.Data;
using SwissBank.Data.Models;
using SwissBank.Services;

namespace SwissBank.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private readonly UserService _userService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly LoggerService _loggerService;
        private readonly ILogger<LoginWith2faModel> _logger;

        public TransactionsController(UserService userService, UserManager<IdentityUser> userManager, ApplicationDbContext context, LoggerService loggerService, ILogger<LoginWith2faModel> logger)
        {
            _userService = userService;
            _userManager = userManager;
            _context = context;
            _loggerService = loggerService;
            _logger = logger;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Transactionses
                .Where(x => x.Sender.Id.Equals(_userService.GetCurrentUserId()))
                .ToListAsync());
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
            return View(new Transactions() { Sender = _userService.GetCurrentIdentityUser() });
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,Receiver,TwoFactorCode")] Transactions transactions)
        {
            if (!ModelState.IsValid)
            {
                return View(transactions);
            }

            var user = _userService.GetCurrentIdentityUser();
            if (user == null)
            {
                _loggerService.Add("Unable to load two-factor authentication user for money transfer to.", transactions.Receiver.Id);
                ViewBag.Message = "Unable to load two-factor authentication user for money transfer.";
            }

            if (transactions.Amount > user.Monney)
            {
                ViewBag.Message = "Not enough money";
                return View(transactions);
            }
            
            var result = await _userManager.VerifyTwoFactorTokenAsync(user, new IdentityOptions().Tokens.AuthenticatorTokenProvider, transactions.TwoFactorCode);

            if (result)
            {
                _loggerService.Add("logged in with 2fa for money transfer.", user);
                transactions.DateTime = DateTime.Now;
                transactions.Sender = user;
                transactions.Receiver = _userService.GetIdentityUserById(transactions.Receiver.Id);
                transactions.Sender.Monney = transactions.Sender.Monney - transactions.Amount;
                transactions.Receiver.Monney = transactions.Receiver.Monney + transactions.Amount;
                _loggerService.Add(transactions.ToString(), user);
                _context.Add(transactions);
                _loggerService.Add(_context.Transactionses.Last().ToString(), user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else if (user.LockoutEnabled)
            {
                _loggerService.Add("User with ID account locked out.", user);
                return RedirectToPage("./Lockout");
            }
            else
            {
                _loggerService.Add("Invalid authenticator code entered for user with ID.", user);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View(transactions);
            }
        }
        
        private bool TransactionsExists(int id)
        {
            return _context.Transactionses.Any(e => e.Id == id);
        }
    }
}
