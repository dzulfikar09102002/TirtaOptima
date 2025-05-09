using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class DistrictsViewModel
    {
        public List<District> Districts { get; set; } = new();
        public long Id { get; set; }
        public long KodeKec { get; set; }
        public string? Nama { get; set; }
        public List<Village> Villages { get; set; } = new();
    }
}
