using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class CustomerType
{
    public string Id { get; set; } = null!;

    public string Deskripsi { get; set; } = null!;

    public int? MinPakai { get; set; }

    public int? Tarif1 { get; set; }

    public int? Tarif2 { get; set; }

    public int? Denda { get; set; }

    public int? Retribusi { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual User? DeletedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
