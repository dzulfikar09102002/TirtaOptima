using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class LettersCategoryService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<LetterCategory> GetLetterCategories() => [.._context.LetterCategories.Where(x => x.DeletedAt == null)];
        public LetterCategory? GetLetterCategory(long id) => _context.LetterCategories.FirstOrDefault(x => x.Id == id && x.DeletedAt ==null);
        public void Store(LetterCategory input, long userid)
        {
            input.Id = _context.LetterCategories.Select(x => x.Id).Max() + 1;
            input.CreatedAt = DateTime.Now;
            input.CreatedBy = userid;
            input.UpdatedAt = DateTime.Now;
            input.UpdatedBy = userid;
            _context.LetterCategories.Add(input);
            _context.SaveChanges();
        }
        public void Update(LetterCategory input, long userid)
        {
            LetterCategory? letterCategory = GetLetterCategory(input.Id);
            if (letterCategory != null)
            {
                letterCategory.Code = input.Code;
                letterCategory.Name = input.Name;
                letterCategory.UpdatedAt = DateTime.Now;
                letterCategory.UpdatedBy = userid;
                _context.SaveChanges();
            }
        }
        public void Delete(long id, long userid)
        {
            LetterCategory? letterCategory = GetLetterCategory(id);
            if (letterCategory != null)
            {
                letterCategory.DeletedAt = DateTime.Now;
                letterCategory.DeletedBy = userid;
                _context.SaveChanges();
            }
        }

    }
}
