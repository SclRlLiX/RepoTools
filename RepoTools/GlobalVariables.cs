using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoTools
{
    internal static class GlobalVariables
    {
        //Enable / Disable DEBUG Mode 
        public static  bool Debug = true;
        public static bool Test = false;

        public static string? SvnArchivePath;
        public static string? SvnArchiveUrl; 



         if (Debug)
            {
                SvnArchivePath = @"C:\SVN-Archiv";
                SvnArchiveUrl = @"https://WORK-VM-W11.cis-test.dcstest.de/svn/Archiv/";
            }
            else if (Test)
            {
                SvnArchivePath = @"C:\SVN-Archiv-Test";
                SvnArchiveUrl = @"https://S7701170.sis-entw.dcsentw.de/svn/Archiv-TEST/";
            }
            else
        {
            SvnArchivePath = @"C:\SVN-Archiv";
            SvnArchiveUrl = @"https://S7701170.sis-entw.dcsentw.de/svn/Archiv/";
        }
    }
}
