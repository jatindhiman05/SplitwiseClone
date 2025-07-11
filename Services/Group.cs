using System;
using System.Collections.Generic;
using System.Linq;
using SplitwiseClone.Enums;
using SplitwiseClone.Interfaces;
using SplitwiseClone.Models;
using SplitwiseClone.Strategy;
using SplitwiseClone.Utils;

namespace SplitwiseClone.Services
{
    public class Group
    {
        private static int nextGroupId = 0;

        private User GetUserById(string userId)
        {
            return Members.FirstOrDefault(member => member.UserId == userId);
        }

        public string GroupId { get; }
        public string Name { get; }
        public List<User> Members { get; } = new List<User>();
        public Dictionary<string, Expense> GroupExpenses { get; } = new Dictionary<string, Expense>();
        public Dictionary<string, Dictionary<string, double>> GroupBalances { get; } = new Dictionary<string, Dictionary<string, double>>();

        public Group(string name)
        {
            GroupId = "group" + (++nextGroupId);
            Name = name;
        }

        public void AddMember(User user)
        {
            Members.Add(user);
            GroupBalances[user.UserId] = new Dictionary<string, double>();
            Console.WriteLine($"{user.Name} added to group {Name}");
        }

        public bool RemoveMember(string userId)
        {
            if (!CanUserLeaveGroup(userId))
            {
                Console.WriteLine("\nUser not allowed to leave group without clearing expenses");
                return false;
            }

            var user = GetUserById(userId);
            if (user != null)
            {
                Members.Remove(user);
            }

            GroupBalances.Remove(userId);

            foreach (var memberBalance in GroupBalances)
            {
                memberBalance.Value.Remove(userId);
            }

            return true;
        }

        public void NotifyMembers(string message)
        {
            foreach (var member in Members)
            {
                member.Update(message);
            }
        }

        public bool IsMember(string userId)
        {
            return GroupBalances.ContainsKey(userId);
        }

        public void UpdateGroupBalance(string fromUserId, string toUserId, double amount)
        {
            if (!GroupBalances[fromUserId].ContainsKey(toUserId))
                GroupBalances[fromUserId][toUserId] = 0;

            if (!GroupBalances[toUserId].ContainsKey(fromUserId))
                GroupBalances[toUserId][fromUserId] = 0;

            GroupBalances[fromUserId][toUserId] += amount;
            GroupBalances[toUserId][fromUserId] -= amount;

            if (Math.Abs(GroupBalances[fromUserId][toUserId]) < 0.01)
                GroupBalances[fromUserId].Remove(toUserId);

            if (Math.Abs(GroupBalances[toUserId][fromUserId]) < 0.01)
                GroupBalances[toUserId].Remove(fromUserId);
        }

        public bool CanUserLeaveGroup(string userId)
        {
            if (!IsMember(userId))
                throw new Exception("User is not a part of this group");

            return GroupBalances[userId].All(balance => Math.Abs(balance.Value) <= 0.01);
        }

        public Dictionary<string, double> GetUserGroupBalances(string userId)
        {
            if (!IsMember(userId))
                throw new Exception("User is not a part of this group");

            return GroupBalances[userId];
        }

        public bool AddExpense(string description, double amount, string paidByUserId,
                               List<string> involvedUsers, SplitType splitType,
                               List<double> splitValues = null)
        {
            if (!IsMember(paidByUserId))
                throw new Exception("Payer is not part of the group");

            foreach (var userId in involvedUsers)
            {
                if (!IsMember(userId))
                    throw new Exception("One or more involved users are not part of the group");
            }

            var strategy = SplitFactory.GetSplitStrategy(splitType);
            var splits = strategy.CalculateSplit(amount, involvedUsers, splitValues);

            var expense = new Expense(description, amount, paidByUserId, splits, GroupId);
            GroupExpenses[expense.ExpenseId] = expense;

            foreach (var split in splits)
            {
                if (split.UserId != paidByUserId)
                {
                    UpdateGroupBalance(paidByUserId, split.UserId, split.Amount);
                }
            }

            var paidByName = GetUserById(paidByUserId).Name;

            Console.WriteLine("\n=========== Sending Notifications ====================");
            NotifyMembers($"New expense added: {description} (Rs {amount})");

            Console.WriteLine("\n=========== Expense Message ====================");
            Console.WriteLine($"Expense added to {Name}: {description} (Rs {amount}) paid by {paidByName}");
            Console.WriteLine("Involved members:");

            if (splitValues != null && splitValues.Count > 0)
            {
                for (int i = 0; i < splitValues.Count; i++)
                {
                    Console.WriteLine($"{GetUserById(involvedUsers[i]).Name} : {splitValues[i]}");
                }
            }
            else
            {
                foreach (var user in involvedUsers)
                {
                    Console.Write($"{GetUserById(user).Name}, ");
                }
                Console.WriteLine("\nWill be Paid Equally");
            }

            return true;
        }

        public bool SettlePayment(string fromUserId, string toUserId, double amount)
        {
            if (!IsMember(fromUserId) || !IsMember(toUserId))
            {
                Console.WriteLine("User is not a part of this group");
                return false;
            }

            UpdateGroupBalance(fromUserId, toUserId, amount);

            var fromName = GetUserById(fromUserId).Name;
            var toName = GetUserById(toUserId).Name;

            NotifyMembers($"Settlement: {fromName} paid {toName} Rs {amount}");
            Console.WriteLine($"Settlement in {Name}: {fromName} settled Rs {amount} with {toName}");

            return true;
        }

        public void ShowGroupBalances()
        {
            Console.WriteLine($"\n=== Group Balances for {Name} ===");

            foreach (var pair in GroupBalances)
            {
                var memberId = pair.Key;
                var memberName = GetUserById(memberId).Name;

                Console.WriteLine($"{memberName}'s balances in group:");

                var userBalances = pair.Value;
                if (userBalances.Count == 0)
                {
                    Console.WriteLine("  No outstanding balances");
                }
                else
                {
                    foreach (var userBalance in userBalances)
                    {
                        var otherUserId = userBalance.Key;
                        var otherName = GetUserById(otherUserId).Name;

                        double balance = userBalance.Value;
                        if (balance > 0)
                        {
                            Console.WriteLine($"  {otherName} owes: Rs {balance:F2}");
                        }
                        else
                        {
                            Console.WriteLine($"  Owes {otherName}: Rs {Math.Abs(balance):F2}");
                        }
                    }
                }
            }
        }

        public void SimplifyGroupDebts()
        {
            var simplified = DebtSimplifier.SimplifyDebts(GroupBalances);
            GroupBalances.Clear();

            foreach (var kvp in simplified)
            {
                GroupBalances[kvp.Key] = kvp.Value;
            }

            Console.WriteLine($"\nDebts have been simplified for group: {Name}");
        }
    }
}
