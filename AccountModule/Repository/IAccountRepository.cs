using AccountModule.Model;
using AccountModule.ResponseModels;

namespace AccountModule.Repository
{
    public interface IAccountRepository
    {
        public Task<IEnumerable<Users>> GetUsersList();
        public Task<IEnumerable<AccountDetailResponse>> GetAccountDetailList();
        public Task<AccountDetailResponse> GetAccountDetailById(long AccountId);
        public void AddAccountDetail(AccountDetails accountDetails);
        public void UpdateAccountDetail(AccountDetails accountDetails);
        public Task<IEnumerable<TransactionResponse>> GetTransactionList();
        public Task<TransactionResponse> GetTransactionById(long TransactionId);
        public Task<string> AddTransaction(Model.Transaction transaction);
        public void UpdateTransaction(Model.Transaction transaction);
        public Users GetLoginUser(string UserName, string Password);
    }
}
