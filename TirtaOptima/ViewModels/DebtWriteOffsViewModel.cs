using System.ComponentModel.DataAnnotations;

namespace TirtaOptima.ViewModels
{
	public class DebtWriteOffsViewModel
	{
		public List<DebtSummaryViewModel> Debts { get; set; } = new();
		public int BulanSelect { get; set; }
		public int TahunSelect { get; set; }
		public long IdPelanggan { get; set; }
		public long IdPiutang { get; set; }
		public decimal Nominal {  get; set; }
	}
}
