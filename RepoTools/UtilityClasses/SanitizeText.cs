using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoTools.UtilityClasses
{
    internal static class SanitizeText
    {
        public static string GetSanitizedText(string Text)
        {
            string sanitizedText = Text;
            sanitizedText = sanitizedText.Replace("Ä", "AE");
            sanitizedText = sanitizedText.Replace("Ö", "OE");
            sanitizedText = sanitizedText.Replace("Ü", "UE");
            sanitizedText = sanitizedText.Replace("ä", "ae");
            sanitizedText = sanitizedText.Replace("ö", "oe");
            sanitizedText = sanitizedText.Replace("ü", "ue");
            sanitizedText = sanitizedText.Replace("ß", "ss");
            sanitizedText = sanitizedText.Replace("\"", "'");
            sanitizedText = sanitizedText.Replace("\n", " ");
            sanitizedText = sanitizedText.Replace("\r\n", " ");
            return sanitizedText;
        }
    }
}
