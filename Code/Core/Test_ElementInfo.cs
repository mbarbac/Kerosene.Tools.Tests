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
	public partial class Test_ElementInfo
	{
		class ClassA
		{
			string Name { get; set; }
			public bool IsMaster { get; set; }
			public string Division { get; set; }
			public ClassA(string name = null) { Name = name; IsMaster = false; }
		}

		class ClassB
		{
			public ClassA Plain { get; private set; }
			public ClassB(string name = null) { Plain = new ClassA(name); }
			public static ClassB CreateWithoutPlain() { return new ClassB() { Plain = null }; }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void ParseName_Multipart()
		{
			var name = ElementInfo.ParseName<ClassB>(x => x.Plain.IsMaster);
			ConsoleEx.WriteLine("\n- {0}", name);
			Assert.AreEqual("Plain.IsMaster", name);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void ParseName_Invalid_UnaryOperation()
		{
			try { var name = ElementInfo.ParseName<ClassB>(x => !x.Plain.IsMaster); Assert.Fail(); }
			catch (ArgumentException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void ParseName_Invalid_BinaryOperation()
		{
			try { var name = ElementInfo.ParseName<ClassB>(x => x.Plain.Division + "Other"); Assert.Fail(); }
			catch (ArgumentException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void ParseName_InvalidMethodInvocation()
		{
			try { var name = ElementInfo.ParseName<ClassB>(x => x.GetType()); Assert.Fail(); }
			catch (ArgumentException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void ParseName_Lambda_CannotResolveIntoItself()
		{
			try { var name = ElementInfo.ParseName<ClassB>(x => x); Assert.Fail(); }
			catch (ArgumentException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void ParseName_LambdaToString_CannotResolveIntoItself()
		{
			try { var name = ElementInfo.ParseName<ClassB>(x => "x"); Assert.Fail(); }
			catch (ArgumentException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Non_Existent_Element()
		{
			try { var info = ElementInfo.Create<ClassB>("Plain.Whatever"); Assert.Fail(); }
			catch (NotFoundException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Invalid_Parent()
		{
			try
			{
				var obj = ClassB.CreateWithoutPlain();
				var info = ElementInfo.Create<ClassB>("Plain.Name");
				var item = info.GetValue(obj);
				Assert.Fail();
			}
			catch (EmptyException) { }
		}
	}

	// ====================================================
	public partial class Test_ElementInfo
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Values_WithMultipartPublicMembers()
		{
			var obj = new ClassB("James");
			obj.Plain.Division = "Corporate";

			var info = ElementInfo.Create<ClassB>(x => x.Plain.Division);
			var item = info.GetValue(obj);
			Assert.AreEqual("Corporate", item);

			info.SetValue(obj, "Sales");
			item = info.GetValue(obj);
			Assert.AreEqual("Sales", item);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Values_WithMultipartPrivateMembers()
		{
			var obj = new ClassB("James");

			var info = ElementInfo.Create<ClassB>("Plain.Name");
			var item = info.GetValue(obj);
			Assert.AreEqual("James", item);

			info.SetValue(obj, "Maria");
			item = info.GetValue(obj);
			Assert.AreEqual("Maria", item);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Dispose_DoesNot_DisposeParentByDefault()
		{
			var info = ElementInfo.Create<ClassB>("Plain.Name");
			var parent = info.Parent;
			Assert.IsNotNull(parent);

			info.Dispose();
			Assert.IsTrue(info.IsDisposed);
			Assert.IsFalse(parent.IsDisposed);
		}
	}

	// ====================================================
	public partial class Test_ElementInfo
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Static_Values_WithMultipartPublicMembers()
		{
			var obj = new ClassB("James");
			obj.Plain.Division = "Corporate";

			var item = ElementInfo.GetElementValue(obj, x => x.Plain.Division);
			Assert.AreEqual("Corporate", item);

			ElementInfo.SetElementValue(obj, x => x.Plain.Division, "Sales");
			Assert.AreEqual("Sales", obj.Plain.Division);
		}
	}

	// ====================================================
	public partial class Test_ElementInfo
	{
		interface IUser<T> { T Key { get; } }
		interface IUser : IUser<string> { }
		public class User<T> : IUser<T> { public T Key { get; set; } }
		public class User : User<string> { }
		public class MyUser : User { }

		//[OnlyThisTest]
		[TestMethod]
		public void GetElements()
		{
			var type = typeof(MyUser);
			var flags = TypeEx.InstancePublicAndHidden;
			var items = ElementInfo.GetElements(type, flags);
			bool found = items.Find(x => x.Name == "Key") != null;
			Assert.IsTrue(found);

			type = typeof(User);
			flags = TypeEx.FlattenInstancePublicAndHidden;
			items = ElementInfo.GetElements(type, flags);
			found = items.Find(x => x.Name == "Key") != null;
			Assert.IsTrue(found);

			type = typeof(IUser);
			flags = TypeEx.FlattenInstancePublicAndHidden;
			items = ElementInfo.GetElements(type, flags);
			found = items.Find(x => x.Name == "Key") != null;
			Assert.IsTrue(found);

			type = typeof(IUser<>);
			flags = TypeEx.FlattenInstancePublicAndHidden;
			items = ElementInfo.GetElements(type, flags);
			found = items.Find(x => x.Name == "Key") != null;
			Assert.IsTrue(found);

			type = typeof(IUser<int>);
			flags = TypeEx.FlattenInstancePublicAndHidden;
			items = ElementInfo.GetElements(type, flags);
			found = items.Find(x => x.Name == "Key") != null;
			Assert.IsTrue(found);

			// Even if a member is found (with type 'int') we want a type 'string'...
			type = typeof(IUser<int>);
			flags = TypeEx.FlattenInstancePublicAndHidden;
			items = ElementInfo.GetElements(type, flags);
			found = items.Find(x => x.Name == "Key" && x.ElementType == typeof(string)) != null;
			Assert.IsFalse(found);
		}
	}
}
