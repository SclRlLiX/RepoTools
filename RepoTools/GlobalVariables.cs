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


        public static string GetSvnArchivePath()
        {
            string SvnArchivePath; 

            if (Debug)
            {
                SvnArchivePath = @"C:\SVN-Archiv";
            }
            else if (Test)
            {
                SvnArchivePath = @"C:\SVN-Archiv-Test";
            }
            else
            {
                SvnArchivePath = @"C:\SVN-Archiv";
            }

            return SvnArchivePath;
        }

        public static string GetSvnArchiveUrl()
        {
            string SvnArchiveUrl;

            if (Debug)
            {
                SvnArchiveUrl = @"https://WORK-VM-W11.cis-test.dcstest.de/svn/Archiv/";
            }
            else if (Test)
            {
                SvnArchiveUrl = @"https://S7701170.sis-entw.dcsentw.de/svn/Archiv-TEST/";
            }
            else
            {
                SvnArchiveUrl = @"https://S7701170.sis-entw.dcsentw.de/svn/Archiv/";
            }

            return SvnArchiveUrl;
        }

    }
}
