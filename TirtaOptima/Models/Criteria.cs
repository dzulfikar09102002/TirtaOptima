using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class Criteria
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal? Bobot { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? DeletedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<CriteriaComparison> CriteriaComparisonCriteriaId1Navigations { get; set; } = new List<CriteriaComparison>();

    public virtual ICollection<CriteriaComparison> CriteriaComparisonCriteriaId2Navigations { get; set; } = new List<CriteriaComparison>();

    public virtual ICollection<CriteriaNormalization> CriteriaNormalizations { get; set; } = new List<CriteriaNormalization>();

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<PsoIteration> PsoIterations { get; set; } = new List<PsoIteration>();

    public virtual User? UpdatedByNavigation { get; set; }
}
