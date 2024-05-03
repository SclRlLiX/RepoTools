using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace RepoTools
{
    internal class ReadMe
    {
        public SvnCheckInObject? SvnCheckInObject {  get; set; }
        public string? PathToPackageVersion { get; set; }

        //Returns true on success
        public bool CreateAndSaveReadme()
        {
            if(SvnCheckInObject == null)
            {
                ApplicationError.ShowApplicationError(@"Die Variable [SvnCheckInObject] ist null.");
                return false; 
            }
            if(String.IsNullOrEmpty(PathToPackageVersion))
            {
                ApplicationError.ShowApplicationError(@"Die Variable [PathToPackageVersion] ist null.");
                return false;
            }

            //Set icons
            string dcsEntw = SvnCheckInObject.DcsEntw ? "&#9989;" : "&#10060;";
            string dcsTest = SvnCheckInObject.DcsTest ? "&#9989;" : "&#10060;";
            string dcsProd = SvnCheckInObject.DcsProd ? "&#9989;" : "&#10060;";
            string stvmv = SvnCheckInObject.Stvmv ? "&#9989;" : "&#10060;";
            string sccm = SvnCheckInObject.Sccm ? "&#9989;" : "&#10060;";


            string readme = $@"
# {SvnCheckInObject?.PackageName} {SvnCheckInObject?.PackageVersion}
            
## Allgemeine Information

Paketname | Version | Software Version | Archiv
--- | --- | --- | ---
**{SvnCheckInObject?.PackageName}** | ``{SvnCheckInObject?.PackageVersion}`` | ``{SvnCheckInObject?.SoftwareVersion}`` | *{SvnCheckInObject?.RepoFolder}*


## Information zur Version

Bearbeiter | Erstellt am | Auftragsnummer
--- | --- | --- 
**{SvnCheckInObject?.UserName}** | {DateTime.Now.ToString("dd.MM.yyyy HH:mm")} | ``{SvnCheckInObject?.OrderId}`` 


## Bemerkung 
{SvnCheckInObject?.PackageDescription}


## Umgebungen 

*Stand: {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}*

DCSENTW | DCSTEST | DCSPROD | STVMV | SCCM
:---: | :---: | :---: | :---: | :---: 
{dcsEntw} | {dcsTest} | {dcsProd} | {stvmv} | {sccm} 
";

            //save readme to file
            string readmeFileName = PathToPackageVersion + @"\readme.md";
            File.WriteAllText(readmeFileName, readme, Encoding.UTF8);

            if(!(File.Exists(readmeFileName)))
            {
                ApplicationError.ShowApplicationError($@"Die Datei [{readmeFileName}] wurde nicht angelegt.");
                return false; 
            }

            return true;

        }

        //Returns true on success
        public bool CreateAndSaveJson()
        {
            if (SvnCheckInObject == null)
            {
                ApplicationError.ShowApplicationError(@"Die Variable [SvnCheckInObject] ist null.");
                return false;
            }
            if (String.IsNullOrEmpty(PathToPackageVersion))
            {
                ApplicationError.ShowApplicationError(@"Die Variable [PathToPackageVersion] ist null.");
                return false;
            }

            //Create JSON 
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement),
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(SvnCheckInObject, options);

            //save JSON to file 
            string jsonFileName = PathToPackageVersion + @"\" + GlobalVariables.JsonFileName; 
            File.WriteAllText(jsonFileName, jsonString, Encoding.UTF8);

            if (!(File.Exists(jsonFileName)))
            {
                ApplicationError.ShowApplicationError($@"Die Datei [{jsonFileName}] wurde nicht angelegt.");
                return false;
            }

            return true;
        }

    }
}
