using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoTools
{
    internal static class SanitizeText
    {
        public static string GetSanitizedText(string Text)
        {
            string sanitizedText = Text;

            sanitizedText = sanitizedText.Replace("\"", "'");

            return sanitizedText;
        }
    }
}
