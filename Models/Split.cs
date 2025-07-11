namespace SplitwiseClone.Models
{
    public class Split
    {
        public string UserId { get; }
        public double Amount { get; }

        public Split(string userId, double amount)
        {
            UserId = userId;
            Amount = amount;
        }
    }
}
