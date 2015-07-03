using System;

namespace Kerosene.Tools.Tests
{
	// ====================================================
	/// <summary>
	/// Represents the current program.
	/// </summary>
	public class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		static void Main(string[] args)
		{
			DebugEx.IndentSize = 2;
			DebugEx.AutoFlush = true;
			DebugEx.AddConsoleListener();
			ConsoleEx.AskInteractive();

			TestsLauncher.Execute();
			ConsoleEx.ReadLine("\n=== Press [Enter] to finish... ");
		}
	}
}
