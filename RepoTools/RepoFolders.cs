using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoTools
{
    internal static class RepoFolders
    {
        public static string[] GetRepoFolders()
        {
            string[] repoFolders = [
                                    "alle PARAMs/MV",
                                    "alle PARAMs/NDL",
                                    "CERT",
                                    "DATAPORT_intern",
                                    "HB",
                                    "HH",
                                    "ITU",
                                    "MV",
                                    "NDL",
                                    "NI",
                                    "SH",
                                    "ST",
                                    "UNIFA"
                                    ];

            return repoFolders;
        }
    }
}
