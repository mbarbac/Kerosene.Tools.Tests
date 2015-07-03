using Kerosene.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Kerosene.Tools.Tests
{
	// ====================================================
	[TestClass]
	public class Test_SerializationEx
	{
		public void DoSerialization(object source, bool binary)
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
		}

		[Serializable]
		class Temporal : ISerializable, IEquivalent<Temporal>
		{
			public object Value { get; set; }

			public Temporal() { Value = null; }
			public bool EquivalentTo(Temporal target) { return Value.IsEquivalentTo(target.Value); }
			public void GetObjectData(SerializationInfo info, StreamingContext context) { info.AddExtended("Value", Value); }
			protected Temporal(SerializationInfo info, StreamingContext context) { Value = info.GetExtended("Value"); }
			public override string ToString() { return Value == null ? GetType().EasyName() : Value.Sketch(); }
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Ints()
		{
			var source = new Temporal() { Value = 0 };
			ConsoleEx.WriteLine("\n> Original... {0}", source.Sketch());

			DoSerialization(source, true);
			DoSerialization(source, false);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Strings()
		{
			var source = new Temporal() { Value = "James" };
			ConsoleEx.WriteLine("\n> Original... {0}", source.Sketch());

			DoSerialization(source, true);
			DoSerialization(source, false);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Arrays()
		{
			var source = new Temporal[]
			{
				new Temporal() {Value = "James"},
				new Temporal() {Value = "Maria"}
			};
			ConsoleEx.WriteLine("\n> Original... {0}", source.Sketch());

			DoSerialization(source, true);
			DoSerialization(source, false);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Lists()
		{
			var source = new Temporal() { Value = new List<string>() { "James", "Bond" } };
			ConsoleEx.WriteLine("\n> Original... {0}", source.Sketch());

			DoSerialization(source, true);
			DoSerialization(source, false); // We can use generic types even with a SOAP serializer :)!
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Dictionaries()
		{
			var source = new Temporal()
			{
				Value = new Dictionary<string, DateTime>() { { "James", DateTime.Now }, { "Maria", DateTime.Now } }
			};
			ConsoleEx.WriteLine("\n> Original... {0}", source.Sketch());

			DoSerialization(source, true);
			DoSerialization(source, false); // We can use generic types even with a SOAP serializer :)!
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Embedded_Standard_Types()
		{
			var source = new Temporal() { Value = typeof(string) };
			ConsoleEx.WriteLine("\n> Original... {0}", source.Sketch());

			DoSerialization(source, true);
			DoSerialization(source, false);

			source = new Temporal() { Value = typeof(List<string>) };
			ConsoleEx.WriteLine("\n> Original... {0}", source.Sketch());

			DoSerialization(source, true);
			DoSerialization(source, false);
		}

		//[OnlyThisTest]
		[TestMethod]
		public void Embedded_Generic_Types()
		{
			var source = new Temporal() { Value = typeof(List<>) };
			ConsoleEx.WriteLine("\n> Original... {0}", source.Sketch());

			DoSerialization(source, true);
			DoSerialization(source, false);
		}
	}
}
