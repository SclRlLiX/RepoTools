using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoTools
{
    internal class SvnCheckInObject
    {
        public bool? NewPackage { get; set; }
        public string? RepoFolder { get; set; }
        public string? PackageName { get; set; }
        public string? PackageVersion { get; set; }
        public bool DcsEntw { get; set; }
        public bool DcsTest { get; set; }
        public bool DcsProd { get; set; }
        public bool Stvmv { get; set; }
        public bool Sccm {  get; set; }
        public string? OrderId { get; set; } = GlobalVariables.EmptyTextboxText;
        public string? PackageDescription { get; set; }
        public string? SoftwareVersion { get; set; } = GlobalVariables.EmptyTextboxText;
        public bool? AddToMail { get; set; }
        public bool? ValidationError { get; set; } = false;

        readonly public string? UserName  = Environment.UserName;

    }
}
