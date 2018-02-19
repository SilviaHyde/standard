namespace Standard.Data.StringMetrics
{
    public abstract class AbstractAffineGapCost : IAffineGapCost
    {
        protected AbstractAffineGapCost()
        {
        }

        public abstract double GetCost(string textToGap, int stringIndexStartGap, int stringIndexEndGap);

        public abstract double MaxCost { get; }

        public abstract double MinCost { get; }
    }
}