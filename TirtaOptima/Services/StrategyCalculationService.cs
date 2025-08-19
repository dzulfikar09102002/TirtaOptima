using Microsoft.EntityFrameworkCore;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class StrategyCalculationService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public CriteriaComparison? GetCriteriaComparison(long id1, long id2) => _context.CriteriaComparisons
            .Include(x => x.CriteriaId1Navigation)
            .Include(x => x.CriteriaId2Navigation)
            .FirstOrDefault(x => x.CriteriaId1 == id1 && x.CriteriaId2 == id2);
        public List<Criteria> GetCriterias() => [.._context.Criterias
            .Where(c => c.Bobot != null)
            .OrderByDescending(c => c.Bobot)
            .ToList()];
        public List<FuzzyComparisonViewModel> GetFuzzyComparisons()
        {
            var kriteriaList = _context.Criterias.ToList();
            var result = new List<FuzzyComparisonViewModel>();

            for (int i = 0; i < kriteriaList.Count; i++)
            {
                for (int j = 0; j < kriteriaList.Count; j++)
                {
                    if (i <= j)
                    {
                        var comparison = GetCriteriaComparison(kriteriaList[i].Id, kriteriaList[j].Id);

                        result.Add(new FuzzyComparisonViewModel
                        {
                            Kriteria1Id = kriteriaList[i].Id,
                            Kriteria2Id = kriteriaList[j].Id,
                            NamaKriteria1 = kriteriaList[i].Name,
                            NamaKriteria2 = kriteriaList[j].Name,
                            L = comparison?.FuzzyL ?? 1,
                            M = comparison?.FuzzyM ?? 1,
                            U = comparison?.FuzzyU ?? 1
                        });
                    }
                }
            }

            return result;
        }

        public void Save(CriteriaComparison comparison, bool repricoral)
        {
            var tfn = FuzzyAHPHelper.TFNScaleNumber(comparison.ScaleValue ?? 0);  // asli (L,M,U)

            // hitung reciprocal TFN secara benar (1/U, 1/M, 1/L)
            var reciprocalTFN = new TFNScale(
                l: 1 / tfn.U,
                m: 1 / tfn.M,
                u: 1 / tfn.L
            );

            if (repricoral)
            {
                // jika yang disimpan kebalik
                comparison.ScaleValue = 1 / comparison.ScaleValue;
                // tukar
                (tfn, reciprocalTFN) = (reciprocalTFN, tfn);
            }

            var existingComparison = GetCriteriaComparison(comparison.CriteriaId1, comparison.CriteriaId2);
            var repComparison = GetCriteriaComparison(comparison.CriteriaId2, comparison.CriteriaId1);

            if (existingComparison != null && repComparison != null)
            {
                // sisi utama
                existingComparison.ScaleValue = comparison.ScaleValue;
                existingComparison.FuzzyL = tfn.L;
                existingComparison.FuzzyM = tfn.M;
                existingComparison.FuzzyU = tfn.U;
                // sisi reciprocal
                repComparison.ScaleValue = 1 / comparison.ScaleValue;
                repComparison.FuzzyL = reciprocalTFN.L;
                repComparison.FuzzyM = reciprocalTFN.M;
                repComparison.FuzzyU = reciprocalTFN.U;
            }

            _context.SaveChanges();
        }

        public void Store(Dictionary<string, decimal> result, long userid)
        {
            foreach (var item in result)
            {
                // Ambil angka ID dari "C1", "C2", dst
                if (item.Key.StartsWith("C") && long.TryParse(item.Key.Substring(1), out long id))
                {
                    var criteria = _context.Criterias.FirstOrDefault(c => c.Id == id);

                    if (criteria != null)
                    {
                        criteria.Bobot = item.Value;
                        criteria.UpdatedAt = DateTime.Now;
                        criteria.UpdatedBy = userid;
                    }
                }
            }

            _context.SaveChanges();
        }
        public List<PsoLogViewModel> GetPsoLogSummary()
        {
            var criteriaList = _context.Criterias.OrderBy(c => c.Id).ToList();

            var logs = _context.PsoIterations
                .AsNoTracking()
                .OrderBy(p => p.Iteration)
                .GroupBy(p => p.Iteration)
                .Select(g => new PsoLogViewModel
                {
                    Iteration = g.Key,
                    Weights = g.OrderBy(x => x.CriteriaId).Select(x => x.Weight ?? 0).ToList(),
                    Velocity = g.OrderBy(x => x.CriteriaId).Select(x => x.Velocity ?? 0).ToList(),
                    PBest = g.OrderBy(x => x.CriteriaId).Select(x => x.Pbest ?? 0).ToList(),
                    Fitness = g.Max(x => x.Fitness ?? 0)
                })
                .ToList();

            return logs;
        }
        public List<NormalisasiDetailViewModel> GetNormalisasiDetails()
        {
            return (from n in _context.CriteriaNormalizations
                    join c in _context.Criterias on n.CriteriaId equals c.Id
                    orderby c.Id
                    select new NormalisasiDetailViewModel
                    {
                        NamaKriteria = c.Name ?? $"Kriteria {c.Id}",
                        GeomL = n.GeomL ?? 0,
                        GeomM = n.GeomM ?? 0,
                        GeomU = n.GeomU ?? 0,
                        Normalized = n.NormalizationResult ?? 0
                    }).ToList();
        }


    }
}
