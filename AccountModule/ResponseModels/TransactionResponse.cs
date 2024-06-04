using System.ComponentModel.DataAnnotations.Schema;

namespace AccountModule.ResponseModels
{
    public class TransactionResponse
    {
        public long TransactionId { get; set; }

        public long AccountId { get; set; }
        public string AccountDetailName { get; set; }
        public double Amount { get; set; }
        public int TransactionTypeId { get; set; }
        public char TransactionTypeName { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionModeId { get; set; }
        public string TransactionMode { get; set; }
    }
}
