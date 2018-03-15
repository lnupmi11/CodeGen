using System;
using CodeGen.generators;
using CodeGen.parser;
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
		public const string DefaultLang = "go";

		public static readonly Package DefaultPkg = GeneratorConf.ExamplePkg;

		public static Options Opts;


		private const string fileContent = @"
type Apple struct {
        Colour string
        Sort string
        size int
}

func (Apple) print(colour string) {}

func (Apple) getSizeValue() int {
        return nil
}

func (Apple) GetColorName() string {
        return nil
}

type Seed struct {
        Size int
}

func (Seed) Transform() int {
        return nil
}
";
		
		private static void Main(string[] args)
		{
			CommandLine.Parser.Default.ParseArguments<Options>(args)
				.WithParsed(opts => Opts = opts)
				.WithNotParsed(Console.WriteLine);
			try
			{
			#if DEBUG
//				Console.Out.WriteLine("Opts = {0}", Opts);				
			#endif
				
			//	ExecuteConf.Execute();
				var pkg = new GoParser().ParsePackage(fileContent);
				var m = new GoGenerator().Generate(pkg);
				foreach (var file in m)
				{
					Console.WriteLine(file.Value);
				}

			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}