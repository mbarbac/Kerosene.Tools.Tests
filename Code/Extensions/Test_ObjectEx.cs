using Kerosene.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Kerosene.Tools.Tests
{
	// ====================================================
	[TestClass]
	public partial class Test_ObjectEx
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Skecth_EasyCases()
		{
			object obj = null;
			string str = obj.Sketch(); Assert.AreEqual(string.Empty, str);

			obj = "whatever";
			str = obj.Sketch(); Assert.AreEqual("whatever", str);

			obj = new char[] { };
			str = obj.Sketch(); Assert.AreEqual(string.Empty, str);

			obj = "other".ToCharArray();
			str = obj.Sketch(); Assert.AreEqual("other", str);

			obj = SketchOptions.Default; // Enumeration
			str = obj.Sketch(); Assert.AreEqual("Default", str);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Sketch_Dictionary()
		{
			var obj = new Dictionary<string, int>() { { "James", 50 }, { "Maria", 25 } };
			var str = obj.Sketch();
			Assert.AreEqual("[James = 50, Maria = 25]", str);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Sketch_List()
		{
			var obj = new List<string>() { "James", "Maria" };
			var str = obj.Sketch();
			Assert.AreEqual("[James, Maria]", str);

			str = obj.Sketch(SketchOptions.RoundedBrackets);
			Assert.AreEqual("(James, Maria)", str);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Sketch_List_WithTypeName()
		{
			var obj = new List<string>() { "James", "Maria" };
			var str = obj.Sketch(SketchOptions.WithTypeName);
			Assert.AreEqual("List<String>([James, Maria])", str);

			str = obj.Sketch(SketchOptions.RoundedBrackets);
			Assert.AreEqual("(James, Maria)", str);
		}

		class Parent
		{
			public override string ToString() { return "ParentString"; }
		}

		class Child : Parent { }

		//[OnlyThisTest]
		[TestMethod]
		public void Sketch_ToString_Inherited()
		{
			var obj = new Child();
			var str = obj.Sketch(); Assert.AreEqual("ParentString", str);
		}

		class Child2 : Parent
		{
			public override string ToString() { return "Child2String"; }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Sketch_ToString_At_Instance_Level()
		{
			var obj = new Child2();
			var str = obj.Sketch(); Assert.AreEqual("Child2String", str);
		}

#pragma warning disable 414
		class WithPrivateMembers { string Name = "James"; int Age = 50; }
#pragma warning restore 414

		//[OnlyThisTest]
		[TestMethod]
		public void Sketch_Private_Members()
		{
			var obj = new WithPrivateMembers();
			var str = obj.Sketch();
			Assert.AreEqual("WithPrivateMembers", str);

			str = obj.Sketch(SketchOptions.IncludePrivateMembers | SketchOptions.IncludeFields);
			Assert.AreEqual("{Name = James, Age = 50}", str);
		}

#pragma warning disable 414
		class WithPublicAndPrivateMembers { public string Name { get; set; } int Age = 50; }
#pragma warning restore 414

		//[OnlyThisTest]
		[TestMethod]
		public void Sketch_Public_And_Private_Members()
		{
			var obj = new WithPublicAndPrivateMembers() { Name = "James" };
			var str = obj.Sketch();
			Assert.AreEqual("{Name = James}", str);

			str = obj.Sketch(SketchOptions.IncludePrivateMembers | SketchOptions.IncludeFields);
			Assert.AreEqual("{Name = James, Age = 50}", str);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Sketch_Public_And_Private_Members_WithTypeName()
		{
			var obj = new WithPublicAndPrivateMembers() { Name = "James" };
			var str = obj.Sketch(SketchOptions.WithTypeName);
			Assert.AreEqual("WithPublicAndPrivateMembers({Name = James})", str);

			str = obj.Sketch(SketchOptions.IncludePrivateMembers | SketchOptions.IncludeFields);
			Assert.AreEqual("{Name = James, Age = 50}", str);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Sketch_Dynamic()
		{
			var obj = new ExpandoObject();
			dynamic d = obj;
			d.Name = "James";
			d.Age = 50;

			var str = obj.Sketch();
			Assert.AreEqual("[[Name, James], [Age, 50]]", str);

			str = obj.Sketch(SketchOptions.RoundedBrackets);
			Assert.AreEqual("([Name, James], [Age, 50])", str);
		}
	}

	// ====================================================
	public partial class Test_ObjectEx
	{
		//[OnlyThisTest]
		[TestMethod]
		public void TryClone_ValueType()
		{
			int obj = 7;
			var result = obj.TryClone();
			Assert.AreEqual(7, result);
		}

		public void TryClone_String()
		{
			string str = "Hello";
			string tmp = str.TryClone();
			Assert.IsTrue(object.ReferenceEquals(str, tmp));
		}

		//[OnlyThisTest]
		[TestMethod]
		public void TryClone_Null_Reference()
		{
			object obj = null;
			object tmp = obj.TryClone();
			Assert.AreEqual(null, tmp);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void TryClone_AutoCasting()
		{
			var obj = new WithPublicAndPrivateMembers() { Name = "James Bond" };
			var tmp = obj.TryClone();
			Assert.IsTrue(object.ReferenceEquals(obj, tmp));
		}

		class Another : ICloneable
		{
			public string Name { get; set; }
			public Another Clone() { return new Another() { Name = this.Name }; }
			object ICloneable.Clone() { return this.Clone(); }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void TryClone_Extended()
		{
			var obj = new Another() { Name = "Bond" };
			var clone = obj.TryClone();
			Assert.IsFalse(object.ReferenceEquals(obj, clone));
			Assert.AreEqual("Bond", clone.Name);
		}
	}

	// ====================================================
	public partial class Test_ObjectEx
	{
		//[OnlyThisTest]
		[TestMethod]
		public void ConvertTo_SameType()
		{
			var child = new Child();
			var res = child.ConvertTo(typeof(Child));
			Assert.IsTrue(object.ReferenceEquals(child, res));
		}

		//[OnlyThisTest]
		[TestMethod]
		public void ConvertTo_FromParent_ToChild()
		{
			var child = new Child();
			var res = child.ConvertTo(typeof(Parent));
			Assert.IsTrue(object.ReferenceEquals(child, res));
		}

		class Alpha
		{
			public string Age { get; set; }
		}
		class Beta
		{
			public int Age { get; set; }
			public static implicit operator Alpha(Beta beta)
			{
				if (beta == null) return null;
				return new Alpha() { Age = beta.Age.ToString() };
			}
		}

		//[OnlyThisTest]
		[TestMethod]
		public void ConvertTo_With_Implicit_Operator()
		{
			Beta beta = new Beta() { Age = 7 };
			Alpha alpha = beta.ConvertTo<Alpha>();
			Assert.IsNotNull(alpha);
			Assert.AreEqual("7", alpha.Age);

			object obj = null;
			try
			{
				obj.ConvertTo<int>();
				Assert.Fail("Conversion of an int into a null should have failed.");
			}
			catch (ArgumentException) { }
		}
	}
}
