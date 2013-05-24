using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace VSpniff.Tests.Helpers
{
	public static class AssertHelper
	{
		public static void AreEqualByContent(IEnumerable<string> collection1, IEnumerable<string> collection2)
		{
			foreach (var elem in collection1)
			{
				Assert.AreEqual(true, collection2.Where(x => x == elem).Count() == 1);
			}

			Assert.AreEqual(true, collection1.Count() == collection2.Count());
		}
	}
}
