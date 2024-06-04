using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountModule.Model
{
    public class TransactionType
    {
        [Key]
        public int TransactionTypeId { get; set; }

        [Column(TypeName = "char")]
        [StringLength(2)]
        public char TransactionCode { get; set; }
        public string TransactionTypeName { get; set; }
    }
}
