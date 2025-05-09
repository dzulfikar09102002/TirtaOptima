using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class StatusCodesViewModel
    {
        public List<Status> Statuses { get; set; } = new();
        public long Id { get; set; }
        public string? Name { get; set; }

    }
}
