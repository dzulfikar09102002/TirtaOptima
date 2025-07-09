using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class PsoResult
{
    public long Id { get; set; }

    public long PiutangId { get; set; }

    public decimal Prioritas { get; set; }

    public int Iterasi { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? DeletedBy { get; set; }

    public virtual Debt Piutang { get; set; } = null!;

    public virtual ICollection<PsoParticle> PsoParticles { get; set; } = new List<PsoParticle>();

    public virtual ICollection<StrategyResult> StrategyResults { get; set; } = new List<StrategyResult>();
}
