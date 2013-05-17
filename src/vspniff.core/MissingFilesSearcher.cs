using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace VSpniff.Core
{
    public class MissingFilesSearcher
    {
        public delegate void StringEventHandler(object sender, string value); 
        public event StringEventHandler NewMessage;

		private void GenerateMessage(string message)
		{
			if (NewMessage != null)
			{
				NewMessage(this, message);
			}
		}

        public MissingFilesSearcher()
        {
            
        }

		public void Search(string directoryPath)
		{
			if (Directory.Exists(directoryPath))
			{
				GenerateMessage("######## Checking for missing references to files started ##############");
				GenerateMessage("Starting in directory: " + directoryPath);
				DirectoryInfo dir = new DirectoryInfo(directoryPath);
				LookForProjectFile(dir, Config.Default);
				GenerateMessage("######## Checking for missing references to files ends ##############");
			}
			else
			{
				throw new DirectoryNotFoundException(string.Format("Directory {0} doesn't exists", directoryPath));
			}
		}

        private void LookForProjectMissingFiles(string[] projectFiles, string projectPath, DirectoryInfo dir, Config currentConfig)
        {
            string relativeDirPath = dir.FullName.Replace(projectPath + "\\", "");
            LookForConfigFileAndMaybeChangeConfiguration(dir, ref currentConfig);
            if (!currentConfig.ExcludedDirs.Any(x => relativeDirPath.StartsWith(x)))
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    string relativeFilePath = file.FullName.Replace(projectPath + "\\", "");

					if (!currentConfig.ExcludedExtensions.Any(x => x == file.Extension.TrimStart(new char[] { '.' })))
                    {
                        if (!projectFiles.Contains(relativeFilePath))
                        {
							GenerateMessage("Potentially missing file " + relativeFilePath);
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
            var projectFiles = dir.GetFiles().Where(f => f.Extension.In(".csproj", ".vbproj", ".fsproj", ".sqlproj"));
            
            LookForConfigFileAndMaybeChangeConfiguration(dir, ref currentConfig);

            foreach (var projectFile in projectFiles)
            {
                string projectPath = projectFile.DirectoryName;

				XDocument doc = XDocument.Load(projectFile.FullName);
				XNamespace vsNamespace = Constants.VisualStudioProjectFileXmlNamespace;

				var filesIncludedInCurrentProject = doc.Element(vsNamespace + "Project")
					.Descendants(vsNamespace + "ItemGroup")
					.SelectMany(x => x.Elements().Where(y => y.Name.LocalName.In(Constants.VisualStudioBuildActions)).Select(y => y.Attribute("Include").Value)).ToArray();

				GenerateMessage("----Project found : " + projectFile.Name);
                LookForProjectMissingFiles(filesIncludedInCurrentProject, projectPath, dir, currentConfig);
            }

            if (projectFiles == null || projectFiles.Count() == 0)
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
                Config newConfig = Config.Load(configFile.FullName);
				currentConfig.Merge(newConfig);
            }
        }

    }
}
