using System;
using System.Collections.Generic;
using SplitwiseClone.Enums;
using SplitwiseClone.Services;
using SplitwiseClone.Models;

namespace SplitwiseClone
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = Splitwise.Instance;

            Console.WriteLine("\n=========== Creating Users ====================");
            var user1 = manager.CreateUser("Aditya", "aditya@gmail.com");
            var user2 = manager.CreateUser("Rohit", "rohit@gmail.com");
            var user3 = manager.CreateUser("Manish", "manish@gmail.com");
            var user4 = manager.CreateUser("Saurav", "saurav@gmail.com");

            Console.WriteLine("\n=========== Creating Group and Adding Members ====================");
            var hostelGroup = manager.CreateGroup("Hostel Expenses");
            manager.AddUserToGroup(user1.UserId, hostelGroup.GroupId);
            manager.AddUserToGroup(user2.UserId, hostelGroup.GroupId);
            manager.AddUserToGroup(user3.UserId, hostelGroup.GroupId);
            manager.AddUserToGroup(user4.UserId, hostelGroup.GroupId);

            Console.WriteLine("\n=========== Adding Expenses in group ====================");
            var groupMembers = new List<string> { user1.UserId, user2.UserId, user3.UserId, user4.UserId };
            manager.AddExpenseToGroup(hostelGroup.GroupId, "Lunch", 800.0, user1.UserId, groupMembers, SplitType.EQUAL);

            var dinnerMembers = new List<string> { user1.UserId, user3.UserId, user4.UserId };
            var dinnerAmounts = new List<double> { 200.0, 300.0, 200.0 };
            manager.AddExpenseToGroup(hostelGroup.GroupId, "Dinner", 700.0, user3.UserId, dinnerMembers, SplitType.EXACT, dinnerAmounts);

            Console.WriteLine("\n=========== printing Group-Specific Balances ====================");
            manager.ShowGroupBalances(hostelGroup.GroupId);

            Console.WriteLine("\n=========== Debt Simplification ====================");
            manager.SimplifyGroupDebts(hostelGroup.GroupId);

            Console.WriteLine("\n=========== printing Group-Specific Balances ====================");
            manager.ShowGroupBalances(hostelGroup.GroupId);

            Console.WriteLine("\n=========== Adding Individual Expense ====================");
            manager.AddIndividualExpense("Coffee", 40.0, user2.UserId, user4.UserId, SplitType.EQUAL);

            Console.WriteLine("\n=========== printing User Balances ====================");
            manager.ShowUserBalance(user1.UserId);
            manager.ShowUserBalance(user2.UserId);
            manager.ShowUserBalance(user3.UserId);
            manager.ShowUserBalance(user4.UserId);

            Console.WriteLine("\n========== Attempting to remove Rohit from group ==========");
            manager.RemoveUserFromGroup(user2.UserId, hostelGroup.GroupId);

            Console.WriteLine("\n======== Making Settlement to Clear Rohit's Debt ==========");
            manager.SettlePaymentInGroup(hostelGroup.GroupId, user2.UserId, user3.UserId, 200.0);

            Console.WriteLine("\n======== Attempting to Remove Rohit Again ==========");
            manager.RemoveUserFromGroup(user2.UserId, hostelGroup.GroupId);

            Console.WriteLine("\n=========== Updated Group Balances ====================");
            manager.ShowGroupBalances(hostelGroup.GroupId);
        }
    }
}
