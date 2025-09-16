using System.Text.RegularExpressions;
namespace gthf
{
    static class gthf
    {
        public static string SanitizeMSG(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9]", "");
        }
    }
}