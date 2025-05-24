using TirtaOptima.Models;

namespace TirtaOptima.Services
{
    public class ViewDebtsService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<VDebts> GetViewDebts() => [.. _context.VDebts];
        public void UpdateDebts()
        {
            var vDebts = GetViewDebts();

            if (!vDebts.Any()) return;

            var pelangganIds = vDebts.Select(v => v.PelangganId).ToList();

            var existingDebts = _context.Debts
                .Where(d => pelangganIds.Contains(d.PelangganId))
                .ToDictionary(d => d.PelangganId);

            foreach (var v in vDebts)
            {
                if (existingDebts.TryGetValue(v.PelangganId, out var existing))
                {
                    if (existing.Nominal != v.TotalNominal || existing.Rekening != v.Rekening)
                    {
                        existing.Nominal = v.TotalNominal;
                        existing.Rekening = v.Rekening;
                        existing.TanggalTerakhir = v.TanggalTerakhir;
                        existing.StatusTerakhir = v.StatusTerakhir;
                        existing.UpdatedAt = DateTime.Now;
                    }
                }
                else
                {
                    _context.Debts.Add(new Debt
                    {
                        PelangganId = v.PelangganId,
                        Nominal = v.TotalNominal,
                        Rekening = v.Rekening,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        TanggalTerakhir= v.TanggalTerakhir,
                        StatusTerakhir = v.StatusTerakhir
                    });
                }
            }

            _context.SaveChanges();
        }
    }
}
