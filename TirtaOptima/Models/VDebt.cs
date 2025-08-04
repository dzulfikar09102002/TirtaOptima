using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class VDebt
{
    public long? IdPelanggan { get; set; }

    public decimal? TotalNominal { get; set; }

    public string? StatusTerakhir { get; set; }

    public DateTime? TanggalTerakhir { get; set; }
}
