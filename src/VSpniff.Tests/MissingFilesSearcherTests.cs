using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using VSpniff.Core;
using VSpniff.Tests.Helpers;

namespace VSpniff.Tests
{
	[TestFixture]
	public class MissingFilesSearcherTests
	{
		private string SampleApplicationDirectory = @"..\..\..\VSpniff.Tests.SampleWebApplication";
		private List<string> missingFilesListed;

		[SetUp]
		public void SetUp()
		{
			missingFilesListed = new List<string>();
		}

		[Test]
		public void is_searcher_listed_all_mising_files()
		{
			string[] missingFilesInProject = { "About.cshtml", "jquery.ui.autocomplete.css", "jquery.validate.js" };

			MissingFilesSearcher searcher = new MissingFilesSearcher();
			searcher.NewMessage += searcher_NewMessage;
			searcher.Search(SampleApplicationDirectory);
			AssertHelper.AreEqualByContent(missingFilesInProject, missingFilesListed);
		}

		void searcher_NewMessage(object sender, string value)
		{
			if (value.Contains("Potentially missing file "))
			{
				string missingFileRelativePath = value.Replace("Potentially missing file ", "");
				FileInfo file = new FileInfo(missingFileRelativePath);
				missingFilesListed.Add(file.Name);
			}
		}
	}
}
