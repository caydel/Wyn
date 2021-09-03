using System;

using Wyn.Utils.Attributes;
using Wyn.Utils.Extensions;
using Wyn.Utils.Constant;

namespace Wyn.Utils.Helpers
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 时间戳起始日期
        /// </summary>
        public static DateTime TimestampStart = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="milliseconds">是否使用毫秒</param>
        /// <returns>时间戳</returns>
        public static string GetTimeStamp(bool milliseconds = false)
        {
            var ts = DateTime.UtcNow - TimestampStart;
            return Convert.ToInt64(milliseconds ? ts.TotalMilliseconds : ts.TotalSeconds).ToString();
        }

        /// <summary>判断当前年份是否是闰年</summary>
        /// <param name="year">年份</param>
        /// <returns>真或假</returns>
        public static bool IsLeapYear(int year)
        {
            var n = year;
            return (n % 400 == 0) || (n % 4 == 0 && n % 100 != 0);
        }

        /// <summary>
        /// 获得服务器当前时间是周几
        /// </summary>
        /// <returns>星期几</returns>
        public static string GetWeek()
        {
            var dayOfWeek = DateTime.Now.DayOfWeek.ToInt();
            var week = dayOfWeek switch
            {
                0 => DateConstant.Sunday,
                1 => DateConstant.Monday,
                2 => DateConstant.Tuesday,
                3 => DateConstant.Wednesday,
                4 => DateConstant.Thursday,
                5 => DateConstant.Friday,
                _ => DateConstant.Saturday
            };
            return week;
        }
    }
}
