using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class StrategyResult
{
    public long Id { get; set; }

    public long PiutangId { get; set; }

    public long StrategiId { get; set; }

    public decimal? Score { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? DeletedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? DeletedByNavigation { get; set; }

    public virtual Debt Piutang { get; set; } = null!;

    public virtual Policy Strategi { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
