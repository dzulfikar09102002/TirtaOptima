using Microsoft.EntityFrameworkCore;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
	public class LeaderService(DatabaseContext context)
	{
		private readonly DatabaseContext _context = context;
		public List<Leader> GetLeaders() => [.. _context.Leaders.Include(x => x.User).Where(x => x.DeletedAt == null)];
		public Leader? GetLeader(long id) => _context.Leaders.Include(x => x.User).FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
		public void Update(LeaderViewModel leader, long userid)
		{
			Leader? checkleader = _context.Leaders.FirstOrDefault(x => x.Id == leader.Id);
			if(checkleader != null)
			{
				checkleader.TanggalAwal = leader.TanggalAwal;
				checkleader.TanggalAkhir = leader.TanggalAkhir;
				checkleader.Signature = leader.Signature;
				checkleader.UpdatedAt = DateTime.Now;
				checkleader.UpdatedBy = userid;
				_context.SaveChanges();
			}
		}
	}
}
