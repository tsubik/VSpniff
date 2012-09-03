using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace CheckVSMissingFilesCmdLet
{
    [Cmdlet(VerbsCommon.Find,"MissingFiles")]
    public class CheckVSMissingFilesCmdlet : PSCmdlet
    {

        [Alias("d")]
        [Parameter(Position = 0, Mandatory = true)]
        public string Directory { get; set; }

        #region Some configs

        private string[] excludedExtensions = new string[] { "user", "csproj", "aps", "pch", "vspscc", "vssscc", "ncb", "suo", "tlb", "tlh", "bak", "log", "lib" };
        private string[] excludedDirs = new string[] { "bin", "obj" };

        #endregion

        protected override void ProcessRecord()
        {
            WriteObject("######## Checking for missing references to files started ##############");

            DirectoryInfo dir = new DirectoryInfo(GetUnresolvedProviderPathFromPSPath(Directory));
            WriteObject("Starting in directory: " + dir.ToString());
            LookForProjectFile(dir);
            WriteObject("######## Checking for missing references to files ends ##############");
        }

        private void LookForProjectFile(DirectoryInfo dir)
        {
            FileInfo projectFile = dir.GetFiles().Where(f => f.Extension == ".csproj").FirstOrDefault();
            if (projectFile != null)
            {
                string projectPath = projectFile.DirectoryName;

                XmlDocument doc = new XmlDocument();
                doc.Load(projectFile.FullName);
                XmlNamespaceManager nm = new XmlNamespaceManager(doc.NameTable);
                nm.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

                var projectfiles = doc.SelectNodes("/x:Project/x:ItemGroup/*[self::x:Compile or self::x:Content or self::x:Nones]/@Include", nm)
                    .Cast<XmlNode>()
                    .Select(x => x.Value)
                    .ToArray();

                WriteObject("----Project found : " + projectFile.Name);
                CheckProjectIntegrity(projectfiles, projectPath, dir);
            }
            else
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    LookForProjectFile(subDir);
                }
            }
        }

        private void CheckProjectIntegrity(string[] projectFiles, string projectPath, DirectoryInfo dir)
        {
            string relativeDirPath = dir.FullName.Replace(projectPath + "\\", "");
            if (!excludedDirs.Any(x => relativeDirPath.StartsWith(x)))
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    string relativeFilePath = file.FullName.Replace(projectPath + "\\", "");

                    if (!excludedExtensions.Any(x => x == file.Extension.TrimStart(new char[] { '.' })))
                    {
                        if (!projectFiles.Contains(relativeFilePath))
                        {
                            WriteObject("Potentially missing file " + relativeFilePath);
                        }
                    }
                }
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    CheckProjectIntegrity(projectFiles, projectPath, subDir);
                }
            }
        }
    }
}
