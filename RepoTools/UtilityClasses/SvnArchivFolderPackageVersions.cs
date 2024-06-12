using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RepoTools.UtilityClasses
{
    internal static class SvnArchivFolderPackageVersions
    {
        //Get all Versions from a Package inside the SVN-Archiv Folder
        public static ArrayList GetPackageVersions(string? packageName)
        {
            ArrayList svnArchivFolderPackageVersions = [];

            if (string.IsNullOrEmpty(packageName))
            {
                return svnArchivFolderPackageVersions;
            }


            string svnArchivePath = GlobalVariables.GetSvnArchivePath();
            string svnArchivePackagePath = svnArchivePath + @"\" + packageName;

            if (!Directory.Exists(svnArchivePackagePath))
            {
                string warningMessage = "Der Ordner [" + svnArchivePackagePath + "] existiert nicht.";
                ApplicationWarning.ShowApplicationWarning(warningMessage);
                return svnArchivFolderPackageVersions;
            }

            string[] packageVersions = Directory.GetDirectories(svnArchivePackagePath);

            foreach (string packageVersion in packageVersions)
            {

                //Only save Package Name and not full Path
                string currentPackageVersion = packageVersion.Split("\\").Last();

                //Check if Folder Matches Regex Pattern (4 Digits) 
                string pattern = @"\d{4}$";
                Match m = Regex.Match(currentPackageVersion, pattern);
                if (m.Success)
                {
                    svnArchivFolderPackageVersions.Add(currentPackageVersion);
                }


            }
            Debug.WriteLine(svnArchivFolderPackageVersions);
            return svnArchivFolderPackageVersions;
        }

    }
}
