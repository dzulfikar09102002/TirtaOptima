using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class CollectionMonitoringsViewModel
    {
        public int BulanSelect { get; set; }
        public int TahunSelect { get; set; }
        public List<Collection> Collections { get; set; } = new();
        public List<Letter> Letters { get; set; } = new();
        public Letter? Letter { get; set; } = new();
    }
}
