using SplitwiseClone.Enums;
using SplitwiseClone.Interfaces;

namespace SplitwiseClone.Strategy
{
    public static class SplitFactory
    {
        public static ISplitStrategy GetSplitStrategy(SplitType type)
        {
            switch (type)
            {
                case SplitType.EQUAL:
                    return new EqualSplit();
                case SplitType.EXACT:
                    return new ExactSplit();
                case SplitType.PERCENTAGE:
                    return new PercentageSplit();
                default:
                    return new EqualSplit(); // default fallback
            }
        }
    }
}
