using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RepoTools
{
    internal static class SvnArchivFolderPackages
    {
        //Get Packages WITH .svn Folder
        public static ArrayList GetPackagesWithSvnFolder()
        {
            ArrayList packageFoldersWithSvnFolder = [];

            string svnArchivePath = GlobalVariables.GetSvnArchivePath();
                
            string[] packageFolders = Directory.GetDirectories(svnArchivePath);

            foreach (string svnFolder in packageFolders)
            {
                if (Directory.Exists(svnFolder + @"\.svn"))
                {
                    //Only save Package Name and not full Path
                    string packageName = svnFolder.Split("\\").Last();
                    packageFoldersWithSvnFolder.Add(packageName);
                }
            }

            return packageFoldersWithSvnFolder;
        }

        //Get Packages WITHOUT .svn Folder
        public static ArrayList GetPackagesWithoutSvnFolder()
        {
            ArrayList packageFoldersWithoutSvnFolder = [];

            string svnArchivePath = GlobalVariables.GetSvnArchivePath();

            string[] packageFolders = Directory.GetDirectories(svnArchivePath);

            foreach (string svnFolder in packageFolders)
            {
                if (!(Directory.Exists(svnFolder + @"\.svn")))
                {
                    //Only save Package Name and not full Path
                    string packageName = svnFolder.Split("\\").Last();
                    packageFoldersWithoutSvnFolder.Add(packageName);
                }
            }

            return packageFoldersWithoutSvnFolder;
        }
    }
}
