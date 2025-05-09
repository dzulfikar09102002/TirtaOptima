using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class CustomerViewModel
    {
        [Required(ErrorMessage ="Harap Pilih Bulan")]
        public int BulanSelect { get; set; }

        [Required(ErrorMessage ="Harap Pilih Tahun")]
        public int TahunSelect { get; set; }
        public List<Customer> Customers { get; set; } = new();

    }
    public class CustomerColumn
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("noPelanggan")]
        public string Nomor { get; set; } = null!;

        [JsonPropertyName("namaPelanggan")]
        public string Nama { get; set; } = null!;

        [JsonPropertyName("jenisPelanggan")]
        public string? Jenis { get; set; }

        [JsonPropertyName("alamat")]
        public string? Alamat { get; set; }

        [JsonPropertyName("tanggalPasang")]
        public DateTime? Pasang { get; set; }

        [JsonPropertyName("kodeKec")]
        public long? Kecamatan { get; set; }

        [JsonPropertyName("kodeDesa")]
        public long? Kelurahan { get; set; }

        [JsonPropertyName("status")]
        public long Status { get; set; }

        [JsonPropertyName("noCabang")]
        public long? Cabang { get; set; }

        [JsonPropertyName("noWilayah")]
        public long? Wilayah { get; set; }

        [JsonPropertyName("noTelepon")]
        public string? NoTelepon { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? CreatedBy { get; set; }

        public long? UpdatedBy { get; set; }

        public long? DeletedBy { get; set; }
        public virtual CustomerType? JenisNavigation { get; set; }

        public virtual District? KecamatanNavigation { get; set; }

        public virtual Village? KelurahanNavigation { get; set; }

        public virtual Status? StatusNavigation { get; set; } = null!;


    }
}
