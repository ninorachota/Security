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
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginWith2faModel> _logger;

        public TransactionsController(UserService userService, UserManager<IdentityUser> userManager, ApplicationDbContext context, SignInManager<IdentityUser> signInManager, ILogger<LoginWith2faModel> logger)
        {
            _userService = userService;
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
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
            return View(new Transactions() { Sender = _userService.GetCurrentIdentityUser().Result });
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,Reseaver,TwoFactorCode")] Transactions transactions)
        {
            if (!ModelState.IsValid)
            {
                return View(transactions);
            }

            var user = _userService.GetCurrentIdentityUser().Result;
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }
            
            var result = await _userManager.VerifyTwoFactorTokenAsync(user, new IdentityOptions().Tokens.AuthenticatorTokenProvider, transactions.TwoFactorCode);

            if (result)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                transactions.DateTime = DateTime.Now;
                transactions.Sender = _userService.GetCurrentIdentityUser().Result;
                transactions.Reseaver = _userService.GetIdentityUserById(transactions.Reseaver.Id);
                transactions.Sender.Monney = transactions.Sender.Monney - transactions.Amount;
                transactions.Reseaver.Monney = transactions.Reseaver.Monney + transactions.Amount;
                _context.Add(transactions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else if (user.LockoutEnabled)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
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
