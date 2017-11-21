/*******************************************
 * 
 * Marcos Tito de Pardo Marques
 * https://www.linkedin.com/in/mrctito/
 * 
 ********************************************/
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BotLib
{
    [Serializable]
    public static class Utilities
    {
        public static DateUtils DateEx = new DateUtils();
        public static EnumUtils EnumEx = new EnumUtils();
    }

    [Serializable]
    public class DateUtils
    {
        private const string regstr1 = @"(3[01] | 2\d | 1\d | 0 ?\d)(de)(jan ?| fev ?| mar ?| abr ?| mai ?| jun ?| jul ?| ago ?| set ?|out ?| nov ?| dez) ? (de) ? ( \d{4}))";
        private const string regstr2 = @"(3[01] | 2\d | 1\d | 0 ?\d)(de)(janeiro ?| fevereiro ?| março ?| abril ?| maio ?| junho ?| julho ?| agosto ?| setembro ?|outubro ?| novembro ?| dezembro ?) ? (de) ? ( \d{4}))";

        public bool IsMatch(string dateStr)
        {
            Regex regex = new Regex(regstr1);
            Match match = regex.Match(dateStr);
            if (match.Success)
            {
                return true;
            }

            regex = new Regex(regstr2);
            match = regex.Match(dateStr);
            if (match.Success)
            {
                return true;
            }

            string format = "dd/MM/yyyy";
            var provider = new CultureInfo("pt-BR");
            try
            {
                DateTime.ParseExact(dateStr, format, provider);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool ConvertDateStringToDate(string dateStr, out DateTime resultDate)
        {
            string[] format = new string[] {    "dd/MM/yyyy", "dd-MM-yyyy", "dd MM yyyy",
                                                "d/MM/yyyy", "d-MM-yyyy", "d MM yyyy",
                                                "dd/MMM/yyyy", "dd-MMM-yyyy", "dd MMM yyyy",
                                                "d/MMM/yyyy", "d-MMM-yyyy", "d MMM yyyy",
                                                "dd/MMMM/yyyy", "dd-MMMM-yyyy", "dd MMMM yyyy",
                                                "d/MMMM/yyyy", "d-MMMM-yyyy", "d MMMM yyyy",
                                                "dd 'de' MMM 'de' yyyy", "dd 'de' MMMM 'de' yyyy"
                                            };
            var provider = new CultureInfo("pt-BR");
            return DateTime.TryParseExact(dateStr.Trim(), format,
                 provider, DateTimeStyles.AllowWhiteSpaces, out resultDate);
        }

        public bool ConvertTimeStringToTime(string timeStr, out DateTime resultTime)
        {
            string[] format = new string[] { "HH:mm" };
            var provider = new CultureInfo("pt-BR");
            return DateTime.TryParseExact(timeStr.Trim(), format, provider, DateTimeStyles.NoCurrentDateDefault, out resultTime);
        }


        public bool ConvertDateTimeStringToDateTime(string dateStr, out DateTime resultDateTime)
        {
            string[] format = new string[] {    "dd/MM/yyyy HH:mm",
                                                "dd-MM-yyyy HH:mm",
                                                "dd MM yyyy HH:mm",
                                                "d/MM/yyyy HH:mm",
                                                "d-MM-yyyy HH:mm",
                                                "d MM yyyy HH:mm",
                                                "dd/MMM/yyyy HH:mm",
                                                "dd-MMM-yyyy HH:mm",
                                                "dd MMM yyyy HH:mm",
                                                "d/MMM/yyyy HH:mm",
                                                "d-MMM-yyyy HH:mm",
                                                "d MMM yyyy HH:mm",
                                                "dd/MMMM/yyyy HH:mm",
                                                "dd-MMMM-yyyy HH:mm",
                                                "dd MMMM yyyy HH:mm",
                                                "d/MMMM/yyyy HH:mm",
                                                "d-MMMM-yyyy HH:mm",
                                                "d MMMM yyyy HH:mm",
                                                "dd 'de' MMM 'de' yyyy HH:mm",
                                                "dd 'de' MMMM 'de' yyyy HH:mm"
                                            };
            var provider = new CultureInfo("pt-BR");
            return DateTime.TryParseExact(dateStr.Trim(), format, provider, DateTimeStyles.None, out resultDateTime);
        }

    }

    [Serializable]
    public class EnumUtils
    {
        public T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }

}