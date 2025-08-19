using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class LetterDeliveriesViewModel
    {
        public int BulanSelect { get; set; }
        public int TahunSelect { get; set; }
        public List<Collection> Collections { get; set; } = new();
        public List<Letter> Letters { get; set; } = new();
        public Collection? Collection { get; set; }
        public Letter? Letter { get; set; }
        public string? Status {  get; set; }
        public IFormFile? Img { get; set; }
    }
}
