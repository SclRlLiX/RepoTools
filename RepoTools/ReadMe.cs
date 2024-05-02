using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoTools
{
    internal class ReadMe
    {
        public SvnCheckInObject? SvnCheckInObject {  get; set; }

        public void CreateAndSaveReadme()
        {
            string readme = $@"
# {SvnCheckInObject?.PackageName} {SvnCheckInObject?.PackageVersion}
            
## Allgemeine Information

Paketname | Version | Software Version | Archiv
--- | --- | ---
**{SvnCheckInObject?.PackageName}** | ``{SvnCheckInObject?.PackageVersion}`` | ``{SvnCheckInObject?.SoftwareVersion}`` | *{SvnCheckInObject?.RepoFolder}*
    
";

            File.WriteAllText("C:\\SVN-Archiv\\readme.md", readme);

            Debug.WriteLine(readme);
        }

    }
}
