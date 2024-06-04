using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountModule.Model
{
    public class AccountDetails
    {
        [Key]
        public long AccountId { get; set; }

        [ForeignKey(nameof(AccountTypeMaster.AccountTypeId))]
        public int AccountTypeId { get; set; }
        public double CurrentBalance { get; set; }
        [DefaultValue(1)]
        public int Status { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public DateTime OpenDate { get; set; } = DateTime.Now;
        [SwaggerSchema(ReadOnly = true)]
        [DefaultValue("")]
        public DateTime? ClosedDate { get; set; }

        [ForeignKey(nameof(Users.UserId))]
        public int UserId { get; set; }
    }
}
