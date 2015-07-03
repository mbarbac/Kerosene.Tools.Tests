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
	public class Test_ClockTime
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Invalid_Constructors()
		{
			try { var obj = new ClockTime(-1, 0, 0); Assert.Fail(); }
			catch (ArgumentException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Equality()
		{
			var left = new ClockTime(0, 0, 0);
			var right = new ClockTime(0, 0, 0);
			Assert.AreEqual(left, right);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Comparison()
		{
			var left = new ClockTime(0, 0, 0);
			var right = new ClockTime(0, 0, 1);
			Assert.IsTrue(left < right);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Additions()
		{
			var obj = new ClockTime(0, 23, 59);
			var temp = obj.Add(24);
			Assert.AreEqual(new ClockTime(0, 23, 59), temp);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Parse()
		{
			CalendarDate obj = null;

			obj = CalendarDate.Parse("19990101");
			Assert.AreEqual(new CalendarDate(1999, 1, 1), obj);
		}
	}
}
