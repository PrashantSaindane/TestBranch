using AccountModule.Controllers;
using AccountModule.Model;
using AccountModule.Repository;
using AccountModule.ResponseModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AccountModule.Service
{
    public class AccountService:IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountController> _logger;
        public AccountService(IAccountRepository accountRepository, ILogger<AccountController> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<AccountDetailResponse>> GetAccountDetailList()
        {
            //_logger.LogDebug("Inside GetAccountDetailList in AccountService endpoint");
            try
            {
                var item = await _accountRepository.GetAccountDetailList();
                if(item == null && item.Count()<1)
                {
                    return null;
                }
                _logger.LogDebug($"The response for the GetAccountDetailList in AccountService is .{item.Count()}");
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
            _logger.LogDebug("Inside GetAccountDetailById in AccountService endpoint");
            try
            {
                var item = await _accountRepository.GetAccountDetailById(AccountId);
                if(item == null && item.AccountId<0) 
                {
                    return null;
                }
                _logger.LogDebug($"The response for the GetAccountDetailById in AccountService is .{item.AccountId}");
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
            _logger.LogDebug("Inside GetAccountDetailById in AccountService endpoint");
            try
            {
                _accountRepository.AddAccountDetail(accountDetails);
                _logger.LogDebug($"The response for the AddAccountDetail in AccountService is .{accountDetails.AccountId}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }
        public async void UpdateAccountDetail(AccountDetails accountDetails)
        {
            _logger.LogDebug("Inside UpdateAccountDetail endpoint");
            try
            {
                _accountRepository.UpdateAccountDetail(accountDetails);
                _logger.LogDebug($"The response for the UpdateAccountDetail in AccountService is .{accountDetails.AccountId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public async Task<IEnumerable<TransactionResponse>> GetTransactionList()
        {
            _logger.LogDebug("Inside UpdateAccountDetail endpoint");
            try
            {
                var item = await _accountRepository.GetTransactionList();
                if(item == null)
                {
                    return Enumerable.Empty<TransactionResponse>();
                }
                _logger.LogDebug($"The response for the UpdateAccountDetail in AccountService is .{item.Count()}");
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
            _logger.LogDebug("Inside GetTransactionById in AccountService endpoint");
            try
            {
                var item = await _accountRepository.GetTransactionById(TransactionId);
                if (item == null)
                {
                    return null;
                } 
                _logger.LogDebug($"The response for the UpdateAccountDetail in AccountService is .{item.TransactionId}");
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
            _logger.LogDebug("Inside AddTransaction in AccountService endpoint");
            try
            {
                string result = await _accountRepository.AddTransaction(transaction);
                _logger.LogDebug($"The response for the AddTransaction in AccountService is .{result.Count()}");
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public async void UpdateTransaction(Model.Transaction transaction)
        {
            _logger.LogDebug("Inside UpdateTransaction in AccountService endpoint");
            try
            {
                _accountRepository.UpdateTransaction(transaction);
                _logger.LogDebug($"The response for the UpdateTransaction in AccountService is .{transaction.TransactionId}");
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
            _logger.LogDebug("Inside GetUsersList in AccountService endpoint");
            try
            {
                var result= await _accountRepository.GetUsersList();
                _logger.LogDebug($"The response for the GetUsersList in AccountService is .{result.Count()}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
        public Users GetLoginUser(string Username, string Password)
        {
            _logger.LogDebug("Inside GetLoginUser in AccountService endpoint");
            try
            {
                var result = _accountRepository.GetLoginUser(Username, Password);
                _logger.LogDebug($"The response for the GetLoginUser in AccountService is .{result}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }
        }
    }
}
