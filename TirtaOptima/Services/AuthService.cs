using Microsoft.EntityFrameworkCore;
using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class AuthService
    {
        private readonly DatabaseContext _context;

        public AuthService(DatabaseContext context) => _context = context;

        public AuthService() => _context = new DatabaseContext();

        public User? GetUser(string username) =>
            _context.Users.Include(role => role.Role).FirstOrDefault(user => user.Username == username);

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }

}
