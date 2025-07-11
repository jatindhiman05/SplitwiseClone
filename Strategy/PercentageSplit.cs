using System;
using System.Collections.Generic;
using SplitwiseClone.Interfaces;
using SplitwiseClone.Models;

namespace SplitwiseClone.Strategy
{
    public class PercentageSplit : ISplitStrategy
    {
        public List<Split> CalculateSplit(double totalAmount, List<string> userIds, List<double> values = null)
        {
            if (values == null || values.Count != userIds.Count)
            {
                throw new ArgumentException("Percentage split requires values for all users");
            }

            var splits = new List<Split>();

            for (int i = 0; i < userIds.Count; i++)
            {
                double amount = (totalAmount * values[i]) / 100.0;
                splits.Add(new Split(userIds[i], amount));
            }

            return splits;
        }
    }
}
