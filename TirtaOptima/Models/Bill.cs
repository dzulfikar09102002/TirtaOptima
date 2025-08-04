using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class Bill
{
    public long Id { get; set; }

    public long IdPelanggan { get; set; }

    public decimal? Total { get; set; }

    public string? Ket { get; set; }

    public DateOnly? JatuhTempo { get; set; }

    public int? Bulan { get; set; }

    public int? Tahun { get; set; }

    public int? Awal { get; set; }

    public int? Akhir { get; set; }

    public int? Pakai { get; set; }

    public int? Tagihan { get; set; }

    public int? Admin { get; set; }

    public int? Dpm { get; set; }

    public int? Materai { get; set; }

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
