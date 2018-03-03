using System;
using CodeGen.generators;

namespace CodeGen
{
	public static class ExecuteConf
	{
		public static void Execute()
		{
			const string lang = Program.DefaultLang;
			
			var gen = GeneratorConf.GetGenerator(lang);
			var data = gen.Generate(Program.DefaultPkg);
			var comment = GeneratorConf.Languanges[lang].Comment;
			var extension = GeneratorConf.Languanges[lang].Extension;
			foreach (var item in data)
			{
				var filename = item.Key + "." + extension;
				Console.WriteLine(comment, filename);
				Console.WriteLine(item.Value);
			}
		}
	}
}