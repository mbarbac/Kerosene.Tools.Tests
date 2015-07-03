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
	public class Test_EasyVersion
	{
		//[OnlyThisTest]
		[TestMethod]
		public void Empty_Results()
		{
			EasyVersion obj; string str; bool res;

			str = null;
			obj = new EasyVersion(str);
			res = obj == EasyVersion.Empty; Assert.IsTrue(res);

			str = string.Empty;
			obj = new EasyVersion(str);
			res = obj == EasyVersion.Empty; Assert.IsTrue(res);

			str = "..   ..";
			obj = new EasyVersion(str);
			res = obj == EasyVersion.Empty; Assert.IsTrue(res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Empty_And_Tailing_Parts()
		{
			EasyVersion obj; string str; bool res;

			str = "1..   .4... .. .";
			obj = new EasyVersion(str);
			res = "1...4" == obj.ToString(); Assert.IsTrue(res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Extracting_Spaces_And_Leading_Ceroes()
		{
			EasyVersion obj; string str; bool res;

			str = " 1 . 000 . 3 ";
			obj = new EasyVersion(str);
			res = "1.0.3" == obj.ToString(); Assert.IsTrue(res);

			str = " 1 . 00 0 . 000 3 00";
			obj = new EasyVersion(str);
			res = "1.0.300" == obj.ToString(); Assert.IsTrue(res);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Comparisons()
		{
			EasyVersion left, right; int res;

			left = null;
			right = null;
			res = EasyVersion.Compare(left, right); Assert.AreEqual(0, res);

			left = null;
			right = EasyVersion.Empty;
			res = EasyVersion.Compare(left, right); Assert.AreEqual(-1, res);

			left = EasyVersion.Empty;
			right = null;
			res = EasyVersion.Compare(left, right); Assert.AreEqual(+1, res);

			left = new EasyVersion("b1.02.45");
			right = new EasyVersion("b1.02.45");
			Assert.IsTrue(left == right);

			left = new EasyVersion("b2");
			right = new EasyVersion("b1");
			Assert.IsTrue(left > right);

			left = new EasyVersion("b1");
			right = new EasyVersion("b2");
			Assert.IsTrue(left < right);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Serialization()
		{
			Action<object, bool> del = (source, binary) =>
			{
				var mode = binary ? "binary" : "text/soap";
				ConsoleEx.WriteLine("\n- Source ({0}): {1}", mode, source.Sketch());

				var path = "c:\\temp\\data" + (binary ? ".bin" : ".xml");
				path.PathSerialize(source, binary);

				var temp = path.PathDeserialize(binary);
				ConsoleEx.WriteLine("- Created ({0}): {1}", mode, temp.Sketch());

				var result = source.IsEquivalentTo(temp);
				Assert.IsTrue(result,
					"With mode '{0}' source '{1}' and deserialized '{2}' are not equivalent."
					.FormatWith(mode, source.Sketch(), temp.Sketch()));
			};

			var obj = new EasyVersion("1.2new.3");
			del(obj, true);
			del(obj, false);
		}
	}
}
