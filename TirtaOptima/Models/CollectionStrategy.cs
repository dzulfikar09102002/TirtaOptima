using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class CollectionStrategy
{
    public long Id { get; set; }

    public string NamaStrategi { get; set; } = null!;

    public string Deskripsi { get; set; } = null!;

    public string Kondisi { get; set; } = null!;

    public int? RentangWaktu { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? DeletedBy { get; set; }

    public virtual ICollection<StrategyResult> StrategyResults { get; set; } = new List<StrategyResult>();
}
