using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoTools.UtilityClasses
{
    internal class SvnProcessObject
    {
        public string? StandardOutput { get; set; }
        public string? ErrorOutput { get; set; }
        public int ExitCode { get; set; }
    }
}
