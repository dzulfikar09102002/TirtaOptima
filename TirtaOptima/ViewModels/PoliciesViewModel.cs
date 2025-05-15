using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class PoliciesViewModel
    {
        public List<Policy> Policies { get; set; } = new();
        public long Id { get; set; }
        public string NamaStrategi { get; set; } = null!;

        public string Deskripsi { get; set; } = null!;

        public string Kondisi { get; set; } = null!;

        public int? RentangWaktu { get; set; } 

    }
}
