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
	public partial class Test_TypeEx
	{
		//OnlyThisTest]
		[TestMethod]
		public void EasyName_Bad_Scenarios()
		{
			Type type = null;

			type = null;
			try { type.EasyName(); Assert.Fail("Null reference not allowed."); }
			catch (NullReferenceException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void EasyName_SystemType()
		{
			Type type = null;
			string str = null;

			type = typeof(string);
			str = type.EasyName(); Assert.AreEqual("String", str);
			str = type.EasyName(chain: true); Assert.AreEqual("System.String", str);
		}

		public class Super<R, S>
		{
			public class Nested<T>
			{
			}
		}

		//[OnlyThisTest]
		[TestMethod]
		public void EasyName_Generic_Unbounded()
		{
			Type type = null;
			string str = null;

			type = typeof(Super<,>.Nested<>);
			str = type.EasyName(); Assert.AreEqual("Nested<>", str);
			str = type.EasyName(genericNames: true); Assert.AreEqual("Nested<T>", str);
			str = type.EasyName(chain: true); Assert.AreEqual("Kerosene.Tools.Tests.Test_TypeEx.Super<,>.Nested<>", str);
			str = type.EasyName(chain: true, genericNames: true); Assert.AreEqual("Kerosene.Tools.Tests.Test_TypeEx.Super<R, S>.Nested<T>", str);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void EasyName_Generic_Bounded()
		{
			Type type = null;
			string str = null;

			type = typeof(Super<string, int>.Nested<char>);
			str = type.EasyName(); Assert.AreEqual("Nested<Char>", str);
			str = type.EasyName(chain: true); Assert.AreEqual("Kerosene.Tools.Tests.Test_TypeEx.Super<System.String, System.Int32>.Nested<System.Char>", str);
		}
	}

	// ====================================================
	public partial class Test_TypeEx
	{
		//[OnlyThisTest]
		[TestMethod]
		public void IsNullable_SystemString()
		{
			var type = typeof(string);
			var result = type.IsNullableType();
			Assert.IsTrue(result);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void IsNullable_ValueType()
		{
			Type type;
			bool res;

			type = typeof(DateTime);
			res = type.IsNullableType();
			Assert.IsFalse(res);

			type = typeof(DateTime?);
			res = type.IsNullableType();
			Assert.IsTrue(res);
		}

		public interface IExample
		{
		}

		//[OnlyThisTest]
		[TestMethod]
		public void IsNullable_Interface_False()
		{
			var type = typeof(IExample);
			var res = type.IsNullableType();
			Assert.IsFalse(res);
		}
	}

	// ====================================================
	public partial class Test_TypeEx
	{
		//[OnlyThisTest]
		[TestMethod]
		public void IsAnoymous()
		{
			var obj = new { First = "James", Last = "Bond" };
			var type = obj.GetType();
			var result = type.IsAnonymousType();
			Assert.IsTrue(result);

			type = typeof(int);
			result = type.IsAnonymousType();
			Assert.IsFalse(result);
		}
	}

	// ====================================================
	public partial class Test_TypeEx
	{
		interface IUser<T> { T Key { get; } }
		interface IUser : IUser<string> { }
		public class User<T> : IUser<T> { public T Key { get; set; } }
		public class User : User<string> { }
		public class MyUser : User { }

		//[OnlyThisTest]
		[TestMethod]
		public void Implements_Chain()
		{
			bool r = false;

			r = typeof(IUser).Implements(typeof(IUser<>)); Assert.IsTrue(r);
			r = typeof(IUser).Implements(typeof(IUser<string>)); Assert.IsTrue(r);
			r = typeof(IUser).Implements(typeof(IUser<int>)); Assert.IsFalse(r);

			r = typeof(User<>).Implements(typeof(IUser<>)); Assert.IsTrue(r);

			r = typeof(User<string>).Implements(typeof(IUser<string>)); Assert.IsTrue(r);
			r = typeof(User<string>).Implements(typeof(IUser<int>)); Assert.IsFalse(r);

			r = typeof(User).Implements(typeof(IUser<>)); Assert.IsTrue(r);
			r = typeof(User).Implements(typeof(IUser<string>)); Assert.IsTrue(r);
			r = typeof(User).Implements(typeof(IUser<int>)); Assert.IsFalse(r);

			r = typeof(MyUser).Implements(typeof(User)); Assert.IsTrue(r);
			r = typeof(MyUser).Implements(typeof(IUser<>)); Assert.IsTrue(r);
			r = typeof(MyUser).Implements(typeof(IUser<string>)); Assert.IsTrue(r);
			r = typeof(MyUser).Implements(typeof(IUser<int>)); Assert.IsFalse(r);
		}
	}
}
