using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class CriteriaService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Criteria> GetCriterias() => [.. _context.Criterias.Where(x => x.DeletedAt == null)];
        public Criteria? GetCriteria(long id) => _context.Criterias.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        public void Store(Criteria input, long userid)
        {
            _context.Criterias.Add(
                new Criteria
                {
                    Id = _context.Criterias.Select(x => x.Id).Max() + 1,
                    Name = input.Name,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    CreatedBy = userid,
                    UpdatedBy  = userid
                });
            _context.SaveChanges();
        }
        public void Update(Criteria input, long userid)
        {
            var existing = _context.Criterias.FirstOrDefault(x => x.Id == input.Id);
            if (existing != null)
            {
                existing.Name = input.Name;
                existing.UpdatedAt = DateTime.Now;
                existing.UpdatedBy = userid;
                _context.SaveChanges();
            }
        }
        public void Delete (long id, long userid)
        {
            var existing = _context.Criterias.FirstOrDefault(x => x.Id == id);
            if (existing != null)
            {
                existing.DeletedAt = DateTime.Now;
                existing.DeletedBy = userid;
                _context.SaveChanges();
            }
        }
    }
}
