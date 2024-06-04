using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AccountModule.ResponseModels
{
    public class AccountDetailResponse
    {
        public long AccountId { get; set; }
        public int AccountTypeId { get; set; }
        public double CurrentBalance { get; set; }
        [DefaultValue(1)]
        public int Status { get; set; }
        public DateTime OpenDate { get; set; } = DateTime.Now;
        public DateTime? ClosedDate { get; set; } = null;
        public string AccountTypeName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
