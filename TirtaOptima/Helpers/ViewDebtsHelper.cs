using TirtaOptima.Models;

namespace TirtaOptima.Helpers
{
    public static class ViewDebtsHelper
    {
        public static List<VDebt> GetViewDebts(DatabaseContext context) =>
            context.VDebts.ToList();

        public static void UpdateDebts(DatabaseContext context)
        {
            var vDebts = GetViewDebts(context);

            if (!vDebts.Any()) return;

            var pelangganIds = vDebts.Select(v => v.IdPelanggan).ToList();

            var existingDebts = context.Debts
                .Where(d => pelangganIds.Contains(d.PelangganId))
                .ToDictionary(d => d.PelangganId);

            foreach (var v in vDebts)
            {
                if (existingDebts.TryGetValue(v.IdPelanggan ?? 0, out var existing))
                {
                    if (existing.Nominal != v.TotalNominal ||
                        existing.TanggalTerakhir != v.TanggalTerakhir ||
                        existing.StatusTerakhir != v.StatusTerakhir)
                    {
                        existing.Nominal = v.TotalNominal ?? 0;
                        existing.TanggalTerakhir = v.TanggalTerakhir;
                        existing.StatusTerakhir = v.StatusTerakhir;
                        existing.UpdatedAt = DateTime.Now;
                    }
                }
                else
                {
                    context.Debts.Add(new Debt
                    {
                        PelangganId = v.IdPelanggan ?? 0,
                        Nominal = v.TotalNominal ?? 0,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        TanggalTerakhir = v.TanggalTerakhir,
                        StatusTerakhir = v.StatusTerakhir
                    });
                }
            }

            context.SaveChanges();
        }
    }
}

