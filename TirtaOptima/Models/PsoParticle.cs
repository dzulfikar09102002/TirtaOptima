using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class PsoParticle
{
    public long Id { get; set; }

    public long PsoId { get; set; }

    public long CriteriaId { get; set; }

    public int ParticleIndex { get; set; }

    public decimal WeightValue { get; set; }

    public decimal? Fitness { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public virtual Criteria Criteria { get; set; } = null!;

    public virtual PsoResult Pso { get; set; } = null!;
}
