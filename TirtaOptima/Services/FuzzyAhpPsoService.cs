using Microsoft.EntityFrameworkCore;
using TirtaOptima.Interfaces;
using TirtaOptima.Models;
using TirtaOptima.Requests;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class FuzzyAhpPsoService : IFuzzyAhpPsoService
    {
        private readonly DatabaseContext _context;
        private readonly string _option;
        private List<int> _fahpRanking = new();
        public List<NormalisasiDetailViewModel> NormalisasiLog { get; set; } = new();
        public List<PsoLogViewModel> PsoIterationsLog { get; set; } = new();
        public FuzzyAhpPsoService(DatabaseContext context, string option)
        {
            _context = context;
            _option = option;
        }

        public async Task<List<decimal>> CalculateWeightsWithValidationAsync(FuzzyComparisonRequest request)
        {
            var fahpWeights = CalculateFahpWeights(request);

            _fahpRanking = fahpWeights
                .Select((v, i) => new { Index = i, Value = v })
                .OrderByDescending(x => x.Value)
                .Select(x => x.Index)
                .ToList();

            List<decimal> bestWeights = new();
            decimal bestFitness = decimal.MinValue;
            decimal bestSpearman = -1m;

            int attempts = 0;
            const int maxAttempts = 5;

            while (attempts < maxAttempts)
            {
                var psoWeights = await CalculateWeightsAsync(request);

                // hitung fitnessnya SEKALI LAGI menggunakan matriks asli PSO
                var fitness = Fitness(
                    psoWeights.ToArray(),
                    /* kita ambil matrix dari request -> diisi di method Fitness */
                    CreateLowerMatrix(request),
                    CreateMiddleMatrix(request),
                    CreateUpperMatrix(request),
                    request.CriteriaCount
                );

                var spearman = CalculateSpearmanCorrelation(fahpWeights, psoWeights);

                if (fitness > bestFitness || (fitness == bestFitness && spearman > bestSpearman))
                {
                    bestWeights = psoWeights;
                    bestFitness = fitness;
                    bestSpearman = spearman;
                }

                attempts++;
            }

            SavePsoLogs();
            SaveNormalizations();

            // Konvergensi sudah ditemukan di dalam metode CalculateWeightsAsync
            // Jadi, tidak perlu melakukan pencarian lagi di sini

            // Tampilkan hanya hasil ringkas
            Console.WriteLine($"Spearman rho: {bestSpearman:F4}");

            // Jika ingin mengembalikan bestWeights tetap dipakai UI
            return bestWeights;
        }


        public class FuzzyWeight
        {
            public decimal L { get; set; }
            public decimal M { get; set; }
            public decimal U { get; set; }
        }

        public List<decimal> CalculateFahpWeights(FuzzyComparisonRequest request)
        {
            int n = request.CriteriaCount;
            var comparisons = request.Comparisons;

            decimal[] geomL = new decimal[n];
            decimal[] geomM = new decimal[n];
            decimal[] geomU = new decimal[n];

            // Hitung G-mean tiap kriteria
            for (int i = 0; i < n; i++)
            {
                decimal prodL = 1, prodM = 1, prodU = 1;
                for (int j = 0; j < n; j++)
                {
                    var item = comparisons.FirstOrDefault(c => c.Kriteria1Id == i && c.Kriteria2Id == j);

                    if (item == null)
                    {
                        if (i == j)
                            item = new FuzzyComparisonViewModel { L = 1, M = 1, U = 1 };
                        else
                        {
                            var reverse = comparisons.FirstOrDefault(c => c.Kriteria1Id == j && c.Kriteria2Id == i);
                            if (reverse != null)
                                item = new FuzzyComparisonViewModel
                                {
                                    L = 1 / reverse.U,
                                    M = 1 / reverse.M,
                                    U = 1 / reverse.L
                                };
                        }
                    }

                    prodL *= item?.L ?? 1;
                    prodM *= item?.M ?? 1;
                    prodU *= item?.U ?? 1;
                }

                geomL[i] = (decimal)Math.Pow((double)prodL, 1.0 / n);
                geomM[i] = (decimal)Math.Pow((double)prodM, 1.0 / n);
                geomU[i] = (decimal)Math.Pow((double)prodU, 1.0 / n);
            }

            // Total untuk normalisasi
            decimal sumL = geomL.Sum();
            decimal sumM = geomM.Sum();
            decimal sumU = geomU.Sum();

            var result = new List<decimal>();
            NormalisasiLog.Clear();

            for (int i = 0; i < n; i++)
            {
                decimal normalized = (geomL[i] / sumL + geomM[i] / sumM + geomU[i] / sumU) / 3m;
                result.Add(normalized);

                NormalisasiLog.Add(new NormalisasiDetailViewModel
                {
                    NamaKriteria = $"C{i + 1}",
                    GeomL = geomL[i],
                    GeomM = geomM[i],
                    GeomU = geomU[i],
                    NormalizedL = geomL[i] / sumL,
                    NormalizedM = geomM[i] / sumM,
                    NormalizedU = geomU[i] / sumU
                });
            }

            return result;
        }

        public Task<List<decimal>> CalculateWeightsAsync(FuzzyComparisonRequest request)
        {
            int n = request.CriteriaCount;
            var comparisons = request.Comparisons;

            decimal[,] lowerMatrix = new decimal[n, n];
            decimal[,] middleMatrix = new decimal[n, n];
            decimal[,] upperMatrix = new decimal[n, n];

            foreach (var item in comparisons)
            {
                long i = item.Kriteria1Id;
                long j = item.Kriteria2Id;

                lowerMatrix[i, j] = item.L;
                middleMatrix[i, j] = item.M;
                upperMatrix[i, j] = item.U;

                lowerMatrix[j, i] = 1 / item.U;
                middleMatrix[j, i] = 1 / item.M;
                upperMatrix[j, i] = 1 / item.L;
            }
            int particleCount = 100;
            int maxIter = 100;

            var rand = new Random();
            var particles = new List<decimal[]>();
            var velocities = new List<decimal[]>();
            var pBest = new List<decimal[]>();
            var pBestScore = new List<decimal>();
            decimal[] gBest = new decimal[n];
            decimal gBestScore = decimal.MinValue;
            decimal previousGBestScore = decimal.MinValue;

            for (int p = 0; p < particleCount; p++)
            {
                var w = RandomWeights(n, rand);
                var v = new decimal[n];
                particles.Add(w);
                velocities.Add(v);
                pBest.Add((decimal[])w.Clone());
                var score = Fitness(w, lowerMatrix, middleMatrix, upperMatrix, n);
                pBestScore.Add(score);
                if (score > gBestScore)
                {
                    gBestScore = score;
                    gBest = (decimal[])w.Clone();
                }
            }

            decimal c1 = 1.0m, c2 = 1.0m, wInertia = 0.2m;
            PsoIterationsLog.Clear();

            for (int iter = 0; iter < maxIter; iter++)
            {
                for (int p = 0; p < particleCount; p++)
                {
                    var w = particles[p];
                    var v = velocities[p];

                    for (int d = 0; d < n; d++)
                    {
                        decimal r1 = (decimal)rand.NextDouble();
                        decimal r2 = (decimal)rand.NextDouble();

                        v[d] = wInertia * v[d]
                             + c1 * r1 * (pBest[p][d] - w[d])
                             + c2 * r2 * (gBest[d] - w[d]);

                        w[d] += v[d];
                        if (w[d] < 0.01m) w[d] = 0.01m;
                    }

                    var total = w.Sum();
                    for (int d = 0; d < n; d++)
                        w[d] /= total;

                    var score = Fitness(w, lowerMatrix, middleMatrix, upperMatrix, n);
                    if (score > pBestScore[p])
                    {
                        pBestScore[p] = score;
                        pBest[p] = (decimal[])w.Clone();
                    }
                    if (score > gBestScore)
                    {
                        gBestScore = score;
                        gBest = (decimal[])w.Clone();
                    }

                    PsoIterationsLog.Add(new PsoLogViewModel
                    {
                        Iteration = iter + 1,
                        Weights = w.ToList(),
                        Velocity = v.ToList(),
                        PBest = pBest[p].ToList(),
                        PBestScore = pBestScore[p],
                        Fitness = score
                    });
                }

                // Kondisi penghentian yang BENAR-BENAR menghentikan loop iterasi
                if (Math.Round(gBestScore, 6) == Math.Round(previousGBestScore, 6) && iter > 0)
                {
                    Console.WriteLine($"Konvergen di iterasi: {iter + 1}");
                    break; // Ini akan menghentikan seluruh proses
                }

                previousGBestScore = gBestScore;
            }

            return Task.FromResult(gBest.ToList());
        }
        private decimal[] RandomWeights(int n, Random rand)
        {
            var raw = new decimal[n];
            for (int i = 0; i < n; i++)
            {
                raw[i] = (decimal)(rand.NextDouble() + 0.01);
            }

            var total = raw.Sum();
            for (int i = 0; i < n; i++)
            {
                raw[i] /= total;
            }

            return raw;
        }

        private decimal Fitness(decimal[] w, decimal[,] L, decimal[,] M, decimal[,] U, int n)
        {
            decimal cost = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j || w[j] <= 0.000001m) continue;

                    decimal ratio = w[i] / w[j];
                    decimal lij = L[i, j];
                    decimal mij = M[i, j];
                    decimal uij = U[i, j];
                    decimal mu = 0;

                    if (ratio <= mij)
                    {
                        mu = (mij == lij) ? 1 : (mij - ratio) / (mij - lij);
                    }
                    else
                    {
                        mu = (uij == mij) ? 1 : (ratio - mij) / (uij - mij);
                    }

                    if (mu < 0) mu = 0;
                    if (mu > 1) mu = 1;

                    cost += mu * mu * ratio;
                }
            }

            // Ranking penalty
            if (_fahpRanking != null)
            {
                var currentRanking = w
                    .Select((v, i) => new { Index = i, Value = v })
                    .OrderByDescending(x => x.Value)
                    .Select(x => x.Index)
                    .ToList();

                int mismatch = 0;
                for (int i = 0; i < n; i++)
                {
                    if (currentRanking[i] != _fahpRanking[i])
                        mismatch++;
                }

                // Penalti proporsional terhadap jumlah ketidaksesuaian ranking
                cost *= (1 + mismatch);
            }

            return 1 / (cost + 0.000001m);
        }


        public async Task<Dictionary<string, decimal>> CalculateFromDatabaseAsync()
        {
            var comparisons = await _context.CriteriaComparisons
                .Where(c => c.FuzzyL != null && c.FuzzyM != null && c.FuzzyU != null)
                .ToListAsync();

            var allCriteriaIds = comparisons
                .SelectMany(c => new[] { c.CriteriaId1, c.CriteriaId2 })
                .Distinct()
                .OrderBy(id => id)
                .ToList();

            int n = allCriteriaIds.Count();

            var idIndexMap = allCriteriaIds
                .Select((id, index) => new { id, index })
                .ToDictionary(x => x.id, x => x.index);

            var comparisonViewModels = comparisons.Select(c => new FuzzyComparisonViewModel
            {
                Kriteria1Id = idIndexMap[c.CriteriaId1],
                Kriteria2Id = idIndexMap[c.CriteriaId2],
                L = c.FuzzyL ?? 1,
                M = c.FuzzyM ?? 1,
                U = c.FuzzyU ?? 1
            }).ToList();

            var request = new FuzzyComparisonRequest
            {
                CriteriaCount = n,
                Comparisons = comparisonViewModels
            };

            var weights = await CalculateWeightsWithValidationAsync(request);

            var result = weights
                .Select((value, index) => new { Key = $"C{index + 1}", Value = Math.Round(value, 4) })
                .ToDictionary(x => x.Key, x => x.Value);

            return result;
        }
        private decimal CalculateSpearmanCorrelation(List<decimal> list1, List<decimal> list2)
        {
            if (list1.Count != list2.Count)
                throw new ArgumentException("List lengths must be equal.");

            int n = list1.Count;

            var rank1 = list1
                .Select((value, index) => new { Index = index, Value = value })
                .OrderByDescending(x => x.Value)
                .Select((x, rank) => new { x.Index, Rank = rank + 1 })
                .OrderBy(x => x.Index)
                .Select(x => x.Rank)
                .ToList();

            var rank2 = list2
                .Select((value, index) => new { Index = index, Value = value })
                .OrderByDescending(x => x.Value)
                .Select((x, rank) => new { x.Index, Rank = rank + 1 })
                .OrderBy(x => x.Index)
                .Select(x => x.Rank)
                .ToList();

            decimal sumDiffSquared = 0;
            for (int i = 0; i < n; i++)
            {
                int d = rank1[i] - rank2[i];
                sumDiffSquared += d * d;
            }

            return 1 - (6 * sumDiffSquared) / (n * (n * n - 1));
        }
        public void SavePsoLogs()
        {
            _context.PsoIterations.RemoveRange(_context.PsoIterations);
            _context.SaveChanges();

            var criteriaList = _context.Criterias.OrderBy(c => c.Id).ToList();

            var grouped = PsoIterationsLog
                .GroupBy(l => l.Iteration)
                .OrderBy(g => g.Key);
            long currentId = _context.PsoIterations.Any()
                ? _context.PsoIterations.Max(p => p.Id) + 1
                : 1;

            foreach (var group in grouped)
            {
                var best = group.OrderByDescending(x => x.Fitness).First();

                for (int i = 0; i < best.Weights.Count; i++)
                {
                    var criteria = criteriaList[i];

                    _context.PsoIterations.Add(new PsoIteration
                    {
                        Id = currentId++,
                        Iteration = best.Iteration,
                        CriteriaId = criteria.Id,
                        Weight = best.Weights[i],
                        Velocity = best.Velocity[i],
                        Pbest = best.PBest[i],
                        Fitness = best.Fitness,
                        CreatedAt = DateTime.Now
                    });
                }
            }

            _context.SaveChanges();
        }

        public void SaveNormalizations()
        {
            // Hapus data lama
            _context.CriteriaNormalizations.RemoveRange(_context.CriteriaNormalizations);
            _context.SaveChanges();

            var criteriaList = _context.Criterias.OrderBy(c => c.Id).ToList();

            for (int i = 0; i < NormalisasiLog.Count; i++)
            {
                var criteria = criteriaList[i];
                var log = NormalisasiLog[i];

                var data = new CriteriaNormalization
                {
                    CriteriaId = criteria.Id,
                    GeomL = log.GeomL,
                    GeomM = log.GeomM,
                    GeomU = log.GeomU,

                    // Simpan defuzzifikasi (opsional)
                    NormalizationResult = (log.NormalizedL + log.NormalizedM + log.NormalizedU) / 3,

                    CreatedAt = DateTime.Now
                };

                _context.CriteriaNormalizations.Add(data);
            }

            _context.SaveChanges();
        }
        private decimal[,] CreateLowerMatrix(FuzzyComparisonRequest request)
        {
            int n = request.CriteriaCount;
            decimal[,] mat = new decimal[n, n];
            foreach (var c in request.Comparisons)
            {
                mat[c.Kriteria1Id, c.Kriteria2Id] = c.L;
                mat[c.Kriteria2Id, c.Kriteria1Id] = 1 / c.L;
            }
            return mat;
        }

        private decimal[,] CreateMiddleMatrix(FuzzyComparisonRequest request)
        {
            int n = request.CriteriaCount;
            decimal[,] mat = new decimal[n, n];
            foreach (var c in request.Comparisons)
            {
                mat[c.Kriteria1Id, c.Kriteria2Id] = c.M;
                mat[c.Kriteria2Id, c.Kriteria1Id] = 1 / c.M;
            }
            return mat;
        }

        private decimal[,] CreateUpperMatrix(FuzzyComparisonRequest request)
        {
            int n = request.CriteriaCount;
            decimal[,] mat = new decimal[n, n];
            foreach (var c in request.Comparisons)
            {
                mat[c.Kriteria1Id, c.Kriteria2Id] = c.U;
                mat[c.Kriteria2Id, c.Kriteria1Id] = 1 / c.U;
            }
            return mat;
        }
    }
}