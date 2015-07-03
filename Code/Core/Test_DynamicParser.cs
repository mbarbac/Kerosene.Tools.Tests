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
	public class Test_DynamicParser
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Only_With_Concrete_Arguments()
		{
			Func<string, int, object>
				func = (s, i) => string.Format("{0}-{1}", s, i);

			var parser = DynamicParser.Parse(func, "James", 50);
			ConsoleEx.WriteLine("- Parser: {0}", parser);
			ConsoleEx.WriteLine("- Result: {0}", parser.Result);
			Assert.AreEqual("James-50", parser.Result);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void With_Some_Concrete_Arguments()
		{
			Func<dynamic, int, object>
				func = (x, i) => (string)(x + i)[x.Alfa + x.Beta];

			var parser = DynamicParser.Parse(func, 7);
			ConsoleEx.WriteLine("- Parser: {0}", parser);
			ConsoleEx.WriteLine("- Result: {0}", parser.Result);

			Assert.AreEqual("(String (x Add 7)[(x.Alfa Add x.Beta)])", parser.Result.ToString());
		}

		//[OnlyThisTest]
		[TestMethod]
		public void With_Many_Dynamic_Arguments()
		{
			Func<dynamic, dynamic, int, object>
				func = (x, y, i) => x.Where(x.Alfa(), y.Beta)[y.Gamma] == x.Delta(i).Whatever();

			var parser = DynamicParser.Parse(func, 7);
			ConsoleEx.WriteLine("- Parser: {0}", parser);
			ConsoleEx.WriteLine("- Result: {0}", parser.Result);

			Assert.AreEqual(
				"(x.Where(x.Alfa(), y.Beta)[y.Gamma] Equal x.Delta(7).Whatever())"
				, parser.Result.ToString());
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Dispose_Twice()
		{
			Func<dynamic, int, object>
				func = (x, i) => (string)(x + i)[x.Alfa + x.Beta];

			var parser = DynamicParser.Parse(func, 7);
			parser.Dispose();
			Assert.IsTrue(parser.IsDisposed);
			Assert.IsNull(parser.Arguments);
			Assert.AreEqual("disposed::DynamicParser(() => )", parser.ToString());

			parser.Dispose();
			Assert.IsTrue(parser.IsDisposed);
			Assert.IsNull(parser.Arguments);
			Assert.AreEqual("disposed::DynamicParser(() => )", parser.ToString());
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Equivalences()
		{
			Func<dynamic, object> sourcef = x =>
				x.Phi = x.Alpha[x.Beta, 7].MyMethod(x.Delta && !x.Gamma);

			Func<dynamic, object> targetf = x =>
				x.Phi = x.Alpha[x.Beta, 7].MyMethod(x.Delta && !x.Gamma);

			var source = DynamicParser.Parse(sourcef).Result;
			var target = DynamicParser.Parse(targetf).Result;
			var res = source.IsEquivalentTo(target); Assert.IsTrue(res);

			Func<dynamic, object> otherf = x =>
				x.Phi = (x.Alpha[x.Beta, 7].MyMethod(x.Delta && !x.Gamma) + 1);

			var other = DynamicParser.Parse(otherf).Result;
			res = source.IsEquivalentTo(other); Assert.IsFalse(res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Cloning()
		{
			Action<object> del = (source) =>
			{
				ConsoleEx.WriteLine("\n- Source: {0}", source);

				var temp = ((ICloneable)source).Clone();
				ConsoleEx.WriteLine("- Cloned: {0}", temp);

				Assert.IsTrue(source.IsEquivalentTo(temp),
					"Source '{0}' and cloned '{1}' are not equivalent."
					.FormatWith(source, temp));
			};

			Func<dynamic, object> func = x => (string)x.Alpha[!x.Beta == (string)(x.Delta + 5)];
			var parser = DynamicParser.Parse(func);
			var obj = parser.Result;

			del(obj);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Serialization()
		{
			Action<object, bool> del = (source, binary) =>
			{
				var mode = binary ? "binary" : "text/soap";
				ConsoleEx.WriteLine("\n- Source ({0}): {1}", mode, source.Sketch());

				var path = "c:\\temp\\data"; path += binary ? ".bin" : ".xml";
				path.PathSerialize(source, binary);

				var temp = path.PathDeserialize(binary);
				ConsoleEx.WriteLine("- Created ({0}): {1}", mode, temp.Sketch());

				var result = source.IsEquivalentTo(temp);
				Assert.IsTrue(result,
					"With mode '{0}' source '{1}' and deserialized '{2}' are not equivalent."
					.FormatWith(mode, source.Sketch(), temp.Sketch()));
			};

			Func<dynamic, object> func = x => (string)x.Alpha[!x.Beta == (string)(x.Delta + 5)];
			var parser = DynamicParser.Parse(func);
			var obj = parser.Result;

			del(obj, true);
			del(obj, false);
		}
	}
}
