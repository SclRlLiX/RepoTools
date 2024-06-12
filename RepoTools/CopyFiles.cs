using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoTools
{
    internal static class CopyFiles
    {
        public static bool StartCopyFiles(DirectoryInfo source, DirectoryInfo target)
        {
            if (!(Directory.Exists(target.FullName)))
            {
                Directory.CreateDirectory(target.FullName);
            }

            foreach (var file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name), true);
            }

            foreach (var sourceSubdirectory in source.GetDirectories())
            {
                var targetSubdirectory = target.CreateSubdirectory(sourceSubdirectory.Name);
                StartCopyFiles(sourceSubdirectory, targetSubdirectory);
            }

            if(Directory.Exists(target.FullName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
