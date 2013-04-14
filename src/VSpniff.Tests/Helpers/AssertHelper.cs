using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace VSpniff.Tests.Helpers
{
	public static class AssertHelper
	{
		public static void AreEqual(string[] table1, string[] table2)
		{
			table1.ToList().ForEach((elem) =>
			{
				Assert.AreEqual(true, table2.Where(x => x == elem).Count() == 1);
			});
			Assert.AreEqual(true, table1.Length == table2.Length);
		}
	}
}
