using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoTools
{
    internal class SvnCheckInObject
    {
        public int? ChooseOption { get; set; }
        public string? RepoFolder { get; set; }
        public string? PackageName { get; set; }
        public string? PackageVersion { get; set; }
        public bool? DcsEntw { get; set; }
        public bool? DcsTest { get; set; }
        public bool? DcsProd { get; set; }
        public bool? Stvmv { get; set; }
        public bool? Sccm {  get; set; }
        public string? OrderId { get; set; }
        public string? PackageDescription { get; set; }
        public string? SoftwareVersion { get; set; }
        public bool? AddToMail { get; set; }
        public bool? ValidationError { get; set; } = false;

        private string? userName;
        public string? UserName
        {
            get { return userName; }
            set { userName = Environment.UserName; }
        }
    }
}
