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
    internal class SvnArchivFolderPackages
    {
        public ArrayList packageFoldersWithSvnFolder = new ArrayList();
        public ArrayList packageFoldersWithoutSvnFolder = new ArrayList();

        public ArrayList GetPackagesWithSvnFolder()
        {
            string? SvnArchivePath = App.SvnArchivePath;

            if(string.IsNullOrEmpty(SvnArchivePath))
            {
                string errorMessage = "Der Ordner [" + SvnArchivePath + "] existiert nicht. \nDie Anwendung wird beendet.";
                _ = new ApplicationError(ErrorMessage: errorMessage);
                return packageFoldersWithSvnFolder;
            }
                

            Debug.WriteLine("GetPackagesWithSvnFolder");
            string[] packageFolders = Directory.GetDirectories(SvnArchivePath);

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
    }
}
