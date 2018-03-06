using System;
using System.Collections.Generic;
using System.IO;
using CodeGen.generators;
using CodeGen.parser;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


namespace CodeGen
{
	/// <summary>
	/// The configuration for execution, contains the application flow
	/// </summary>
	public static class ExecuteConf
	{
		private const string OutputDir = "out";
		
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
		
		private static Package ParseFileByFormat(string body, string fileName)
		{
			Package pkg;

			var fileFormat = Parser.GetFileFormat(fileName);

			switch (fileFormat)
			{
				case "xml":
					pkg = DeserializeXml(body);
					break;
				case "json":
					pkg = DeserializeJson(body);
					break;
				case "yml":
					pkg = DeserializeYaml(body);
					break;
				default:
					throw new InvalidDataException("Invalid format of '" + fileName + "' file.");
			}

			return pkg;
		}
		
		private static string GetSerializedData(string url, string fileName)
		{
			return url != "" ? Parser.Download(url) : Parser.Read(fileName);
		}
		
		private static void PutDataToFiles(Dictionary<string, string> fileContextMap, string lang)
		{
			var ext = Parser.GetExtension(lang);
			Directory.CreateDirectory(OutputDir);
			foreach (var file in fileContextMap)
			{
				Parser.Write(OutputDir + "/" + file.Key + ext, file.Value);
			}
			Console.WriteLine("Generated successfully.");
		}
		
		private static void PutDataToStdout(Dictionary<string, string> fileContextMap, string lang)
		{
			var ext = Parser.GetExtension(lang);
			var comment = GeneratorConf.Languanges[lang].Comment;
			var filename = "";
			foreach (var file in fileContextMap)
			{
				if (comment != "")
				{
					filename = string.Format(comment, OutputDir, file.Key + ext);
				}
				Console.WriteLine(filename);
				Console.WriteLine("\n{0}\n", file.Value);
			}
		}

		private static Package DeserializeYaml(string body)
		{
			var deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
			var pkg = deserializer.Deserialize<Package>(new StringReader(body));
			return pkg;
		}

		private static Package DeserializeJson(string body)
		{
			return new Package();
		}

		private static Package DeserializeXml(string body)
		{
			return new Package();
		}
	}
}
