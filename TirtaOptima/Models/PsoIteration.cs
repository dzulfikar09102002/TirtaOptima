using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class PsoIteration
{
    public long Id { get; set; }

    public int Iteration { get; set; }

    public long CriteriaId { get; set; }

    public decimal? Weight { get; set; }

    public decimal? Velocity { get; set; }

    public decimal? Pbest { get; set; }

    public decimal? Fitness { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Criteria Criteria { get; set; } = null!;
}
