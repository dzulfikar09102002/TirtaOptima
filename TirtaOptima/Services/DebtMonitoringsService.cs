using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
	public class DebtMonitoringsService(DatabaseContext context)
	{
		private readonly DatabaseContext _context = context;
		public List<Policy> GetPolicies() => [.. _context.Policies.Where(x => x.DeletedAt == null)];
		public List<Debt> GetDebts(DebtMonitoringsViewModel input)
		{
            ViewDebtsHelper.UpdateDebts(_context);
            return [.._context.Debts.
				Where(x => x.DeletedAt == null && x.TanggalTerakhir!.Value.Month == input.BulanSelect
				&& x.TanggalTerakhir!.Value.Year == input.TahunSelect)
            .Include(x => x.Pelanggan).ThenInclude(x => x.JenisNavigation)
            .Include(x => x.Pelanggan).ThenInclude(x => x.KelurahanNavigation!).ThenInclude(x => x.KodeKecNavigation)
            .Include(x => x.Collections)] ;
		}
	}
}
