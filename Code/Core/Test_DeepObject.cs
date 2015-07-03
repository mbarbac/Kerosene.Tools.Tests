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
	public class Test_DeepObject
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Create_With_Indexed_And_Dynamic_Syntaxes()
		{
			DeepObject obj = new DeepObject();
			obj["FirstName"] = "James";
			obj["LastName"] = "Bond";
			ConsoleEx.WriteLine("\n> {0}", obj);
			Assert.AreEqual(
				"{[{FirstName='James'}, {LastName='Bond'}]}"
				, obj.ToString());

			dynamic d = obj;
			d.Age = 50;
			d.Roles[0] = "Spy";
			d.Roles[1] = "Lover";
			d.Books["1965", 5] = "Thunderball";
			d.Gear.Gun = true;
			d.Gear.Knife = false;
			ConsoleEx.WriteLine("\n> {0}", obj);
			Assert.AreEqual(
				"{[" + "{FirstName='James'}, {LastName='Bond'}, {Age='50'}, " +
				"{Roles [{[0]='Spy'}, {[1]='Lover'}]}, " +
				"{Books [{[1965, 5]='Thunderball'}]}, " +
				"{Gear [{Gun='True'}, {Knife='False'}]}" + "]}"
				, obj.ToString());
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Null_Indexes()
		{
			var obj = new DeepObject();
			obj[null] = "Hello";
			Assert.AreEqual("{[{[]='Hello'}]}", obj.ToString());

			obj = new DeepObject();
			obj[null, null] = "Hello";
			Assert.AreEqual("{[{[, ]='Hello'}]}", obj.ToString());
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Removed_Not_Owned()
		{
			var obj = new DeepObject();
			dynamic d = obj;
			d.Name.First = "James";
			d.Name.Last = "Bond";
			ConsoleEx.WriteLine("\n> {0}", obj);

			var temp = new DeepObject();
			dynamic t = temp;
			t.Name.First = "James";

			bool r;

			var dyn = t.Name.DeepFind("First");
			r = obj.DeepRemove(dyn);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Dynamic_Adding()
		{
			var obj = new DeepObject();
			dynamic d = obj;

			d.DeepAdd("James"); Assert.AreEqual("{[{James}]}", obj.ToString());

			obj.DeepClear();
			d["Bond"] = "Spy"; Assert.AreEqual("{[{Bond='Spy'}]}", obj.ToString());
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Dispose()
		{
			var obj = new DeepObject();
			dynamic d = obj;
			d.Name.First = "James";
			d.Name.Last = "Bond";

			obj.Dispose();
			ConsoleEx.WriteLine("\n- Disposed: {0}", obj);
			Assert.IsTrue(obj.IsDisposed);
			Assert.AreEqual(0, obj.DeepCount());

			obj.Dispose();
			ConsoleEx.WriteLine("- Disposed Twice: {0}", obj);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Cloning()
		{
			Action<object> del = (source) =>
			{
				ConsoleEx.WriteLine("\n- Source: {0}", source.Sketch());

				var temp = ((ICloneable)source).Clone();
				ConsoleEx.WriteLine("- Cloned: {0}", temp.Sketch());

				Assert.IsTrue(source.IsEquivalentTo(temp),
					"Source '{0}' and cloned '{1}' are not equivalent."
					.FormatWith(source, temp));
			};

			var obj = new DeepObject();
			dynamic d = obj;
			d.Name.First = "James";
			d.Name.Last = "Bond";

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

			var obj = new DeepObject();
			dynamic d = obj;
			d.Name.First = "James";
			d.Name.Last = "Bond";

			del(obj, true);
			del(obj, false);
		}
	}
}
