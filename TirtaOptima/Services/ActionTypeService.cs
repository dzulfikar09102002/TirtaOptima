using Microsoft.EntityFrameworkCore.Storage;
using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class ActionTypeService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<ActionType> GetActions() => [.. _context.ActionTypes.Where(x => x.DeletedAt == null)];
        public ActionType? GetAction(long id)
        {
            return _context.ActionTypes.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        }
        public void Store(ActionType input, long userid)
        {
            _context.ActionTypes.Add(new ActionType
            {
                Id = _context.ActionTypes.Select(x => x.Id).Max() + 1,
                Name = input.Name,
                CreatedAt = DateTime.Now,
                CreatedBy = userid,
                UpdatedAt = DateTime.Now,
                UpdatedBy = userid
            });
            _context.SaveChanges();
        }
        public void Update(ActionType input, long userid)
        {
            ActionType? action = GetAction(input.Id);
            if (action != null)
            {
                action.Name = input.Name;
                action.UpdatedAt = DateTime.Now;
                action.UpdatedBy = userid;
                _context.SaveChanges();
            }
        }
        public void Delete(long id, long userid)
        {
            ActionType? action = GetAction(id);
            if (action != null)
            {
                action.DeletedAt = DateTime.Now;
                action.DeletedBy = userid;
                _context.SaveChanges();
            }
        }
    }
}
