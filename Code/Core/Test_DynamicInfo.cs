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
	public class Test_DynamicInfo
	{
		//[OnlyThisTest]
		[TestMethod]
		public void ParseName_Empty_Names()
		{
			try { var str = DynamicInfo.ParseName(null); Assert.Fail(); }
			catch (ArgumentNullException) { }

			try { var str = DynamicInfo.ParseName(x => null); Assert.Fail(); }
			catch (EmptyException) { }

			try { var str = DynamicInfo.ParseName(x => " "); }
			catch (EmptyException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void ParseName_WithSpaces_IsAllowed_And_Trimmed()
		{
			var str = DynamicInfo.ParseName(x => "  Whatever With Spaces     ");
			Assert.AreEqual("Whatever With Spaces", str);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void ParseName_Cannot_Resolve_Into_Its_Argument()
		{
			try { var str = DynamicInfo.ParseName(x => "x"); Assert.Fail(); }
			catch (ArgumentException) { }

			try { var str = DynamicInfo.ParseName(x => x); Assert.Fail(); }
			catch (ArgumentException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void ParseName_With_Embedded_Methods()
		{
			try { var name = DynamicInfo.ParseName(x => !x.Whatever); Assert.Fail(); }
			catch (ArgumentException) { }

			try { var name = DynamicInfo.ParseName(x => x.Whatever().Pepe); Assert.Fail(); }
			catch (ArgumentException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void ParseName_Remove_Tag_Name()
		{
			var name = DynamicInfo.ParseName(x => "x.Alpha.Beta");
			Assert.AreEqual("Alpha.Beta", name);

			name = DynamicInfo.ParseName(x => x.Delta.Epsilon);
			Assert.AreEqual("Delta.Epsilon", name);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void DynamicSource_Read_And_Write()
		{
			DeepObject obj = new DeepObject();
			dynamic d = obj;
			d.Name.First = "James";
			d.Name.Last = "Bond";

			var name = DynamicInfo.Read(obj, x => x.Name.First);
			Assert.AreEqual(name, "James");

			DynamicInfo.Write(obj, x => x.Name.Last, "Smith");
			name = DynamicInfo.Read(obj, x => x.Name.Last);
			Assert.AreEqual(name, "Smith");
		}

		//[OnlyThisTest]
		[TestMethod]
		public void DynamicSource_Not_Existent_Member()
		{
			try
			{
				DeepObject obj = new DeepObject();
				dynamic d = obj;
				d.Name.First = "James";
				d.Name.Last = "Bond";

				var item = DynamicInfo.Read(obj, x => x.Name.Whatever);
				Assert.Fail();
			}
			catch (NotFoundException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void DynamicSource_Intermediate_Member_Is_Null_Shall_Fail()
		{
			try
			{
				DeepObject obj = new DeepObject();
				dynamic d = obj;
				d.Name.First = "James";
				d.Name.Last = "Bond";

				d.Name = null;
				var item = DynamicInfo.Read(obj, x => x.Name.First);
				Assert.Fail();
			}
			catch (EmptyException) { }
		}
	}
}
