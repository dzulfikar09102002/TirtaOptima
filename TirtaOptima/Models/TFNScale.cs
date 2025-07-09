namespace TirtaOptima.Models
{
    public class TFNScale
    {
        public decimal L { get; set; }
        public decimal M { get; set; }
        public decimal U { get; set; }
        public TFNScale(decimal l, decimal m, decimal u)
        {
            L = l;
            M = m;
            U = u;
        }
    }
}
