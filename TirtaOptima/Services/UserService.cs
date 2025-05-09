using Microsoft.EntityFrameworkCore;
using TirtaOptima.Helpers;
using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class UserService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<User> GetUsers() => [.. _context.Users.Where(x => x.DeletedAt == null).Include(x => x.Role)];
        public User? GetUser(long id) => _context.Users.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        public List<Role> GetRoles() => [.. _context.Roles.Where(x => x.DeletedAt == null)];
        public void Store(User user, long userid)
        {
            user.Id = _context.Users.Select(x => x.Id).Max() + 1;
            user.CreatedAt = DateTime.Now;
            user.CreatedBy = userid;
            user.UpdatedAt = DateTime.Now;
            user.CreatedBy = userid;
            user.Password = EncryptHelper.GeneratedPassword(user.Username ?? "Default", user.Password ?? "Default");
            user.Status = true;
            _context.Users.Add(user);
            _context.SaveChanges();
            if (user.RoleId == 4)
            {
                SetLeader(user.Id, userid);
            }
        }
        public void SetLeader(long id, long userid)
        {
            var checkuser = _context.Users.FirstOrDefault(x => x.Id == id);
            if (checkuser != null && checkuser.RoleId != 4)
            {
                checkuser.RoleId = 4;
            }
            _context.Leaders.Add(new Leader
            {
                Id = _context.Leaders.Select(x => x.Id).Max() + 1,
                UserId = id,
                CreatedBy = userid,
                CreatedAt = DateTime.Now,
                UpdatedBy = userid,
                UpdatedAt = DateTime.Now,
            });
            _context.SaveChanges();
        }

        public void Update(User user, long userid)
        {
            var checkuser = _context.Users.FirstOrDefault(x => x.Id == user.Id);
            if (checkuser != null)
            {
                if (!string.IsNullOrEmpty(user.Password))
                {
                    checkuser.Password = user.Password;
                }
                if (!string.IsNullOrEmpty(user.Photo))
                {
                    checkuser.Photo = user.Photo;
                }
                checkuser.NikNip = user.NikNip;
                checkuser.Email = user.Email;
                checkuser.RoleId = user.RoleId;
                checkuser.Jabatan = user.Jabatan;
                checkuser.Gender = user.Gender;
                checkuser.Name = user.Name;
                checkuser.Username = user.Username;
                checkuser.Alamat = user.Alamat;
                checkuser.More = user.More;
                checkuser.NomorTelepon = user.NomorTelepon;
                checkuser.UpdatedAt = DateTime.Now;
                checkuser.UpdatedBy = userid;
                if (checkuser.RoleId == 4)
                {
                    SetLeader(user.Id, userid);
                }
                if (checkuser.RoleId != 4)
                {
                    var leader = _context.Leaders.FirstOrDefault(x => x.UserId == user.Id);
                    if (leader != null)
                    {
                        leader.DeletedAt = DateTime.Now;
                        leader.DeletedBy = userid;
                    }
                }
                _context.SaveChanges();
            }
        }
        public void Delete(long id, long userid)
        {
            User? user = GetUser(id);
            if (user != null)
            {
                user.DeletedAt = DateTime.Now;
                user.DeletedBy = userid;
                if (user.RoleId == 4)
                {
                    var leader = _context.Leaders.FirstOrDefault(x => x.UserId == id);
                    if (leader != null)
                    {
                        leader.DeletedAt = DateTime.Now;
                        leader.DeletedBy = userid;
                    }
                }
                _context.SaveChanges();
            }
        }
    }
}
