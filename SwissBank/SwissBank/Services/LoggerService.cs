using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using SwissBank.Data;
using SwissBank.Data.Models;

namespace SwissBank.Services
{
    public class LoggerService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;

        public LoggerService(ApplicationDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async void Add(string log, String email)
        {
            await _context.Loggs.AddAsync(new Logg() { DateTime = DateTime.Now, Log = log, user = _context.Users.First(u => u.Email.Equals(email)) });
            await _context.SaveChangesAsync();
        }

        public async void Add(string log, User user)
        {
            await _context.Loggs.AddAsync(new Logg() { DateTime = DateTime.Now, Log = log, user = user });
            await _context.SaveChangesAsync();
        }

        public async void Add(string log)
        {
            User user = _userService.GetCurrentIdentityUser();
            await _context.Loggs.AddAsync(new Logg() { DateTime = DateTime.Now, Log = log, user = user });
            await _context.SaveChangesAsync();
        }
    }
}
