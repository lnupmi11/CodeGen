using System;
using CodeGen.generators;

namespace CodeGen
{
	/// <summary>
	/// The configuration for execution, contains the application flow
	/// </summary>
	public static class ExecuteConf
	{
		/// <summary>
		/// The main flow of application
		/// </summary>
		public static void Execute()
		{
			var langName = Program.Opts != null ? Program.Opts.Lang : Program.DefaultLang;
			var lang = GeneratorConf.GetLanguage(langName);
			var gen = lang.Generator;

			if (gen == null) throw new NullReferenceException("This language has no generator");
			
			var data = gen.Generate(Program.DefaultPkg);
			var comment = lang.Comment;
			var extension = lang.Extension;
			foreach (var item in data)
			{
				var filename = item.Key + "." + extension;
				Console.WriteLine(comment, filename);
				Console.WriteLine(item.Value);
			}
		}
	}
}