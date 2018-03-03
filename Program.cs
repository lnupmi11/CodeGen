using System;
using CodeGen.generators;

namespace CodeGen
{
	class Program
	{
		public static string DefaultLang = "csharp";
		public static Package DefaultPkg = DefaultGenerator.ExamplePkg;

		static void Main(string[] args)
		{
			Console.WriteLine("Hello LHalam!");
			Console.WriteLine(new GoGenerator().Generate(DefaultPkg));
			ExecuteConf.Execute();
		}
	}
}