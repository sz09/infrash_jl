namespace JobLogic.OAuth.Services
{
    public class LoginTrackingResult
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsExpiredPassword { get; set; }
    }
}
