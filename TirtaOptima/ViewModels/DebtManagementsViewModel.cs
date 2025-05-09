using System.ComponentModel.DataAnnotations;
using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class DebtManagementsViewModel
    {
        public int BulanSelect { get; set; }
        public int TahunSelect { get; set; }
        public long IdPelanggan { get; set; }
        public DateOnly? Pencatatan {  get; set; }
        public DateOnly? Pembayaran { get; set; }
        public decimal Nominal {  get; set; }
        public string? Status { get; set; }
        public string? Periode { get; set; }
        public List<Bill> Bills { get; set; } = new();
        public List<Payment> Payments { get; set; } = new();
        public List<DebtSummaryViewModel> DebtManagements { get; set; } = new();
    }
    public class DebtSummaryViewModel
    {
        public long Id {  get; set; }
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
        public DateTime? Pencatatan { get; set; }
        public DateOnly? Pembayaran { get; set; }
    }
}
