using System.ComponentModel.DataAnnotations;

namespace AccountModule.Model
{
    public class TransactionMode
    {
        [Key]
        public int TransactionModeId { get; set; }
        public string TransactionModeName { get; set; }
    }
}
