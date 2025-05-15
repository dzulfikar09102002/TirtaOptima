using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class PoliciesService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Policy> GetPolicies() => [.. _context.Policies.Where(x => x.DeletedAt == null)];
        public Policy? GetPolicy(long id) => _context.Policies.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);

        public void Store(Policy input, long userid)
        {
            input.CreatedAt = DateTime.Now;
            input.UpdatedAt = DateTime.Now;
            input.CreatedBy = userid;
            input.UpdatedBy = userid;
            _context.Policies.Add(input);
            _context.SaveChanges();
        }
        public void Update(Policy input, long userid)
        {
            Policy? policy = GetPolicy(input.Id);
            if (policy != null)
            {
                policy.NamaStrategi = input.NamaStrategi;
                policy.Deskripsi = input.Deskripsi;
                policy.RentangWaktu = input.RentangWaktu;
                policy.Kondisi = input.Kondisi;
                policy.UpdatedAt = DateTime.Now;
                policy.UpdatedBy = userid;
                _context.SaveChanges();
            }
        }
        public void Delete(long id, long userid)
        {
            Policy? policy = GetPolicy(id);
            if (policy != null)
            {
                policy.DeletedAt = DateTime.Now;
                policy.DeletedBy = userid;
                _context.SaveChanges();
            }
        }
    }
}
