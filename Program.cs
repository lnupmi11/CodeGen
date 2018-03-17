using System;
using CodeGen.generators;
using CommandLine;

namespace CodeGen
{
	internal class Options
	{
		public Options(string lang, string file, int? spaces, bool stdout)
		{
			Lang = lang;
			File = file;
			Spaces = spaces;
			Stdout = stdout;
		}

		[Option('l', "lang", Default = Program.DefaultLang, HelpText = "Languege of output")]
		public string Lang { get; }

//		// Omitting long name, defaults to name of property, ie "--verbose"
//		[Option(Default = false, HelpText = "Prints all messages to standard output.")]
//		public bool Verbose { get; set; }

		[Option('f', "file", Default = "", HelpText = "File or url from which to read data")]
		public string File { get; }

		[Option('s', "spaces", Default = -1, HelpText = "Spaces offset (if not set or negative - using tabs)")]
		public int? Spaces { get; }

		// Set Default to false in production
		[Option('o', "stdout", Default = true, HelpText = "")]
		public bool Stdout { get; }

		public override string ToString()
		{
			return $"{{ -l=\"{Lang}\" -f=\"{File}\" -s={Spaces} {(Stdout ? "-o" : "")}}}";
		}
	}

	internal static class Program
	{
		public const string DefaultLang = "python";

		public static readonly Package DefaultPkg = GeneratorConf.ExamplePkg;

		public static Options Opts;

		private static void Main(string[] args)
		{
			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(opts => Opts = opts)
				.WithNotParsed(Console.WriteLine);
			try
			{
			#if DEBUG
//				Console.Out.WriteLine("Opts = {0}", Opts);				
			#endif
				ExecuteConf.Execute();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}