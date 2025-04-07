using Microsoft.EntityFrameworkCore;
using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class AuthService
    {
        public AuthService(DatabaseContext context) => _context = context;

        public AuthService()
        {
            _context = new DatabaseContext();
        }

        private readonly DatabaseContext _context;

        public User? GetUser(string username) => _context.Users.Include(role => role.Role).FirstOrDefault(user => user.Username == username);
    }
}
