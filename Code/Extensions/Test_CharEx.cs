using Kerosene.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kerosene.Tools.Tests
{
	// ====================================================
	[TestClass]
	public class Test_CharEx
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Default_Comparisons()
		{
			char source, target;
			int res;

			source = 'e';
			target = 'é';
			res = source.CompareTo(target);
			Assert.IsTrue(res < 0);

			source = '\0';
			target = 'a';
			res = source.CompareTo(target);
			Assert.IsTrue(res != 0);

			source = 'a';
			target = '\0';
			res = source.CompareTo(target);
			Assert.IsTrue(res != 0);

			source = '\0';
			target = '\0';
			res = source.CompareTo(target);
			Assert.IsTrue(res == 0);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Comparisons_Ignore_Case()
		{
			char source, target;
			int res;

			source = '\0';
			target = 'a';
			res = source.CompareTo(target, StringComparison.CurrentCultureIgnoreCase);
			Assert.IsTrue(res != 0);

			source = 'a';
			target = '\0';
			res = source.CompareTo(target, StringComparison.CurrentCultureIgnoreCase);
			Assert.IsTrue(res != 0);

			source = '\0';
			target = '\0';
			res = source.CompareTo(target, StringComparison.CurrentCultureIgnoreCase);
			Assert.IsTrue(res == 0);

			source = 'a';
			target = 'a';
			res = source.CompareTo(target, StringComparison.CurrentCultureIgnoreCase);
			Assert.IsTrue(res == 0);

			source = 'a';
			target = 'A';
			res = source.CompareTo(target, StringComparison.CurrentCultureIgnoreCase);
			Assert.IsTrue(res == 0);
		}
	}
}
