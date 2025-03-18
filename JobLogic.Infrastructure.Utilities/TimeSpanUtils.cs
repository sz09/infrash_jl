using System;

namespace JobLogic.Infrastructure.Utilities
{
    public static class TimeSpanUtils
    {
        public static TimeSpanWithHours_JobLogic ConvertToTimeSpanWithHours(int totalMinutes)
        {
            var timeSpan = new TimeSpanWithHours_JobLogic();

            if (totalMinutes > 0)
            {
                var elapsedTime = new TimeSpan(0, totalMinutes, 0);
                timeSpan.TotalHours = (int)elapsedTime.TotalHours;
                timeSpan.Minutes = elapsedTime.Minutes;
            }

            return timeSpan;
        }

        public static TimeSpan_JobLogic ConvertToTimeSpan(int totalMinutes)
        {
            var timeSpan = new TimeSpan_JobLogic();

            if (totalMinutes > 0)
            {
                var elapsedTime = new TimeSpan(0, totalMinutes, 0);
                timeSpan.TotalDays = elapsedTime.Days;
                timeSpan.TotalHours = elapsedTime.Hours;
                timeSpan.TotalMinutes = elapsedTime.Minutes;
            }

            return timeSpan;
        }

        public static int GetTotalMinutesFromTimeSpan(int totalDays, int totalHours, int totalMinutes)
        {
            int minutes = 0;

            if (totalDays > 0 || totalHours > 0 || totalMinutes > 0)
            {
                var elapsedTime = new TimeSpan(totalDays, totalHours, totalMinutes, 0);
                minutes = Convert.ToInt32(Math.Floor(elapsedTime.TotalMinutes));
            }
            return minutes;
        }

        public static string GetTimeSpanFormatted(int totalMinutes)
        {
            var priorityTimeSpan = "0d 0h 0m";

            if (totalMinutes != 0)
            {
                var elapsedTime = new TimeSpan(0, totalMinutes, 0);
                var TotalDays = elapsedTime.Days;
                var TotalHours = elapsedTime.Hours;
                var TotalMinutes = elapsedTime.Minutes;
                priorityTimeSpan = $"{TotalDays}d {TotalHours}h {TotalMinutes}m ";
                return priorityTimeSpan;
            }

            return priorityTimeSpan;
        }

    }

    public class TimeSpan_JobLogic
    {
        public int TotalDays { get; set; }
        public int TotalHours { get; set; }
        public int TotalMinutes { get; set; }
    }

    public class TimeSpanWithHours_JobLogic
    {
        public int TotalHours { get; set; }
        public int Minutes { get; set; }
    }
}
