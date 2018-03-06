using System;
using CodeGen.generators;
using CommandLine;

namespace CodeGen
{
	internal class Options
	{
		[Option('l', "lang", Default = Program.DefaultLang, HelpText = "Languege of output")]
		public string Lang { get; set; }

//		// Omitting long name, defaults to name of property, ie "--verbose"
//		[Option(Default = false, HelpText = "Prints all messages to standard output.")]
//		public bool Verbose { get; set; }

		[Option('f', "file", Default = "", HelpText = "File or url from which to read data")]
		public string File { get; set; }

		[Option('s', "spaces", Default = -1, HelpText = "Spaces offset (if not set or negative - using tabs)")]
		public int? Spaces { get; set; }

		public override string ToString()
		{
			return string.Format("{{ -l=\"{0}\" -f=\"{1}\" -s={2}}}", Lang, File, Spaces);
		}
	}


	internal static class Program
	{
		public const string DefaultLang = "cs";
		
		public static Package DefaultPkg = GeneratorConf.ExamplePkg;

		public static Options Opts;

		private static void Main(string[] args)
		{
			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(opts => Opts = opts)
				.WithNotParsed(Console.WriteLine);
			try
			{
				Console.Out.WriteLine("Opts = {0}", Opts);
				ExecuteConf.Execute();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
