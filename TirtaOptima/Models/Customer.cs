using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class Customer
{
    public long Id { get; set; }

    public string Nomor { get; set; } = null!;

    public string Nama { get; set; } = null!;

    public string? Jenis { get; set; }

    public string? Alamat { get; set; }

    public DateTime? Pasang { get; set; }

    public long? Kecamatan { get; set; }

    public long? Kelurahan { get; set; }

    public long Status { get; set; }

    public long? Cabang { get; set; }

    public long? Wilayah { get; set; }

    public string? NoTelepon { get; set; }

    public string? Email { get; set; }

    public DateTime? TanggalPasang { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Debt> Debts { get; set; } = new List<Debt>();

    public virtual User? DeletedByNavigation { get; set; }

    public virtual CustomerType? JenisNavigation { get; set; }

    public virtual District? KecamatanNavigation { get; set; }

    public virtual Village? KelurahanNavigation { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Status StatusNavigation { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
