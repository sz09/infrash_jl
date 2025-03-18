namespace JobLogic.Infrastructure.Log
{
    public class BaseLog
    {
        public BaseLog()
        {
            Type = LogType.Info;
        }

        public LogType Type { get; set; }
    }
}
