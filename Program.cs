using System;
using CodeGen.generators;

namespace CodeGen
{
	internal static class Program
	{
		public const string DefaultLang = "csharp";
		
		public static Package DefaultPkg = GeneratorConf.ExamplePkg;

		private static void Main(string[] args)
		{
			//TODO parse arguments using https://archive.codeplex.com/?p=commandline
			try
			{
				ExecuteConf.Execute();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}