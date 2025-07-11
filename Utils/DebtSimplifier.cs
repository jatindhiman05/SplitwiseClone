using System;
using System.Collections.Generic;
using System.Linq;

namespace SplitwiseClone.Utils
{
    public static class DebtSimplifier
    {
        public static Dictionary<string, Dictionary<string, double>> SimplifyDebts(
            Dictionary<string, Dictionary<string, double>> groupBalances)
        {
            // Step 1: Calculate net balances
            var netAmounts = new Dictionary<string, double>();

            foreach (var user in groupBalances.Keys)
                netAmounts[user] = 0;

            foreach (var payer in groupBalances)
            {
                foreach (var receiver in payer.Value)
                {
                    if (receiver.Value > 0)
                    {
                        netAmounts[payer.Key] += receiver.Value;
                        netAmounts[receiver.Key] -= receiver.Value;
                    }
                }
            }

            // Step 2: Separate into creditors and debtors
            var creditors = new List<KeyValuePair<string, double>>();
            var debtors = new List<KeyValuePair<string, double>>();

            foreach (var user in netAmounts)
            {
                if (user.Value > 0.01)
                    creditors.Add(new KeyValuePair<string, double>(user.Key, user.Value));
                else if (user.Value < -0.01)
                    debtors.Add(new KeyValuePair<string, double>(user.Key, -user.Value)); // store positive
            }

            creditors = creditors.OrderByDescending(c => c.Value).ToList();
            debtors = debtors.OrderByDescending(d => d.Value).ToList();

            // Step 3: Greedy minimize transactions
            var simplified = new Dictionary<string, Dictionary<string, double>>();
            foreach (var user in groupBalances.Keys)
                simplified[user] = new Dictionary<string, double>();

            int i = 0, j = 0;

            while (i < creditors.Count && j < debtors.Count)
            {
                string creditorId = creditors[i].Key;
                string debtorId = debtors[j].Key;
                double amount = Math.Min(creditors[i].Value, debtors[j].Value);

                simplified[creditorId][debtorId] = amount;
                simplified[debtorId][creditorId] = -amount;

                creditors[i] = new KeyValuePair<string, double>(creditorId, creditors[i].Value - amount);
                debtors[j] = new KeyValuePair<string, double>(debtorId, debtors[j].Value - amount);

                if (creditors[i].Value < 0.01) i++;
                if (debtors[j].Value < 0.01) j++;
            }

            return simplified;
        }
    }
}
