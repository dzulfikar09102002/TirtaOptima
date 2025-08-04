using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class BillViewModel
    {
        public int BulanSelect { get; set; }
        public int TahunSelect { get; set; }
        public List<Bill> Bills { get; set; } = new();
        public List<Customer> Customers {  get; set; } = new();
          
    }
    public class BillColumn
    {
        [JsonPropertyName("idPelanggan")]
        public long IdPelanggan { get; set; }

        [JsonPropertyName("total")]
        public decimal? Total { get; set; }

        [JsonPropertyName("ket")]
        public string? Ket { get; set; }

        [JsonPropertyName("jatuh_tempo")]
        public DateOnly? JatuhTempo { get; set; }

        [JsonPropertyName("bulan")]
        public int? Bulan { get; set; }

        [JsonPropertyName("tahun")]
        public int? Tahun { get; set; }

        [JsonPropertyName("awal")]
        public int? Awal { get; set; }

        [JsonPropertyName("akhir")]
        public int? Akhir { get; set; }

        [JsonPropertyName("pakai")]
        public int? Pakai { get; set; }

        [JsonPropertyName("tagihan")]
        public int? Tagihan { get; set; }

        [JsonPropertyName("admin")]
        public int? Admin { get; set; }

        [JsonPropertyName("dpm")]
        public int? Dpm { get; set; }

        [JsonPropertyName("materai")]
        public int? Materai { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? CreatedBy { get; set; }

        public long? UpdatedBy { get; set; }

        public long? DeletedBy { get; set; }
    }
}