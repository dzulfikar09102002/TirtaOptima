using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class AlternativeScore
{
    public long Id { get; set; }

    public long PiutangId { get; set; }

    public long CriteriaId { get; set; }

    public int? Score { get; set; }

    public virtual Criteria Criteria { get; set; } = null!;

    public virtual Debt Piutang { get; set; } = null!;
}
