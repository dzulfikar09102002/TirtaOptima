using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class Village
{
    public long Id { get; set; }

    public long KodeDesa { get; set; }

    public string Nama { get; set; } = null!;

    public long? KodeKec { get; set; }

    public string? Keterangan { get; set; }

    public int? Layanan { get; set; }

    public int? Jarak { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual User? DeletedByNavigation { get; set; }

    public virtual District? KodeKecNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
