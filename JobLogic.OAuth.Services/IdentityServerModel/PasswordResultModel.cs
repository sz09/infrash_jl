namespace JobLogic.OAuth.Services
{
    public class PasswordResultModel
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
    }
}
