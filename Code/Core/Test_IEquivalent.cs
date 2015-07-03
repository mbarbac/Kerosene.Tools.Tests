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
	public class Test_IEquivalent
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Value_Types()
		{
			object source, target;
			bool res;

			source = 0;
			target = 0;
			res = source.IsEquivalentTo(target); Assert.IsTrue(res);

			source = 0;
			target = 1;
			res = source.IsEquivalentTo(target); Assert.IsFalse(res);

			source = new DateTime(2008, 1, 1);
			target = new DateTime(2008, 1, 1);
			res = source.IsEquivalentTo(target); Assert.IsTrue(res);

			source = new DateTime(2008, 1, 1);
			target = new DateTime(2008, 1, 2);
			res = source.IsEquivalentTo(target); Assert.IsFalse(res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Strings()
		{
			var source = "James";
			var target = "JamesBond";
			var result = source.IsEquivalentTo(target);
			Assert.IsFalse(result);

			source = "007";
			target = "007";
			result = source.IsEquivalentTo(target);
			Assert.IsTrue(result);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Arrays()
		{
			var source = "James".ToCharArray();
			var target = "James".ToCharArray();
			var res = source.IsEquivalentTo(target);
			Assert.IsTrue(res);

			var other = new DateTime[] { new DateTime(2001, 1, 1), new DateTime(2002, 2, 2) };
			var extra = new DateTime[] { new DateTime(2001, 1, 1), new DateTime(2002, 2, 2) };
			res = source.IsEquivalentTo(target);
			Assert.IsTrue(res);

			res = source.IsEquivalentTo(other); Assert.IsFalse(res);
			res = other.IsEquivalentTo(source); Assert.IsFalse(res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Dictionaries_Same_Type()
		{
			var source = new Dictionary<string, int>() { { "James", 50 }, { "Maria", 25 } };
			var target = new Dictionary<string, int>() { { "James", 50 }, { "Maria", 25 } };
			var result = source.IsEquivalentTo(target);
			Assert.IsTrue(result);

			source = new Dictionary<string, int>() { { "James", 50 }, { "Maria", 25 } };
			target = new Dictionary<string, int>() { { "James", 50 }, { "Maria", 12 } };
			result = source.IsEquivalentTo(target);
			Assert.IsFalse(result);

			source = new Dictionary<string, int>() { { "James", 50 }, { "Maria", 25 } };
			target = new Dictionary<string, int>() { { "James", 50 } };
			result = source.IsEquivalentTo(target);
			Assert.IsFalse(result);

			source = new Dictionary<string, int>() { { "James", 50 } };
			target = new Dictionary<string, int>() { { "James", 50 }, { "Maria", 25 } };
			result = source.IsEquivalentTo(target);
			Assert.IsFalse(result);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Dictionaries_Different_Types()
		{
			var source = new Dictionary<string, int>() { { "James", 50 }, { "Maria", 25 } };
			var target = new Dictionary<string, DateTime>() { { "James", DateTime.Now }, { "Maria", DateTime.Now } };
			var result = source.IsEquivalentTo(target);
			Assert.IsFalse(result);

			var other = new Dictionary<DateTime, string>() { { DateTime.Now, "James" }, { DateTime.UtcNow, "Maria" } };
			result = source.IsEquivalentTo(other);
			Assert.IsFalse(result);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Enumerables()
		{
			var source = new List<string>() { "James", "Maria" };
			var target = new List<object>() { "James", "Maria" };
			var result = source.IsEquivalentTo(target);
			Assert.IsTrue(result);

			source = new List<string>() { "James", "Maria" };
			target = new List<object>() { "James" };
			result = source.IsEquivalentTo(target);
			Assert.IsFalse(result);

			source = new List<string>() { "James" };
			target = new List<object>() { "James", "Maria" };
			result = source.IsEquivalentTo(target);
			Assert.IsFalse(result);

			var other = new List<object>() { "James", DateTime.Now };
			result = source.IsEquivalentTo(other);
			Assert.IsFalse(result);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Enumerations()
		{
			var source = SketchOptions.IncludePrivateMembers;
			var target = SketchOptions.IncludePrivateMembers;
			var result = source.IsEquivalentTo(target);
			Assert.IsTrue(result);

			var other = SketchOptions.IncludeStaticMembers;
			result = source.IsEquivalentTo(other);
			Assert.IsFalse(result);
		}

		public class Parent : IEquivalent<Parent>
		{
			public string Name { get; set; }
			public bool EquivalentTo(Parent target)
			{
				return target == null ? false : this.Name == target.Name;
			}
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Standard_Class()
		{
			var source = new Parent() { Name = "James" };
			var target = new Parent() { Name = "James" };
			var res = source.IsEquivalentTo(target); Assert.IsTrue(res);

			target = new Parent() { Name = "Bond" };
			res = source.IsEquivalentTo(target); Assert.IsFalse(res);
		}

		public class Child : Parent, IEquivalent<Child>
		{
			public int Age { get; set; }
			public new bool EquivalentTo(Parent target)
			{
				if (!base.EquivalentTo(target)) return false;

				// Its up to the child define how to compare with a parent...
				// Returning false for testing purposes...
				return false;
			}
			public bool EquivalentTo(Child target)
			{
				if (!base.EquivalentTo(target)) return false;
				if (this.Age != target.Age) return false;
				return true;
			}
		}

		//[OnlyThisTest]
		[TestMethod]
		public void From_Parent_To_Child()
		{
			object source = new Parent() { Name = "James" };
			object target = new Child() { Name = "James", Age = 50 };
			bool res = source.IsEquivalentTo(target); Assert.IsTrue(res);

			target = new Child() { Name = "Bond", Age = 50 };
			res = source.IsEquivalentTo(target); Assert.IsFalse(res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void From_Child_To_Parent()
		{
			object source = new Child() { Name = "James", Age = 50 };
			object target = new Parent() { Name = "James" };
			bool res = source.IsEquivalentTo(target); Assert.IsFalse(res);
		}

		public interface IFaceExample : IEquivalent<IFaceExample>
		{
			string Name { get; }
		}

		public class FaceParent : IFaceExample
		{
			public string Name { get; set; }
			public bool EquivalentTo(IFaceExample target)
			{
				return this.Name == target.Name;
			}
		}
		public class FaceChild : IFaceExample
		{
			public string Name { get; set; }
			public bool EquivalentTo(IFaceExample target)
			{
				return this.Name == target.Name;
			}
		}

		//[OnlyThisTest]
		[TestMethod]
		public void On_Interfaces()
		{
			object source = (IFaceExample)null;
			object target = (IFaceExample)null;
			bool res = source.IsEquivalentTo(target); Assert.IsTrue(res);

			source = (IFaceExample)null;
			target = new FaceParent() { Name = "Bond" };
			res = source.IsEquivalentTo(target); Assert.IsFalse(res);

			source = new FaceParent() { Name = "James" };
			target = new FaceChild() { Name = "James" };
			res = source.IsEquivalentTo(target); Assert.IsTrue(res);
			res = target.IsEquivalentTo(source); Assert.IsTrue(res);
		}
	}
}
