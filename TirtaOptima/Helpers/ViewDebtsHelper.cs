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

            var pelangganIds = vDebts.Select(v => v.PelangganId).ToList();

            var existingDebts = context.Debts
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
                    context.Debts.Add(new Debt
                    {
                        PelangganId = v.PelangganId,
                        Nominal = v.TotalNominal,
                        Rekening = v.Rekening,
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

