using System;

namespace Assets._Game._Scripts._4_Services {

    // Сервис для перевода денег в сокращенный формат и отображения его в UI
    public static class NumberFormatterService
    {
        public static string FormatNumber(long num)
        {
            if (num < 1000)
                return num.ToString();

            int exp = (int)(Math.Log(num) / Math.Log(1000));
            var abbreviation = new string[] { "k", "m", "b", "t" };

            return string.Format("{0:0.##}{1}", num / Math.Pow(1000, exp), abbreviation[exp - 1]);
        }
    }
}
