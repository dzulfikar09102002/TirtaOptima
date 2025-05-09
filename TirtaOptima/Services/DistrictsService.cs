using Microsoft.EntityFrameworkCore;
using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class DistrictsService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<District> GetDistricts() => [.._context.Districts.Where(x => x.DeletedAt == null)];
        public District? GetDistrict(long id) => _context.Districts.Include(x => x.Villages).FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        public void Store(District input, long userid)
        {
            var idcode = _context.Districts.Select(x => x.Id).Max() + 1;
            _context.Districts.Add(new District
            {
                Id = idcode,
                KodeKec = idcode,
                Nama = input.Nama,
                CreatedAt = DateTime.Now,
                CreatedBy = userid,
                UpdatedAt = DateTime.Now,
                UpdatedBy = userid
            });
            _context.SaveChanges();
        }
        public void Update(District input, long userid)
        {
            District? district = GetDistrict(input.Id);
            if (district != null)
            {
                district.Nama = input.Nama;
                district.UpdatedAt = DateTime.Now;
                district.UpdatedBy = userid;
                _context.SaveChanges();
            }
        }
        public void Delete(long id, long userid)
        {
            District? district = GetDistrict(id);
            if (district != null)
            {
                district.DeletedAt = DateTime.Now;
                district.DeletedBy = userid;
                _context.SaveChanges();
            }
        }
    }
}
