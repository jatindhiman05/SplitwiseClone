using System;
using System.Collections.Generic;
using System.Linq;
using SplitwiseClone.Interfaces;

namespace SplitwiseClone.Models
{
    public class User : IObserver
    {
        private static int nextUserId = 0;

        public string UserId { get; }
        public string Name { get; }
        public string Email { get; }
        public Dictionary<string, double> Balances { get; } = new Dictionary<string, double>();

        public User(string name, string email)
        {
            UserId = "user" + (++nextUserId);
            Name = name;
            Email = email;
        }

        public void Update(string message)
        {
            Console.WriteLine($"[NOTIFICATION to {Name}]: {message}");
        }

        public void UpdateBalance(string otherUserId, double amount)
        {
            if (Balances.ContainsKey(otherUserId))
            {
                Balances[otherUserId] += amount;
            }
            else
            {
                Balances[otherUserId] = amount;
            }

            if (Math.Abs(Balances[otherUserId]) < 0.01)
            {
                Balances.Remove(otherUserId);
            }
        }

        public double GetTotalOwed()
        {
            return Balances.Where(b => b.Value < 0).Sum(b => Math.Abs(b.Value));
        }

        public double GetTotalOwing()
        {
            return Balances.Where(b => b.Value > 0).Sum(b => b.Value);
        }
    }
}
