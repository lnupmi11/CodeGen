using System;
using CodeGen.generators;

namespace CodeGen
{
	internal static class Program
	{
		public const string DefaultLang = "java";
		public static Package DefaultPkg = GeneratorConf.ExamplePkg;

		private static void Main(string[] args)
		{
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