using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using VSpniff.Core;
using VSpniff.Tests.Helpers;

namespace VSpniff.Tests
{
	[TestFixture]
	public class ConfigTests
	{
		Config _config;
		Config _config2;
		
		[SetUp]
		public void SetUp()
		{
			_config = new Config
			{
				Mode = ConfigFileMode.Append,
				ExcludedDirs = "bin,obj".Split(','),
				ExcludedExtensions = "bat,exe,com".Split(',')
			};

			_config2 = new Config
			{
				Mode = ConfigFileMode.Append,
				ExcludedDirs = new string[] { "bin", "trash", "home" },
				ExcludedExtensions = new string[] { "exe", "bin", "zip" }
			};
		}

		[Test]
		public void is_appending_configuration_in_the_append_mode()
		{
			string[] ResultExcludedDirs = new string[] { "bin", "obj", "trash", "home" };
			string[] ResultExcludedExtensions = new string[] { "bat", "exe", "com", "bin", "zip" };

			_config.Merge(_config2);
			AssertHelper.AreEqualByContent(_config.ExcludedDirs, ResultExcludedDirs);
			AssertHelper.AreEqualByContent(_config.ExcludedExtensions, ResultExcludedExtensions);
		}

		[Test]
		public void is_overriding_configuration_in_the_override_mode()
		{
			_config2.Mode = ConfigFileMode.Override;
			string[] ResultExcludedDirs = new string[] { "bin", "trash", "home" };
			string[] ResultExcludedExtensions = new string[] { "exe", "bin", "zip" };

			_config.Merge(_config2);
			AssertHelper.AreEqualByContent(_config.ExcludedDirs, ResultExcludedDirs);
			AssertHelper.AreEqualByContent(_config.ExcludedExtensions, ResultExcludedExtensions);
		}
	}
}
