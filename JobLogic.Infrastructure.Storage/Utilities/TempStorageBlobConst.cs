
namespace JobLogic.Infrastructure.Storage
{
    public static class TempStorageBlobConst
    {
        public const string Temp1DayContainer = "temp1day";
        public const string Temp3DayContainer = "temp3day";
        public const string Temp7DayContainer = "temp7day";
        public const string Temp30DayContainer = "temp30day";

        public const int Temp1DayExpire = 1;
        public const int Temp3DayExpire = 3;
        public const int Temp7DayExpire = 7;
        public const int Temp30DayExpire = 30;

    }
}
