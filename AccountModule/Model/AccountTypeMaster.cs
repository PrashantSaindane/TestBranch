using System.ComponentModel.DataAnnotations;

namespace AccountModule.Model
{
    public class AccountTypeMaster
    {
        [Key]
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
    }
}
