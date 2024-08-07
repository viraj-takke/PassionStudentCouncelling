using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Students_Councelling.Interface;
using Students_Councelling.Models.DAL;
using Students_Councelling.Models.DTO;
using Students_Councelling.Models.Viewmodels;
using Microsoft.EntityFrameworkCore;

namespace Students_Councelling.Models.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;
        public AccountRepository(ApplicationDbContext context) 
        { 
            _context = context;
        }
        public async Task<Students> LoginAsync(string email, string password)
        {
            var student = await _context.students
                .Where(s => s.Email == email && s.Password == password)
                .FirstOrDefaultAsync();

            return student;
        }

        public async Task LogoutAsync()
        {
            //await _signInManager.SignOutAsync();
        }
    }
}
