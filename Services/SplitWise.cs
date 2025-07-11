using System;
using System.Collections.Generic;
using SplitwiseClone.Enums;
using SplitwiseClone.Models;
using SplitwiseClone.Strategy;

namespace SplitwiseClone.Services
{
    public class Splitwise
    {
        private static Splitwise instance;
        private static readonly object lockObject = new object();

        private Dictionary<string, User> users = new Dictionary<string, User>();
        private Dictionary<string, Group> groups = new Dictionary<string, Group>();
        private Dictionary<string, Expense> expenses = new Dictionary<string, Expense>();

        private Splitwise() { }

        public static Splitwise Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new Splitwise();
                    }
                    return instance;
                }
            }
        }

        public User CreateUser(string name, string email)
        {
            var user = new User(name, email);
            users[user.UserId] = user;
            Console.WriteLine($"User created: {name} (ID: {user.UserId})");
            return user;
        }

        public User GetUser(string userId)
        {
            return users.TryGetValue(userId, out var user) ? user : null;
        }

        public Group CreateGroup(string name)
        {
            var group = new Group(name);
            groups[group.GroupId] = group;
            Console.WriteLine($"Group created: {name} (ID: {group.GroupId})");
            return group;
        }

        public Group GetGroup(string groupId)
        {
            return groups.TryGetValue(groupId, out var group) ? group : null;
        }

        public void AddUserToGroup(string userId, string groupId)
        {
            var user = GetUser(userId);
            var group = GetGroup(groupId);

            if (user != null && group != null)
            {
                group.AddMember(user);
            }
        }

        public bool RemoveUserFromGroup(string userId, string groupId)
        {
            var group = GetGroup(groupId);
            var user = GetUser(userId);

            if (group == null)
            {
                Console.WriteLine("Group not found!");
                return false;
            }

            if (user == null)
            {
                Console.WriteLine("User not found!");
                return false;
            }

            bool removed = group.RemoveMember(userId);
            if (removed)
            {
                Console.WriteLine($"{user.Name} successfully left {group.Name}");
            }

            return removed;
        }

        public void AddExpenseToGroup(string groupId, string description, double amount,
                                      string paidByUserId, List<string> involvedUsers,
                                      SplitType splitType, List<double> splitValues = null)
        {
            var group = GetGroup(groupId);
            if (group == null)
            {
                Console.WriteLine("Group not found!");
                return;
            }

            group.AddExpense(description, amount, paidByUserId, involvedUsers, splitType, splitValues);
        }

        public void SettlePaymentInGroup(string groupId, string fromUserId, string toUserId, double amount)
        {
            var group = GetGroup(groupId);
            if (group == null)
            {
                Console.WriteLine("Group not found!");
                return;
            }

            group.SettlePayment(fromUserId, toUserId, amount);
        }

        public void SettleIndividualPayment(string fromUserId, string toUserId, double amount)
        {
            var fromUser = GetUser(fromUserId);
            var toUser = GetUser(toUserId);

            if (fromUser != null && toUser != null)
            {
                fromUser.UpdateBalance(toUserId, amount);
                toUser.UpdateBalance(fromUserId, -amount);
                Console.WriteLine($"{fromUser.Name} settled Rs{amount} with {toUser.Name}");
            }
        }

        public void AddIndividualExpense(string description, double amount, string paidByUserId,
                                         string toUserId, SplitType splitType, List<double> splitValues = null)
        {
            var strategy = SplitFactory.GetSplitStrategy(splitType);
            var splits = strategy.CalculateSplit(amount, new List<string> { paidByUserId, toUserId }, splitValues);

            var expense = new Expense(description, amount, paidByUserId, splits);
            expenses[expense.ExpenseId] = expense;

            var paidByUser = GetUser(paidByUserId);
            var toUser = GetUser(toUserId);

            paidByUser.UpdateBalance(toUserId, amount);
            toUser.UpdateBalance(paidByUserId, -amount);

            Console.WriteLine($"Individual expense added: {description} (Rs {amount}) paid by {paidByUser.Name} for {toUser.Name}");
        }

        public void ShowUserBalance(string userId)
        {
            var user = GetUser(userId);
            if (user == null) return;

            Console.WriteLine($"\n=========== Balance for {user.Name} ====================");
            Console.WriteLine($"Total you owe: Rs {user.GetTotalOwed():F2}");
            Console.WriteLine($"Total others owe you: Rs {user.GetTotalOwing():F2}");

            Console.WriteLine("Detailed balances:");
            foreach (var balance in user.Balances)
            {
                var otherUser = GetUser(balance.Key);
                if (otherUser != null)
                {
                    if (balance.Value > 0)
                        Console.WriteLine($"  {otherUser.Name} owes you: Rs{balance.Value:F2}");
                    else
                        Console.WriteLine($"  You owe {otherUser.Name}: Rs{Math.Abs(balance.Value):F2}");
                }
            }
        }

        public void ShowGroupBalances(string groupId)
        {
            var group = GetGroup(groupId);
            if (group != null)
            {
                group.ShowGroupBalances();
            }
        }

        public void SimplifyGroupDebts(string groupId)
        {
            var group = GetGroup(groupId);
            if (group != null)
            {
                group.SimplifyGroupDebts();
            }
        }
    }
}
