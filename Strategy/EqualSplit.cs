using System;
using System.Collections.Generic;
using SplitwiseClone.Interfaces;
using SplitwiseClone.Models;

namespace SplitwiseClone.Strategy
{
    public class EqualSplit : ISplitStrategy
    {
        public List<Split> CalculateSplit(double totalAmount, List<string> userIds, List<double> values = null)
        {
            var splits = new List<Split>();
            double amountPerUser = totalAmount / userIds.Count;

            foreach (var userId in userIds)
            {
                splits.Add(new Split(userId, amountPerUser));
            }

            return splits;
        }
    }
}
