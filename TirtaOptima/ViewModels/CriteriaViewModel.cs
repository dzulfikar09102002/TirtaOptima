using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class CriteriaViewModel
    {
        public List<Criteria> Criterias { get; set; } = new();
        public long Id { get; set; }
        public string? Name { get; set; }
    }
}
