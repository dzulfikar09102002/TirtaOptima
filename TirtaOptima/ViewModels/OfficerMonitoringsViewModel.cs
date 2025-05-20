using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class OfficerMonitoringsViewModel
    {
        public int BulanSelect { get; set; }
        public int TahunSelect { get; set; }
        public long IdPenagih {  get; set; }
        public List<User> Officers { get; set; } = new();
        public List<Collection> Collections { get; set; } = new();
    }
}
