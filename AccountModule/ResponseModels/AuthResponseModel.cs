namespace AccountModule.ResponseModels
{
    public class AuthResponseModel
    {
        public object Data { get; set; }
        public int Statuscode { get; set; }
        public string Error { get; set; }
        public string Warning { get; set; }

    }
}
