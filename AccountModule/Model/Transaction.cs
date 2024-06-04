using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountModule.Model
{
    public class Transaction
    {
        [Key]
        public long TransactionId { get; set; }

        [ForeignKey(nameof(AccountDetails.AccountId))]
        public long AccountId { get; set; }
        public double Amount { get; set; }

       [ForeignKey(nameof(TransactionType.TransactionTypeId))]
        public int TransactionTypeId { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [ForeignKey(nameof(TransactionMode.TransactionModeId))]
        public int TransactionModeId { get; set; }
       
    }
}
