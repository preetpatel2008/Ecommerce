namespace Repository.Services.Infrastructure
{
    public class RegularExpressions
    {
        public static string AllowMultipleEmailRegex = @"/^([\w+-.%]+@[\w-.]+\.[A-Za-z]{2,4};?)*$/";
        public static string regexEmailValid = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        public static string IFCICOdeRegex = @"[A-Z|a-z]{4}[0][\d]{6}$"; //^[^\s]{4}\d{7}$
        public static string PANRegex = @"[A-Z]{5}\d{4}[A-Z]{1}";
        public static string MobileRegex = @"^[\w]{3}(p|P|c|C|h|H|f|F|a|A|t|T|b|B|l|L|j|J|g|G)[\w][\d]{4}[\w]$";

        public static string AllowLettersOnly = @"/^[a-zA-Z]*$/";
        public static string AllowCityOnly = @"/^[a-zA-Z ]*$/";
        public static string AllowLettersWithSpaceOnly = @"/^[a-zA-Z ]*$/";
        public static string AllowPhoneOnly = @"/^[0-9 ,]*$/";
        public static string AllowMobileOnly = @"/^[0-9]*$/";
        public static string AllowPinCodeOnly = @"/^[0-9]*$/";
        public static string BlockHtmlTagRegex = @"/<(.*?)>/gi";
        public static string ScriptTagRegex = "<script[^/][^>]*>(.*?)</script[^>]*>|<javascript[^/][^>]*>(.*?)</javascript[^>]*>";

        public static string AllowMinMax8Char = @"/^[a-zA-Z0-9]{8}$/";
        public static string AllowMinMax8Number = @"/^[0-9]{8}$/";
        public static string AllowMinMax16Number = @"/^[0-9]{16}$/";
    }
}
