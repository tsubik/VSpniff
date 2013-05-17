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
	[Cmdlet(VerbsCommon.Find, "MissingFiles")]
	public class FindMissingFilesCommand : PSCmdlet
	{
		[Alias("d")]
		[Parameter(Position = 0, Mandatory = false)]
		public string Directory { get; set; }

		protected override void ProcessRecord()
		{
			MissingFilesSearcher searcher = new MissingFilesSearcher();
			searcher.NewMessage += new MissingFilesSearcher.StringEventHandler(searcher_NewMessage);
			searcher.Search(GetInitialDirectory());
		}

		private string GetInitialDirectory()
		{
			string directoryRelative;
			if (!string.IsNullOrEmpty(Directory))
			{
				directoryRelative = Directory;
			}
			else
			{
				directoryRelative = ".";
			}
			return GetUnresolvedProviderPathFromPSPath(directoryRelative);
		}

		void searcher_NewMessage(object sender, string message)
		{
			WriteObject(message);
		}
	}
}
