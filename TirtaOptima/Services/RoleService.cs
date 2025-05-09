using Microsoft.EntityFrameworkCore.Storage;
using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class RoleService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Role> GetRoles() => [.. _context.Roles.Where(x => x.DeletedAt == null)];
        public Role? GetRole(long id) => _context.Roles.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        public void Store(Role input, long userid)
        {
            _context.Roles.Add(new Role
            {
                Id = _context.Roles.Select(x => x.Id).Max() + 1,
                Name = input.Name,
                CreatedAt = DateTime.Now,
                CreatedBy = userid,
                UpdatedAt = DateTime.Now,
                UpdatedBy = userid
            });
            _context.SaveChanges();
        }
        public void Update(Role input, long userid)
        {
            Role? role = GetRole(input.Id);
            if (role != null)
            {
                role.Name = input.Name;
                role.UpdatedAt = DateTime.Now;
                role.UpdatedBy = userid;
                _context.SaveChanges();
            }
        }
        public void Delete(long id, long userid)
        {
            Role? role = GetRole(id);
            if (role != null)
            {
                role.DeletedAt = DateTime.Now;
                role.DeletedBy = userid;
                _context.SaveChanges();
            }
        }
    }

}
