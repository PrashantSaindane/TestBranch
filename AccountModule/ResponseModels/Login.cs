namespace AccountModule.ResponseModels
{
    public class Login
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
    public class JWTTokenResponse
    {
        public string? Token { get; set; }
    }
    public class UserConstants
    {
        public static List<Login> Users = new()
           {
                   new Login(){ UserName="administrator",Password="admin123"}
           };
    }
}
