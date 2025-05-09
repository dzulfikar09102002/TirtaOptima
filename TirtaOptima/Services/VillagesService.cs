using Microsoft.EntityFrameworkCore;
using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class VillagesService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Village> GetVillages() => [.. _context.Villages.Include(x => x.KodeKecNavigation).Where(x => x.DeletedAt == null)];
        public List<District> GetDistricts() => [.._context.Districts.Where(x => x.DeletedAt == null)]; 
        public Village? GetVillage(long id) => _context.Villages.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);

        public void Store(Village input, long userid)
        {
            var idcode = _context.Villages.Select(x => x.Id).Max() + 1;
            var finalId = int.Parse(input.KodeKec.ToString() + (idcode % 1000).ToString("D3"));

            _context.Villages.Add(new Village
            {
                Id = finalId,
                KodeDesa = finalId,
                KodeKec = input.KodeKec,
                Nama = input.Nama,
                Layanan = input.Layanan,
                Jarak = input.Jarak,
                CreatedAt = DateTime.Now,
                CreatedBy = userid,
                UpdatedAt = DateTime.Now,
                UpdatedBy = userid
            });
            
            _context.SaveChanges();
        }
        public void Update(Village input, long userid)
        {
            Village? village = GetVillage(input.Id);
            if (village != null)
            {
                village.Nama = input.Nama;
                village.Layanan = input.Layanan;
                village.Jarak = input.Jarak;
                village.UpdatedAt = DateTime.Now;
                village.UpdatedBy = userid;
                _context.SaveChanges();
            }
        }
        public void Delete(long id, long userid)
        {
            Village? village = GetVillage(id);
            if (village != null)
            {
                village.DeletedAt = DateTime.Now;
                village.DeletedBy = userid;
                _context.SaveChanges();
            }
        }
    }
}
