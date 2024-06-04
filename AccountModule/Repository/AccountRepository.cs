using AccountModule.Controllers;
using AccountModule.Data;
using AccountModule.Model;
using AccountModule.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace AccountModule.Repository
{
    public class AccountRepository: IAccountRepository
    {
        private readonly AccountContext _dbContext;
        private readonly ILogger<AccountController> _logger;
        public AccountRepository(AccountContext dbContext, ILogger<AccountController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<IEnumerable<AccountDetailResponse>> GetAccountDetailList()
        {
            _logger.LogDebug("Inside GetAccountDetailList Account Repo endpoint");
            try
            {
                var item = await (
                        from ad in _dbContext.AccountDetails
                        join atm in _dbContext.AccountTypeMaster on ad.AccountTypeId equals atm.AccountTypeId
                        join us in _dbContext.Users on ad.UserId equals us.UserId
                        select new AccountDetailResponse
                        {
                            AccountId = ad.AccountId,
                            AccountTypeId = ad.AccountTypeId,
                            UserId = ad.UserId,
                            CurrentBalance = ad.CurrentBalance,
                            Status = ad.Status,
                            OpenDate = ad.OpenDate,
                            ClosedDate = ad.ClosedDate,
                            AccountTypeName = atm.AccountTypeName,
                            UserName = us.FirstName + " " + us.LastName
                        }).ToListAsync();
                if(item == null && item.Count<1)
                {
                    return null;
                }
                _logger.LogDebug($"The response for the GetAccountDetailList in Account Repo is .{item.Count()}");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public async Task<AccountDetailResponse> GetAccountDetailById(long AccountId)
        {
            _logger.LogDebug("Inside GetAccountDetailById in Account Repo endpoint");
            try
            {
                //  return _dbContext.AccountDetails.Find(AccountId);
                var item =await  (
                          from ad in _dbContext.AccountDetails
                          join atm in _dbContext.AccountTypeMaster on ad.AccountTypeId equals atm.AccountTypeId
                          join us in _dbContext.Users on ad.UserId equals us.UserId
                          where ad.AccountId == AccountId
                          select new AccountDetailResponse
                          {
                              AccountId = ad.AccountId,
                              AccountTypeId = ad.AccountTypeId,
                              UserId = ad.UserId,
                              CurrentBalance = ad.CurrentBalance,
                              Status = ad.Status,
                              OpenDate = ad.OpenDate,
                              ClosedDate = ad.ClosedDate,
                              AccountTypeName = atm.AccountTypeName,
                              UserName = us.FirstName + " " + us.LastName
                          }).FirstOrDefaultAsync();
                if (item == null)
                {
                    return null;
                }
                _logger.LogDebug($"The response for the Get GetAccountDetailList in Account Repo is .{item.AccountId}");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public async void AddAccountDetail(AccountDetails accountDetails)
        {
            _logger.LogDebug("Inside AddAccountDetail in Account Repo endpoint");
            try
            {
                _dbContext.AccountDetails.Add(accountDetails);
                _logger.LogDebug($"The response for the AddAccountDetail in Account Repo is .{accountDetails.AccountId}");
                Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }
        public async void UpdateAccountDetail(AccountDetails accountDetails)
        {
            _logger.LogDebug("Inside UpdateAccountDetail in Account Repo endpoint");
            try
            {
                _dbContext.AccountDetails.Entry(accountDetails).State = EntityState.Modified;
                _logger.LogDebug($"The response for the UpdateAccountDetail in Account Repo is .{accountDetails.AccountId}");
                Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public async Task<IEnumerable<TransactionResponse>> GetTransactionList()
        {
            _logger.LogDebug("Inside GetTransactionList in Account Repo endpoint");
            try
            {
                var item =await (
                       from tr in _dbContext.Transactions
                       join acc in _dbContext.AccountDetails on tr.AccountId equals acc.AccountId
                       join trm in _dbContext.TransactionMode on tr.TransactionModeId equals trm.TransactionModeId
                       join ty in _dbContext.TransactionType on tr.TransactionTypeId equals ty.TransactionTypeId
                       select new TransactionResponse
                       {
                           TransactionId = tr.TransactionId,
                           AccountId = tr.AccountId,
                           Amount = tr.Amount,
                           TransactionTypeId = tr.TransactionTypeId,
                           TransactionDate = tr.TransactionDate,
                           TransactionMode = trm.TransactionModeName,
                           TransactionTypeName = ty.TransactionCode

                       }).ToListAsync();
                if(item.Count() <1 )
                {
                    return null;
                }
                _logger.LogDebug($"The response for the GetTransactionList in Account Repo is .{item.Count()}");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public async Task<TransactionResponse> GetTransactionById(long TransactionId)
        {
            _logger.LogDebug("Inside GetTransactionById in Account Repo endpoint");
            try
            {
                var item = await (
                       from tr in _dbContext.Transactions
                       join acc in _dbContext.AccountDetails on tr.AccountId equals acc.AccountId
                       join trm in _dbContext.TransactionMode on tr.TransactionModeId equals trm.TransactionModeId
                       where tr.TransactionId == TransactionId
                       select new TransactionResponse
                       {
                           TransactionId = tr.TransactionId,
                           AccountId = tr.AccountId,
                           Amount = tr.Amount,
                           TransactionTypeId = tr.TransactionTypeId,
                           TransactionDate = tr.TransactionDate,
                           TransactionMode = trm.TransactionModeName

                       }).FirstOrDefaultAsync();
                if( item == null )
                {
                    return null;
                }
                _logger.LogDebug($"The response for the GetTransactionList in Account Repo is .{item.AccountId}");
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public async Task<string> AddTransaction(Model.Transaction transaction)
        {
            _logger.LogDebug("Inside AddTransaction in Account Repo endpoint");
            try
            {
                var item = _dbContext.AccountDetails.Where(x => x.AccountId == transaction.AccountId).AsNoTracking().FirstOrDefault();
                if (item != null)
                {
                    _dbContext.Transactions.Add(transaction);
                    var account = new AccountDetails();
                    account.AccountId = transaction.AccountId;
                    account.AccountTypeId = item.AccountTypeId;
                    account.Status = item.Status;
                    account.UserId = item.UserId;
                    if (item.CurrentBalance > transaction.Amount)
                        account.CurrentBalance = Convert.ToDouble(item.CurrentBalance - transaction.Amount);
                    else
                        return "Balance Not Sufficient";

                    _dbContext.AccountDetails.Attach(account).Property(x => x.CurrentBalance).IsModified = true;

                    Save();
                    _logger.LogDebug($"The response for the AddTransaction in Account Repo is .{item.AccountId}");
                    return  "Success";
                }
                else
                {
                    return "Account Not Found";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

        public async void UpdateTransaction(Model.Transaction transaction)
        {
            _logger.LogDebug("Inside UpdateTransaction in Account Repo endpoint");
            try
            {
                _dbContext.Entry(transaction).State = EntityState.Modified;
                _logger.LogDebug($"The response for the UpdateTransaction in Account Repo is .{transaction.AccountId}");
                Save();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        //Get All Users
        public async Task<IEnumerable<Users>> GetUsersList()
        {
            _logger.LogDebug("Inside GetUsersList in Account Repo endpoint");
            try
            {
                var res= await _dbContext.Users.ToListAsync();
                _logger.LogDebug($"The response for the GetUsersList in Account Repo is .{res.Count()}");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        //Global Save method
        private void Save()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Users GetLoginUser(string Username, string Password1)
        {
            _logger.LogDebug("Inside GetUsersList in Account Repo endpoint");
            try
            {
                var res =  _dbContext.Users.FirstOrDefault(x => x.Username == Username && x.Password == Password1);
                //var tempUser = myContext.Users.FirstOrDefault(u => u.Username == "bob" && u.Password == "password");

                if (res != null)
                {
                    return res;
                }
                else
                {
                    return null;
                }
                _logger.LogDebug($"The response for the GetUsersList in Account Repo is .{res}");
               // return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }

    }
}
