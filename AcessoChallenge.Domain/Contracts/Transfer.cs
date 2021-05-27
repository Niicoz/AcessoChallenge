namespace AcessoChallenge.Infrastructure.Contracts
{
    public class Transfer
    {
        public string AccountNumber { get; }

        public float Value { get; }

        public string Type { get; }

        public Transfer(string accountNumber, float value, string type)
        {
            AccountNumber = accountNumber;
            Value = value;
            Type = type;
        }
    }
}