using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using VSpniff.Core;

namespace VSpniff.Cmdlet
{
    [Cmdlet(VerbsCommon.Find,"MissingFiles")]
    public class FindMissingFilesCommand : PSCmdlet
    {

        [Alias("d")]
        [Parameter(Position = 0, Mandatory = false)]
        public string Directory { get; set; }

        protected override void ProcessRecord()
        {
            
            WriteObject("######## Checking for missing references to files started ##############");
            string directoryRelative;
            if (!string.IsNullOrEmpty(Directory))
            {
                directoryRelative = Directory;
            }
            else
            {
                directoryRelative = ".";
            }
            string dirPath = GetUnresolvedProviderPathFromPSPath(directoryRelative);
            WriteObject("Starting in directory: " + dirPath);
            MissingFilesSearcher searcher = new MissingFilesSearcher();
            searcher.MissingFileFound += new MissingFilesSearcher.StringEventHandler(searcher_MissingFileFound);
            searcher.ProjectFound += new MissingFilesSearcher.StringEventHandler(searcher_ProjectFound);
            searcher.Search(dirPath);
            WriteObject("######## Checking for missing references to files ends ##############");
        }

        void searcher_ProjectFound(object sender, string e)
        {
            WriteObject("----Project found : " + e);
        }

        void searcher_MissingFileFound(object sender, string e)
        {
            WriteObject("Potentially missing file " + e);
        }
    }
}
