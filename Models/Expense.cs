using System.Collections.Generic;

namespace SplitwiseClone.Models
{
    public class Expense
    {
        private static int nextExpenseId = 0;

        public string ExpenseId { get; }
        public string Description { get; }
        public double TotalAmount { get; }
        public string PaidByUserId { get; }
        public List<Split> Splits { get; }
        public string GroupId { get; }

        public Expense(string description, double totalAmount, string paidByUserId, List<Split> splits, string groupId = "")
        {
            ExpenseId = "expense" + (++nextExpenseId);
            Description = description;
            TotalAmount = totalAmount;
            PaidByUserId = paidByUserId;
            Splits = splits;
            GroupId = groupId;
        }
    }
}
