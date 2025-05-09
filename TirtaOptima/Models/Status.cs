using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class Status
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public virtual ICollection<Collection> Collections { get; set; } = new List<Collection>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual User? DeletedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
