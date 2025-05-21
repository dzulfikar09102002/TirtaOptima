using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class Debt
{
    public long Id { get; set; }

    public long PelangganId { get; set; }

    public int Rekening { get; set; }

    public decimal Nominal { get; set; }

    public string? Ket { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<FahpCalculation> FahpCalculations { get; set; } = new List<FahpCalculation>();

    public virtual ICollection<Letter> Letters { get; set; } = new List<Letter>();

    public virtual Customer Pelanggan { get; set; } = null!;

    public virtual ICollection<PsoResult> PsoResults { get; set; } = new List<PsoResult>();

    public virtual ICollection<StrategyResult> StrategyResults { get; set; } = new List<StrategyResult>();

    public virtual User? UpdatedByNavigation { get; set; }
}
