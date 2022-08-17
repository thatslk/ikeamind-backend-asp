using System;
namespace ikeamind_backend.Core.Services
{
    public static class LinuxTimestampConverter
    {
        public static DateTime LinuxTimestampToDateTime(string timestamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(double.Parse(timestamp)*1000).ToLocalTime();
            return dateTime;
        }
    }
}
