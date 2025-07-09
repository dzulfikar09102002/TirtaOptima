using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class VPairwiseComparison
{
    public long CriteriaId1 { get; set; }

    public string CriteriaName1 { get; set; } = null!;

    public long CriteriaId2 { get; set; }

    public string CriteriaName2 { get; set; } = null!;

    public float? ScaleValue { get; set; }

    public float? FuzzyL { get; set; }

    public float? FuzzyM { get; set; }

    public float? FuzzyU { get; set; }
}
