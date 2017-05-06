namespace Jericho.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public static string ToTimeStamp(this DateTime dateTime)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException(nameof(dateTime));
            }

            return dateTime.ToString("yyyyMMddHHmmssffff");
        }
    }
}
