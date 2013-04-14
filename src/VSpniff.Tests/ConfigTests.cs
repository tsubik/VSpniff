using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using VSpniff.Core;

namespace VSpniff.Test
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
			_config.ExcludedDirs.ToList().ForEach((elem) =>
			{
				Assert.AreEqual(true, ResultExcludedDirs.Where(x => x == elem).Count() == 1);
			});
			_config.ExcludedExtensions.ToList().ForEach((elem) =>
			{
				Assert.AreEqual(true, ResultExcludedExtensions.Where(x => x == elem).Count() == 1);
			});
		}

		[Test]
		public void is_overriding_configuration_in_the_override_mode()
		{
			_config2.Mode = ConfigFileMode.Override;
			string[] ResultExcludedDirs = new string[] { "bin", "trash", "home" };
			string[] ResultExcludedExtensions = new string[] { "exe", "bin", "zip" };

			_config.Merge(_config2);
			_config.ExcludedDirs.ToList().ForEach((elem) =>
			{
				Assert.AreEqual(true, ResultExcludedDirs.Where(x => x == elem).Count() == 1);
			});
			_config.ExcludedExtensions.ToList().ForEach((elem) =>
			{
				Assert.AreEqual(true, ResultExcludedExtensions.Where(x => x == elem).Count() == 1);
			});
		}
	}
}
