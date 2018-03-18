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
		public const string DefaultLang = "java";

		public static readonly Package DefaultPkg = GeneratorConf.ExamplePkg;

		public static Options Opts;

		private const string FileContent = JavaContent;
		
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
//				var langName = Opts != null ? Opts.Lang : DefaultLang;
//				ExecuteConf.Execute(langName, Opts?.File, Opts != null && Opts.Stdout);
				
				var pkg = new JavaParser().ParsePackage(FileContent);
				var m = GeneratorConf.GetLanguage(Opts.Lang).Generator.Generate(pkg);
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
		
		private const string JavaContent = @"
public class Fruit{}

class Apple extends Fruit {
        public String colour = ""red"";
		public static String sort = ""Golden"";
		private int size = 1;

		private void print(String colour) {}

		protected static int getSizeValue() {
			return new 0;
		}

		public String getColorName() {
			return new String();
		}

		private class Seed{
			public int size;
        
			public static int transform() {
				return 0;
			}
		}
	}
";
		
		private const string GoContent = @"
type Apple struct {
        Colour_1 string
        Sort string
        size int
}

func (Apple) print(colour string, hash int) {}

func (Apple) get_size() int {
        return nil
}

func (Apple) GetColorValue() string {
        return nil
}

type Seed struct {
        Size int
}

func (Seed) Transform() int {
        return nil
}
";
	}
}
