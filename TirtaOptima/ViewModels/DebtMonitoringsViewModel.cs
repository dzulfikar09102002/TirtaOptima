using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class DebtMonitoringsViewModel
    {
        public List<DebtColumn> Debts { get; set; } = new();
        public List<Policy> Policies { get; set; } = new();
        public int BulanSelect { get; set; }
        public int TahunSelect { get; set; }

    }
    public class DebtColumn
    {
        public long IdPelanggan { get; set; }
        public string? NamaPelanggan { get; set; }
        public string? NomorPelanggan { get; set; }
        public string? JenisPelanggan { get; set; }
        public string? Alamat { get; set; }
        public string? Kelurahan { get; set; }
        public string? Kecamatan { get; set; }
        public string? Periode { get; set; }
        public decimal? TotalNominal { get; set; }
        public string? Status { get; set; }
        public string? Tindakan { get; set; }
        public int RentangWaktu {  get; set; }
    }
}
