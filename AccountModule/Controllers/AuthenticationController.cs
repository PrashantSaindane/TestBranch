using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AccountModule.ResponseModels;
using Newtonsoft.Json;
using AccountModule.Service;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AccountModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccountService _account;
        private readonly IConfiguration _config;
        private readonly ILogger<AccountController> _logger;
        public AuthenticationController(IConfiguration config, ILogger<AccountController> logger, IAccountService account)
        {
            _config = config;
            _logger = logger;
            _account = account;
        }

        [AllowAnonymous]
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] Login user)
        {
            _logger.LogDebug("Inside login endpoint");
            try
            {
                var user1 = Authenticate(user);
                if (user1 != null)
                {
                    var token1 = GenerateToken(user1);
                    AuthResponseModel response = new AuthResponseModel()
                    {
                        Data = new { token = token1 },
                        Statuscode = 200,
                        Error = "",
                        Warning = ""
                    };
                    _logger.LogDebug($"The response for the login is .{user1.UserName}");
                    return new JsonResult(response);
                }
                return new NotFoundResult() ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new JsonResult("Something went wrong");
            }
            // return JsonConvert.SerializeObject(new AuthResponseModel() { Data = "", Statuscode = 0, Error = "user not found", Warning = "" });
          //  return new NotFound();
        }

        // To generate token
        private string GenerateToken(Login user)
        {
           
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim(ClaimTypes.Role,"admin")
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        //To authenticate user
        private Login Authenticate(Login user)
        {
            //var currentUser = UserConstants.Users.FirstOrDefault(x => x.UserName.ToLower() ==
            //    user.UserName.ToLower() && x.Password == user.Password);
            var currentUser = _account.GetLoginUser(user.UserName, user.Password);
            if (currentUser != null)
            {
                Login login = new Login();
                login.UserName = user.UserName;
                login.Password = user.Password;
                return login;
            }
            return null;
        }
    }
}
