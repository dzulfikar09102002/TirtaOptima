using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class LettersCategoryViewModel
    {
        public List<LetterCategory> LetterCategories { get; set; } = new();
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }

    }
}
