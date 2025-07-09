using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class VDebt
{
    public long? PiutangId { get; set; }

    public long PelangganId { get; set; }

    public int Rekening { get; set; }

    public decimal? TotalNominal { get; set; }

    public string? StatusTerakhir { get; set; }

    public DateTime? TanggalTerakhir { get; set; }
}
