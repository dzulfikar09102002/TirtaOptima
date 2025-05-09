using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
	public class LeaderViewModel
	{
		public List<Leader> Leaders { get; set; } = new ();
		public long Id { get; set; }

		public long UserId { get; set; }

		public string Signature { get; set; } = null!;

		public DateTime? TanggalAwal { get; set; }

		public DateTime? TanggalAkhir { get; set; }

		public IFormFile? Img { get; set; }
		public virtual User User { get; set; } = null!;

	}
}
