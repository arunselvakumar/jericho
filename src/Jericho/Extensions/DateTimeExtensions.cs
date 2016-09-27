using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jericho.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToTimeStamp(this DateTime dateTime)
        {
            if(dateTime == null)
            {
                throw new ArgumentNullException(nameof(dateTime));
            }

            return dateTime.ToString("yyyyMMddHHmmssffff");
        }
    }
}
