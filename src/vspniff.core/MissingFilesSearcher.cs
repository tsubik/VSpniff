using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace VSpniff.Core
{
    public class MissingFilesSearcher
    {
        public delegate void StringEventHandler(object sender, string value); 
        public event StringEventHandler MissingFileFound;
        public event StringEventHandler ProjectFound;

        public MissingFilesSearcher()
        {
            
        }

        public void Search(string directoryPath)
        {
            DirectoryInfo dir = new DirectoryInfo(directoryPath);
            LookForProjectFile(dir, Config.Default);
        }

        private void LookForProjectMissingFiles(string[] projectFiles, string projectPath, DirectoryInfo dir, Config currentConfig)
        {
            string relativeDirPath = dir.FullName.Replace(projectPath + "\\", "");
            LookForConfigFileAndMaybeChangeConfiguration(dir, ref currentConfig);
            List<string> excludedDirs = currentConfig.ExcludedDirs.Split(',').Select(x => x.Trim()).ToList();
            List<string> excludedExtensions = currentConfig.ExcludedExtensions.Split(',').Select(x => x.Trim()).ToList();
            if (!excludedDirs.Any(x => relativeDirPath.StartsWith(x)))
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    string relativeFilePath = file.FullName.Replace(projectPath + "\\", "");

                    if (!excludedExtensions.Any(x => x == file.Extension.TrimStart(new char[] { '.' })))
                    {
                        if (!projectFiles.Contains(relativeFilePath))
                        {
                            if (MissingFileFound != null)
                            {
                                MissingFileFound(this, relativeFilePath);
                            }
                        }
                    }
                }
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    LookForProjectMissingFiles(projectFiles, projectPath, subDir, currentConfig);
                }
            }
        }

        private void LookForProjectFile(DirectoryInfo dir, Config currentConfig)
        {
            FileInfo projectFile = dir.GetFiles().Where(f => f.Extension.In(".csproj",".vbproj",".fsproj",".sqlproj")).FirstOrDefault();
            
            LookForConfigFileAndMaybeChangeConfiguration(dir, ref currentConfig);
            
            if (projectFile != null)
            {
                string projectPath = projectFile.DirectoryName;

                XmlDocument doc = new XmlDocument();
                doc.Load(projectFile.FullName);
                XmlNamespaceManager nm = new XmlNamespaceManager(doc.NameTable);
                nm.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003");

                var projectfiles = doc.SelectNodes(@"/x:Project/x:ItemGroup/*[self::x:Compile or self::x:Content or self::x:None or self::x:PostDeploy or self::x:PreDeploy or self::x:RefactorLog
                    or self::x:EmbeddedResource or self::x:Page or self::x:Resource or self::x:CodeAnalysisDictionary or self::x:ApplicationDefinition
                    or self::x:SplashScreen or self::x:DesignData or self::x:DesignDataWithDesignTimeCreatableTypes or self::x:EntityDeploy or self::x:XamlAppDef]/@Include", nm)
                    .Cast<XmlNode>()
                    .Select(x => x.Value)
                    .ToArray();

                if (ProjectFound != null)
                {
                    ProjectFound(this, projectFile.Name);
                }
                LookForProjectMissingFiles(projectfiles, projectPath, dir, currentConfig);
            }
            else
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    LookForProjectFile(subDir, currentConfig);
                }
            }
        }

        private void LookForConfigFileAndMaybeChangeConfiguration(DirectoryInfo currentDir, ref Config currentConfig)
        {
            //look for any file with this extension
            FileInfo configFile = currentDir.GetFiles().Where(f => f.Name.ToLower().EndsWith(".vspniff")).FirstOrDefault();
            if (configFile != null)
            {
                Config newConfig = new Config();
                newConfig.Load(configFile.FullName);
                if (newConfig.Mode == ConfigFileMode.Override)
                {
                    currentConfig = newConfig;
                }
                else if (newConfig.Mode == ConfigFileMode.Append)
                {
                    currentConfig.ExcludedDirs =
                        (currentConfig.ExcludedDirs + newConfig.ExcludedDirs)
                        .Split(',')
                        .Distinct()
                        .ToString(',');

                    currentConfig.ExcludedExtensions =
                        (currentConfig.ExcludedExtensions + newConfig.ExcludedExtensions)
                        .Split(',')
                        .Distinct()
                        .ToString(',');
                }
            }
        }

    }
}
