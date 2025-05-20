using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class CollectionMonitoringsViewModel
    {
        public int BulanSelect { get; set; }
        public int TahunSelect { get; set; }
        public List<Collection> Collections { get; set; } = new();
        public Collection? Collection { get; set; }
    }
}
