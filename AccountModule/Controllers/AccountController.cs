using System.Transactions;
using AccountModule.ApiResponse;
using AccountModule.Model;
using AccountModule.ResponseModels;
using AccountModule.Service;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace AccountModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _account;
        private readonly ILogger<AccountController> _logger;   
        public AccountController(IAccountService account, ILogger<AccountController> logger)
        {
            _account = account;
            _logger = logger;
        }
        /// <summary>
        /// Get Account Detail List
        /// </summary>
        /// <param></param>
        /// <returns>Return Account Detail List</returns>
        [HttpGet]
        [Route("GetAccountDetailList")]
        public async Task<ActionResult> Get()
        {
           // _logger.LogDebug("Inside Get GetAccountDetailList endpoint");
            var accountDetailList = new List<AccountDetailResponse>();
            try
            {
                 accountDetailList = (await _account.GetAccountDetailList()).ToList();
                if (accountDetailList.Count > 0)
                {
                    var response = new ApiResponse<dynamic>
                    {
                        Status = Constants.Success,
                        Message = Constants.Get,
                        Data = accountDetailList
                    };
                    _logger.LogDebug($"The response for the Get GetAccountDetailList is .{accountDetailList.Count()}");
                    //return new OkObjectResult(accountDetailList);
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponse<dynamic>
                    {
                        Status = Constants.Badrequest,
                        Message = Constants.Failure,
                    };
                    _logger.LogError($"The response for the Get GetAccountDetailList is .{accountDetailList.Count()}");
                    return NotFound(response);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Get Single Account Detail 
        /// </summary>
        /// <param>pass AccountId</param>
        /// <returns>Return Added Account Detail</returns>
        [HttpGet]
        [Route("GetAccountDetail")]
        public async Task<ActionResult> GetAccountDetailByid(long AccountId)
        {
          //  _logger.LogDebug("Inside GetAccountDetailByid endpoint");

            try
            {
                if (AccountId <= 0)
                {
                    var response = new ApiResponse<dynamic>
                    {
                        Status = Constants.Badrequest,
                        Message = Constants.Failure,
                    };
                    _logger.LogError($"The response for the get user is .{response.Status}");
                    return NotFound(response);
                }
                else
                {
                    var result = await _account.GetAccountDetailById(AccountId);
                    if (result != null)
                    {
                        var response = new ApiResponse<dynamic>
                        {
                            Status = Constants.Success,
                            Message = Constants.Get,
                            Data = result
                        };
                        _logger.LogDebug($"The response for the GetAccountDetailByid is .{result.AccountId}");
                        return Ok(response);
                    }
                    else
                    {
                        var response = new ApiResponse<dynamic>
                        {
                            Status = Constants.Badrequest,
                            Message = Constants.Failure,
                        };
                        _logger.LogError($"The response for the get user is .{response.Status}");
                        return NotFound(response);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
                
            }
        }


        /// <summary>
        /// Add Account Detail 
        /// </summary>
        /// <param>AccountTypeId,CurrentBalance,Status,OpenDate,ClosedDate,UserId as model</param>
        /// <returns>Return Account Detail</returns>
        [HttpPost]
        [Route("AddAccountDetail")]
        public async Task<ActionResult> AddAccountDetail([FromBody] AccountDetails accountDetails)
        {
            _logger.LogDebug("Inside AddAccountDetail endpoint");
            try
            {
                if (accountDetails.AccountId < 1)
                {
                    if (accountDetails.UserId != 0 && accountDetails.AccountTypeId != 0)
                    {
                        using (var scope = new TransactionScope())
                        {
                            _account.AddAccountDetail(accountDetails);
                            scope.Complete();
                            var response = new ApiResponse<dynamic>
                            {
                                Status = Constants.Success,
                                Message = Constants.Get,
                                Data = accountDetails
                            };
                            _logger.LogDebug($"The response for the AddAccountDetail is .{accountDetails.AccountId}");
                            return Ok(response);

                        }
                    }
                    else
                    {
                        var response = new ApiResponse<dynamic>
                        {
                            Status = Constants.Badrequest,
                            Message = Constants.Failures,
                        };
                        _logger.LogError($"The response for the get user is .{response.Status}");
                        return NotFound(response);
                    }
                }
                else
                {
                    var response = new ApiResponse<dynamic>
                    {
                        Status = Constants.Badrequest,
                        Message = "You can't sent request with Primary Key which is AccountId",
                    };
                    _logger.LogError($"The response for the get user is .{response.Status}");
                    return NotFound(response);
                }

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Update Account Detail record
        /// </summary>
        /// <param>model object accountDetails</param>
        /// <returns>Return Success </returns>

        [HttpPut]
        [Route("UpdateAccountDetail")]
        public async Task<ActionResult> UpdateAccountDetail([FromBody] AccountDetails accountDetails)
        {
            _logger.LogDebug("Inside UpdateAccountDetail endpoint");
            try
            {
                if (accountDetails.AccountId > 0 && accountDetails.UserId > 0)
                {
                    _account.UpdateAccountDetail(accountDetails);
                    var response = new ApiResponse<dynamic>
                    {
                        Status = Constants.Success,
                        Message = Constants.Update,
                        Data = accountDetails
                    };
                    _logger.LogDebug($"The response for the UpdateAccountDetail is .{accountDetails.AccountId}");
                    return Ok(response);

                }
                else
                {
                    var response = new ApiResponse<dynamic>
                    {
                        Status = Constants.Badrequest,
                        Message = Constants.Failure,
                    };
                    _logger.LogError($"The response for the UpdateAccountDetail is .{accountDetails.AccountId}");
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get All Transaction List
        /// </summary>
        /// <param></param>
        /// <returns>Return Transaction List</returns>
        [Route("GetTransactionList")]
        [HttpGet]
        public async Task<ActionResult> TransactionList()
        {
            _logger.LogDebug("Inside GetTransactionList endpoint");
            try
            {
                var transactionList =(await _account.GetTransactionList()).ToList();
                if (transactionList.Count > 0)
                {
                    var response = new ApiResponse<dynamic>
                    {
                        Status = Constants.Success,
                        Message = Constants.Get,
                        Data = transactionList
                    };
                    _logger.LogDebug($"The response for the GetTransactionList is .{transactionList.Count()}");
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponse<dynamic>
                    {
                        Status = Constants.Badrequest,
                        Message = "No Data Found",
                    };
                    _logger.LogError($"The response for the GetTransactionList is .{transactionList.Count()}");
                    return NotFound(response);
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get All Transaction List
        /// </summary>
        /// <param></param>
        /// <returns>Return Transaction List</returns>
        [Route("GetTransaction")]
        [HttpGet]
        public async Task<ActionResult> GetTransaction(long TransactionId)
        {
            _logger.LogDebug("Inside GetTransaction endpoint");
            try
            {
                if (TransactionId <= 0)
                {
                    var response = new ApiResponse<dynamic>
                    {
                        Status = "Data is not available for this TransactionId",
                        Message = "Please Provide the valid TransactionId",
                    };
                    _logger.LogError($"The response for the GetTransaction is .{response.Status}");
                    return NotFound(response);
                }
                else
                {
                    var transactionList = await _account.GetTransactionById(TransactionId);
                    if (transactionList != null)
                    {
                        var response = new ApiResponse<dynamic>
                        {
                            Status = Constants.Success,
                            Message = Constants.Get,
                            Data = transactionList
                        };
                        _logger.LogDebug($"The response for the GetTransaction is .{transactionList.TransactionId}");
                        return Ok(response);
                    }
                    else
                    {
                        var response = new ApiResponse<dynamic>
                        {
                            Status = Constants.Badrequest,
                            Message = "No Data Found",
                        };
                        _logger.LogError($"The response for the GetTransaction is .{response.Status}");
                        return NotFound(response);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new NoContentResult();
            }
        }

        /// <summary>
        /// Add Transaction
        /// </summary>
        /// <param>transaction</param>
        /// <returns>Return Transaction</returns>
        [HttpPost]
        [Route("AddTransaction")]
        public async Task<ActionResult> AddTransaction([FromBody] Model.Transaction transaction)
        {
            _logger.LogDebug("Inside AddTransaction endpoint");
            try
            {
                if (transaction.TransactionId < 1)
                {
                    if (transaction.AccountId != 0)
                    {
                        using (var scope = new TransactionScope())
                        {
                            var result = await _account.AddTransaction(transaction);
                            if (result == "Success")
                            {
                                scope.Complete();
                                var response = new ApiResponse<dynamic>
                                {
                                    Status = Constants.Success,
                                    Message = Constants.Add,
                                    Data = result
                                };
                                _logger.LogDebug($"The response for the AddTransaction is .{result.Count()}");
                                return Ok(response);
                            }
                            else
                            {
                                return new JsonResult(result);
                            }
                        }

                    }
                    else
                    {
                        var response = new ApiResponse<dynamic>
                        {
                            Status = "Data is not available for this TransactionId",
                            Message = "Please provide the valid TransactionId",
                        };
                        _logger.LogError($"The response for the AddTransaction is .{response.Status}");
                        return NotFound(response);
                    }
                }
                else
                {
                    var response = new ApiResponse<dynamic>
                    {
                        Status = Constants.Badrequest,
                        Message = "You can't sent request with Primary Key which is TransactionId",
                    };
                    _logger.LogError($"The response for the AddTransaction is .{response.Status}");
                    return NotFound(response);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new NoContentResult();
            }

        }

        /// <summary>
        /// Update transaction
        /// </summary>
        /// <param>transaction</param>
        /// <returns>Return Transaction List</returns>
        [HttpPut]
        [Route("UpdateTransaction")]
        public async Task<ActionResult> UpdateTransaction([FromBody] Model.Transaction transaction)
        {
            _logger.LogDebug("Inside UpdateTransaction endpoint");
            try
            {
                if (transaction.TransactionId != 0 && transaction.AccountId != 0)
                {
                    _account.UpdateTransaction(transaction);
                    var response = new ApiResponse<dynamic>
                    {
                        Status = Constants.Success,
                        Message = Constants.Update,
                        Data = transaction
                    };
                    _logger.LogDebug($"The response for the UpdateTransaction is .{transaction.TransactionId}");
                    return Ok(response);

                }
                else
                {
                    var response = new ApiResponse<dynamic>
                    {
                        Status = "Data is not available for this TransactionId",
                        Message = "Please provide the valid TransactionId",
                    };
                    _logger.LogError($"The response for the UpdateTransaction is .{response.Status}");
                    return NotFound(response);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);;
                return new NoContentResult();
            }
            return new NoContentResult();
        }

    }
}
