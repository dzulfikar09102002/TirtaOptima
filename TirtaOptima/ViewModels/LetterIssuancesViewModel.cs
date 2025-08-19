using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class LetterIssuancesViewModel
    {
        public int BulanSelect { get; set; }

        public int TahunSelect { get; set; }

        public List<Collection> Collections { get; set; } = new();

        public List<LetterCategory> LetterCategories { get; set; } = new();

        public List<Letter> Letters { get; set; } = new();
        public List<DebtsSaldoPerMonthDto> DebtsManagement { get; set; } = new();
        public Letter? Letter { get; set; }
        public Collection? Collection { get; set; }

        public List<Leader> Leaders { get; set; } = new();

        public long PimpinanId { get; set; }

        public long KategoriId { get; set; }

        public string? Sifat { get; set; }

        public int? Lampiran { get; set; }

        public string? Ket { get; set; }

        public string? Note { get; set; }
    }
    public class DebtsSaldoPerMonthDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalKredit { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal Sisa => TotalKredit - TotalDebit;  
    }
}
