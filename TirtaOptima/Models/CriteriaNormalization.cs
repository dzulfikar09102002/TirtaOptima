using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class CriteriaNormalization
{
    public long Id { get; set; }

    public long CriteriaId { get; set; }

    public decimal? GeomL { get; set; }

    public decimal? GeomM { get; set; }

    public decimal? GeomU { get; set; }

    public decimal? NormalizationResult { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Criteria Criteria { get; set; } = null!;
}
