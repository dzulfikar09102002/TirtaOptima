using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class PaymentViewModel
    {
        [Required(ErrorMessage = "Harap Pilih Bulan")]
        public int BulanSelect { get; set; }

        [Required(ErrorMessage = "Harap Pilih Tahun")]
        public int TahunSelect { get; set; }
        public List<Payment> Payments { get; set; } = new();
    }

    public class PaymentColumn
    {
        [JsonPropertyName("idPelanggan")]
        public long IdPelanggan { get; set; }

        [JsonPropertyName("pembayaran1")]
        public long NominalBayar { get; set; }

        [JsonPropertyName("ket")]
        public string? Ket { get; set; }

        [JsonPropertyName("tanggalBayar")]
        public DateOnly? TanggalBayar { get; set; }

        [JsonPropertyName("kasir")]
        public string? Kasir { get; set; }

        [JsonPropertyName("bulan")]
        public int? Bulan { get; set; }

        [JsonPropertyName("tahun")]
        public int? Tahun { get; set; }
    }
}
