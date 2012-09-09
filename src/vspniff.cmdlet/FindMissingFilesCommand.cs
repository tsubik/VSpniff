using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using CheckVSMissingFiles.Core;

namespace CheckVSMissingFilesCmdLet
{
    [Cmdlet(VerbsCommon.Find,"MissingFiles")]
    public class FindMissingFilesCommand : PSCmdlet
    {

        [Alias("d")]
        [Parameter(Position = 0, Mandatory = true)]
        public string Directory { get; set; }

        protected override void ProcessRecord()
        {
            
            WriteObject("######## Checking for missing references to files started ##############");
            string dirPath = GetUnresolvedProviderPathFromPSPath(Directory);
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
