namespace JobLogic.OAuth.Services
{
    public class UserLockingStatusResultModel
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string UserId { get; set; }
    }
}
