    using TirtaOptima.Models;

    namespace TirtaOptima.ViewModels
    {
        public class VillagesViewModel
        {
            public List<District> Districts { get; set; } = new();
            public List<Village> Villages { get; set; } = new();
            public long Id { get; set; }
            public long KodeDesa { get; set; }

            public string Nama { get; set; } = null!;

            public long? KodeKec { get; set; }

            public int? Layanan { get; set; }

            public int? Jarak { get; set; }

            public string? Keterangan { get; set; }

        }
    }
