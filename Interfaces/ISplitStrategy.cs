using System.Collections.Generic;
using SplitwiseClone.Models;

namespace SplitwiseClone.Interfaces
{
    public interface ISplitStrategy
    {
        List<Split> CalculateSplit(double totalAmount, List<string> userIds, List<double> values = null);
    }
}
