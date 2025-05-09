using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class Payment
{
    public long Id { get; set; }

    public long IdPelanggan { get; set; }

    public long NominalBayar { get; set; }

    public string? Ket { get; set; }

    public DateOnly? TanggalBayar { get; set; }

    public string? Kasir { get; set; }

    public int? Bulan { get; set; }

    public int? Tahun { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<DebtsManagement> DebtsManagements { get; set; } = new List<DebtsManagement>();

    public virtual User? DeletedByNavigation { get; set; }

    public virtual Customer IdPelangganNavigation { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
