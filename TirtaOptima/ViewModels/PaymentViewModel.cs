using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class PaymentViewModel
    {

        public int BulanSelect { get; set; }

        public int TahunSelect { get; set; }
        public List<Payment> Payments { get; set; } = new();
        public List<Customer> Customers { get; set; } = new();
    }

    public class PaymentColumn
    {
        [JsonPropertyName("idPelanggan")]
        public long IdPelanggan { get; set; }

        [JsonPropertyName("pembayaran1")]
        public decimal NominalBayar { get; set; }

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
