using TirtaOptima.Models;

namespace TirtaOptima.Helpers
{
    public static class FuzzyAHPHelper
    {
        public static TFNScale TFNScaleNumber(decimal scale)
        {
            return scale switch
            {
                1 => new TFNScale(1.0m, 1.0m, 1.0m),
                2 => new TFNScale(0.5m, 1.0m, 1.5m),
                3 => new TFNScale(1.0m, 1.5m, 2.0m),
                4 => new TFNScale(1.5m, 2.0m, 2.5m),
                5 => new TFNScale(2.0m, 2.5m, 3.0m),
                6 => new TFNScale(2.5m, 3.0m, 3.5m),
                7 => new TFNScale(3.0m, 3.5m, 4.0m),
                8 => new TFNScale(3.5m, 4.0m, 4.5m),
                9 => new TFNScale(4.0m, 4.5m, 4.5m),
                _ => throw new ArgumentException("Skala AHP harus di antara 1 hingga 9.")
            };
        }
        public static TFNScale TFNReciprocalScaleNumber(decimal scale)
        {
            var tfn = TFNScaleNumber(scale);
            return new TFNScale(
                u: 1 / tfn.U,
                m: 1 / tfn.M,
                l: 1 / tfn.L
            );
        }
    }
}

