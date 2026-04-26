using System;
using System.Collections.Generic;
using System.Text;

namespace Appliction.Helper
{
    public static class DateHelper
    {
        public static string Format(DateTime date)
        {
            var jordanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Jordan Standard Time");
            var jordanTime = TimeZoneInfo.ConvertTimeFromUtc(date, jordanTimeZone);

            return jordanTime.ToString("dd-MM-yyyy hh:mm tt");
        }

    }
}
