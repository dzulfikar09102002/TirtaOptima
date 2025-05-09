using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class StatusCodesService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Status> GetStatuses() => [.. _context.Statuses.Where(x => x.DeletedAt == null)];
        public Status? GetStatus(long id) => _context.Statuses.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        public void Store(Status input, long userid)
        {
            _context.Statuses.Add(new Status
            {
                Id = _context.Statuses.Select(x => x.Id).Max() + 1,
                Name = input.Name,
                CreatedAt = DateTime.Now,
                CreatedBy = userid,
                UpdatedAt = DateTime.Now,
                UpdatedBy = userid
            });
            _context.SaveChanges();
        }
        public void Update(Status input, long userid)
        {
            Status? status = GetStatus(input.Id);
            if (status != null)
            {
                status.Name = input.Name;
                status.UpdatedAt = DateTime.Now;
                status.UpdatedBy = userid;
                _context.SaveChanges();
            }
        }
        public void Delete(long id, long userid)
        {
            Status? status = GetStatus(id);
            if (status != null)
            {
                status.DeletedAt = DateTime.Now;
                status.DeletedBy = userid;
                _context.SaveChanges();
            }
        }
    }
}
