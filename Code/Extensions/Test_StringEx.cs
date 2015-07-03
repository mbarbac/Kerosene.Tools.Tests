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
	public partial class Test_StringEx
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Format_With_Null_Reference_Not_Allowed()
		{
			string str, res;
			try
			{
				str = (string)null;
				res = str.FormatWith("whatever"); Assert.Fail();
			}
			catch (NullReferenceException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Format_With_No_Arguments()
		{
			string str, res;

			str = "Example";
			res = str.FormatWith();
			Assert.AreEqual(str, res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Format_With_Few_Arguments_Fails()
		{
			string str, res;
			try
			{
				str = "Hello {1} {2}";
				res = str.FormatWith("James"); Assert.Fail();
			}
			catch (FormatException) { }
		}
	}

	// ====================================================
	public partial class Test_StringEx
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Null_If_Trimmed_Is_Empty()
		{
			string str, res;

			str = string.Empty;
			res = str.NullIfTrimmedIsEmpty();
			Assert.IsNull(res);

			str = "   ";
			res = str.NullIfTrimmedIsEmpty();
			Assert.IsNull(res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Empty_If_Trimmed_Is_Null()
		{
			var str = (string)null;
			var res = str.EmptyIfTrimmedIsNull();
			Assert.AreEqual(string.Empty, res);
		}
	}

	// ====================================================
	public partial class Test_StringEx
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Left_Bad_Scenarios()
		{
			string str, res;

			str = null;
			try { res = str.Left(1); Assert.Fail("Null reference shall not be allowed."); }
			catch (NullReferenceException) { }

			str = "whatever";
			try { res = str.Left(-1); Assert.Fail("Negative number of chars."); }
			catch (ArgumentException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Left_Good_Scenarios()
		{
			string str, res;

			str = "whatever";
			res = str.Left(0); Assert.AreEqual(string.Empty, res);
			res = str.Left(4); Assert.AreEqual("what", res);
			res = str.Left(999); Assert.AreEqual("whatever", res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Right_Bad_Scenarios()
		{
			string str, res;

			str = null;
			try { res = str.Right(1); Assert.Fail("Null reference shall not be allowed."); }
			catch (NullReferenceException) { }

			str = "whatever";
			try { res = str.Right(-1); Assert.Fail("Negative number of chars."); }
			catch (ArgumentException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Right_Good_Scenarios()
		{
			string str, res;

			str = "whatever";
			res = str.Right(0); Assert.AreEqual(string.Empty, res);
			res = str.Right(4); Assert.AreEqual("ever", res);
			res = str.Right(999); Assert.AreEqual("whatever", res);
		}
	}

	// ====================================================
	public partial class Test_StringEx
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Remove_Bad_Scenarios()
		{
			string source, target, res;

			source = null;
			target = "whatever";
			try { res = source.Remove(target); Assert.Fail(); }
			catch (NullReferenceException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Remove_With_Null_Target()
		{
			var source = "whatever";
			var target = (string)null;
			var res = source.Remove(target);
			Assert.AreEqual(source, res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Remove_Good_Scenarios()
		{
			string source, target, res;

			source = "whatever";
			target = "ever";
			res = source.Remove(target); Assert.AreEqual("what", res);

			source = "whatever";
			target = "EVER";
			res = source.Remove(target); Assert.AreEqual(source, res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Remove_Good_Scenarios_Ignore_Case()
		{
			string source, target, res;

			source = "whatever";
			target = "EVER";
			res = source.Remove(target, StringComparison.OrdinalIgnoreCase); Assert.AreEqual("what", res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void RemoveLast_Examples()
		{
			string source = "JamesBond007";
			string result = source.RemoveLast("Bond");
			Assert.AreEqual(result, "James007");

			source = "JamesBondJames007";
			result = source.RemoveLast("James");
			Assert.AreEqual(result, "JamesBond007");
		}
	}

	// ====================================================
	public partial class Test_StringEx
	{
		//[OnlyThisTest]
		[TestMethod]
		public void IndexOf_Char_Good_Scenarios()
		{
			string source;
			char target;
			int res;

			source = string.Empty;
			target = 'r';
			res = source.IndexOf(target, StringComparison.CurrentCulture); Assert.AreEqual(-1, res);

			source = "whatever";
			target = 'x';
			res = source.IndexOf(target, StringComparison.CurrentCulture); Assert.AreEqual(-1, res);

			source = "whatever";
			target = 't';
			res = source.IndexOf(target, StringComparison.CurrentCulture); Assert.AreEqual(3, res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void IndexOf_Char_Good_Scenarios_Ignore_Case()
		{
			string source;
			char target;
			int res;

			source = "whatever";
			target = 'X';
			res = source.IndexOf(target, StringComparison.CurrentCultureIgnoreCase); Assert.AreEqual(-1, res);

			source = "whatever";
			target = 'T';
			res = source.IndexOf(target, StringComparison.CurrentCultureIgnoreCase); Assert.AreEqual(3, res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void IndexOf_Any_Bad_Scenarios()
		{
			string source;
			char[] anyOf;
			int res;

			source = null;
			anyOf = "x".ToCharArray();
			try { res = source.IndexOfAny(anyOf, StringComparison.CurrentCulture); }
			catch (NullReferenceException) { }

			source = "whatever";
			anyOf = null;
			try { res = source.IndexOfAny(anyOf, StringComparison.CurrentCulture); }
			catch (ArgumentNullException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void IndexOf_Any_Good_Scenarios()
		{
			string source;
			char[] anyOf;
			int res;

			source = "whatever";
			anyOf = "x".ToCharArray();
			res = source.IndexOfAny(anyOf, StringComparison.CurrentCulture); Assert.AreEqual(-1, res);

			source = "whatever";
			anyOf = "xyzt".ToCharArray();
			res = source.IndexOfAny(anyOf, StringComparison.CurrentCulture); Assert.AreEqual(3, res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void IndexOf_Any_Good_Scenarios_Ignore_Case()
		{
			string source;
			char[] anyOf;
			int res;

			source = "whatever";
			anyOf = "x".ToCharArray();
			res = source.IndexOfAny(anyOf, StringComparison.CurrentCultureIgnoreCase); Assert.AreEqual(-1, res);

			source = "whatever";
			anyOf = "xyzT".ToCharArray();
			res = source.IndexOfAny(anyOf, StringComparison.CurrentCultureIgnoreCase); Assert.AreEqual(3, res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void IndexOf_NotValid_Bad_Scenarios()
		{
			string source;
			char[] valids;
			int res;

			source = null;
			valids = "whatever".ToCharArray();
			try { res = source.IndexOfNotValid(valids); Assert.Fail(); }
			catch (NullReferenceException) { }

			source = "whatever";
			valids = null;
			try { res = source.IndexOfNotValid(valids); Assert.Fail(); }
			catch (ArgumentNullException) { }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void IndexOf_NotValid_Good_Scenarios()
		{
			string source;
			char[] valids;
			int res;

			source = string.Empty;
			valids = "whatever".ToCharArray();
			res = source.IndexOfNotValid(valids);
			Assert.AreEqual(-1, res);

			source = "whatever";
			valids = "WHAT".ToCharArray();
			res = source.IndexOfNotValid(valids, StringComparison.CurrentCultureIgnoreCase);
			Assert.AreEqual(4, res);
		}
	}

	// ====================================================
	public partial class Test_StringEx
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Validated_String()
		{
			string str, tmp;

			str = null;
			tmp = str.Validated(canbeNull: true); Assert.IsNull(tmp);
			try { tmp = str.Validated(); Assert.Fail(); }
			catch (ArgumentNullException) { }

			str = "   ";
			tmp = str.Validated(emptyAsNull: true); Assert.IsNull(tmp);
			tmp = str.Validated(canbeEmpty: true); Assert.AreEqual(string.Empty, tmp);
			try { str.Validated(); Assert.Fail(); }
			catch (EmptyException) { }

			str = "Hi";
			try { tmp = str.Validated(minLen: 3); Assert.Fail(); }
			catch (ArgumentException) { }

			str = "James";
			try { tmp = str.Validated(maxLen: 3); Assert.Fail(); }
			catch (ArgumentException) { }

			var valids = "James".ToCharArray();
			str = "James Bond";
			int i = str.IndexOfNotValid(valids); Assert.AreEqual(5, i);

			var invalids = "xyzb".ToCharArray();
			i = str.IndexOfAny(invalids, StringComparison.CurrentCultureIgnoreCase);
			Assert.AreEqual(6, i);
		}
	}
}
