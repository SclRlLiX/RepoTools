﻿using System.Diagnostics;

namespace RepoTools
{
    internal static class GlobalVariables
    {
        //Enable / Disable DEBUG Mode 
        public static  bool Debug = true;
        public static bool Test = false;

        //Default text for empty Text Boxes 
        readonly public static string EmptyTextboxText = "Keine Angabe";

        //Default file name for JSON 
        readonly public static string JsonFileName = "package_info.json";


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
                SvnArchiveUrl = @"https://work-vm-w11.cis-test.dcstest.de/svn/Archiv";
            }
            else if (Test)
            {
                SvnArchiveUrl = @"https://s7701170.sis-entw.dcsentw.de/svn/Archiv-TEST";
            }
            else
            {
                SvnArchiveUrl = @"https://s7701170.sis-entw.dcsentw.de/svn/Archiv";
            }

            return SvnArchiveUrl;
        }

        public static string GetDriveO()
        {
            string DriveO;
            if (Debug)
            {
                DriveO = @"C:\LW_O";
            }
            else
            {
                DriveO = @"\\tsclient\o";
            }

            return DriveO;
        }

        public static string GetTutorialPathUrl()
        {
            string TutorialPathUrl;

            if (Debug)
            {
                TutorialPathUrl = @"https://work-vm-w11.cis-test.dcstest.de/!/#RepoToolsTutorial/view/head/RepoTools_Handbuch.pdf";
            }
            else if (Test)
            {
                TutorialPathUrl = @"https://s7701170.sis-entw.dcsentw.de/!/#RepoTools/view/head/RepoTools-Anwendung/Handbuch/RepoTools_Handbuch.pdf";
            }
            else
            {
                TutorialPathUrl = @"https://s7701170.sis-entw.dcsentw.de/!/#RepoTools/view/head/RepoTools-Anwendung/Handbuch/RepoTools_Handbuch.pdf";
            }

            return TutorialPathUrl;

        }

    }
}
