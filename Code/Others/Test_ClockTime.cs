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
	public class Test_CalendarDate
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Invalid_Constructors()
		{
			CalendarDate obj = null;

			try { obj = new CalendarDate(0, 0, 0); Assert.Fail(); }
			catch (ArgumentException) { }

			try { obj = new CalendarDate(1900, 0, 0); Assert.Fail(); }
			catch (ArgumentException) { }

			try { obj = new CalendarDate(1900, 1, 0); Assert.Fail(); }
			catch (ArgumentException) { }

			try { obj = new CalendarDate(1900, 2, 30); Assert.Fail(); }
			catch (ArgumentException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Equality()
		{
			var left = new DateTime(1900, 1, 1);
			var right = new DateTime(1900, 1, 1);
			Assert.AreEqual(left, right);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Comparison()
		{
			var left = new DateTime(1900, 1, 1);
			var right = new DateTime(1900, 1, 2);
			Assert.IsTrue(left < right);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Additions()
		{
			var obj = new CalendarDate(1900, 1, 31);
			var temp = obj.Add(1);
			Assert.AreEqual(new CalendarDate(1900, 2, 1), temp);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Parse()
		{
			ClockTime obj = null;

			obj = ClockTime.Parse("07");
			Assert.AreEqual(new ClockTime(7, 0, 0), obj);

			obj = ClockTime.Parse("0745");
			Assert.AreEqual(new ClockTime(7, 45, 0), obj);

			obj = ClockTime.Parse("074509");
			Assert.AreEqual(new ClockTime(7, 45, 9), obj);

			obj = ClockTime.Parse("074509005");
			Assert.AreEqual(new ClockTime(7, 45, 9, 5), obj);
		}
	}
}
