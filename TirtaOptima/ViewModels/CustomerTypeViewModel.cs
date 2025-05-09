using System.ComponentModel.DataAnnotations;
using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class CustomerTypeViewModel
    {
        public List<CustomerType> CustomerTypes { get; set; } = new();
        [Required(ErrorMessage = "Jenis Pelanggan Harus Diisi")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Hanya Boleh Diisi Huruf dan Angka")]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "Deskripsi Pelanggan Harus Diisi")]
        public string Deskripsi { get; set; } = null!;

        [Required(ErrorMessage = "Minimal Pemakaian Harus Diisi")]
        public int? MinPakai { get; set; }

        [Required(ErrorMessage = " Tarif 1 Harus Diisi")]
        public int? Tarif1 { get; set; }

        public int? Tarif2 { get; set; }

        public int? Denda { get; set; }

        public int? Retribusi { get; set; }
    }
}
