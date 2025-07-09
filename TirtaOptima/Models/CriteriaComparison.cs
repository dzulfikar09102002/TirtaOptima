using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class CriteriaComparison
{
    public long Id { get; set; }

    public long CriteriaId1 { get; set; }

    public long CriteriaId2 { get; set; }

    public decimal? ScaleValue { get; set; }

    public decimal? FuzzyL { get; set; }

    public decimal? FuzzyM { get; set; }

    public decimal? FuzzyU { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long? CreatedBy { get; set; }

    public virtual Criteria CriteriaId1Navigation { get; set; } = null!;

    public virtual Criteria CriteriaId2Navigation { get; set; } = null!;
}
