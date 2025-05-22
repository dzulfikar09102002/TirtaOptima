namespace TirtaOptima.Models
{
    public class VDebts
    {
        public int PiutangId { get; set; }
        public int PelangganId { get; set; }
        public int Rekening { get; set; }
        public decimal TotalNominal { get; set; }
        public string? StatusTerakhir { get; set; }
        public DateTime? TanggalTerakhir { get; set; }
    }
}
