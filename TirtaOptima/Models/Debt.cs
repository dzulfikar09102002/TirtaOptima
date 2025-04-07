using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class Debt
{
    public long Id { get; set; }

    public long IdPelanggan { get; set; }

    public string? NoPelanggan { get; set; }

    public string Name { get; set; } = null!;

    public string? Alamat { get; set; }

    public string? Jenis { get; set; }

    public string? NoTelepon { get; set; }

    public string? Email { get; set; }

    public long NominalAir { get; set; }

    public long NomialNonair { get; set; }

    public long Denda { get; set; }

    public long? Total { get; set; }

    public string? Ket { get; set; }

    public string? Status { get; set; }

    public DateOnly? JatuhTempo { get; set; }

    public int? Bulan { get; set; }

    public int? Tahun { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public virtual ICollection<Collection> Collections { get; set; } = new List<Collection>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<DebtsManagement> DebtsManagements { get; set; } = new List<DebtsManagement>();

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<FahpCalculation> FahpCalculations { get; set; } = new List<FahpCalculation>();

    public virtual ICollection<Letter> Letters { get; set; } = new List<Letter>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<PsoResult> PsoResults { get; set; } = new List<PsoResult>();

    public virtual ICollection<StrategyResult> StrategyResults { get; set; } = new List<StrategyResult>();

    public virtual User? UpdatedByNavigation { get; set; }
}
