using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class Debt
{
    public long Id { get; set; }

    public long PelangganId { get; set; }

    public int Rekening { get; set; }

    public decimal Nominal { get; set; }

    public DateTime? TanggalTerakhir { get; set; }

    public string? StatusTerakhir { get; set; }

    public string? Ket { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public virtual ICollection<Collection> Collections { get; set; } = new List<Collection>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<Letter> Letters { get; set; } = new List<Letter>();

    public virtual Customer Pelanggan { get; set; } = null!;

    public virtual ICollection<StrategyResult> StrategyResults { get; set; } = new List<StrategyResult>();

    public virtual User? UpdatedByNavigation { get; set; }
}
