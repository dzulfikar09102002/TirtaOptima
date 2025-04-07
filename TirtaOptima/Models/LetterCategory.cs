using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class LetterCategory
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string? Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<Letter> Letters { get; set; } = new List<Letter>();

    public virtual User? UpdatedByNavigation { get; set; }
}
