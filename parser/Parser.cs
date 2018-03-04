using System.IO;
using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace CodeGen.parser
{
	public class Parser
	{
		public static string Read(string name)
		{
			if (File.Exists(name))
			{
				return File.ReadAllText(name);
			}
			throw new FileNotFoundException("file does not exit");
		}

		public static void Write(string path, string fileContext)
		{
			File.WriteAllText(path, fileContext);
		}
		
		public static string Download(string url)
		{
			var webClient = new WebClient();
			return webClient.DownloadString(url);
		}

		public static void ValidateArgs(string lang, string file, string url)
		{
			if (lang == "")
			{
				throw new InvalidDataException("specify language (-l) flag");
			}
			if (url == "" && file == "")
			{
				throw new InvalidDataException("specify file path (-f) or url path (-u) flag");
			}
			if (file != "" && url != "")
			{
				// TODO: remove '-u' flag and add method that check if file path is url
				throw new InvalidDataException("do not use both -f and -u flags at the same time");
			}
		}

		public static string GetExtension(string language)
		{
			language = generators.GeneratorConf.NormalizeLang(language);
			if (generators.GeneratorConf.Languanges.ContainsKey(language))
			{
				return "." + generators.GeneratorConf.Languanges[language].Extension;
			}

			throw new KeyNotFoundException("invalid language");
		}

		public static string GetFileFormat(string name)
		{
			var arr = name.Split('.');
			if (arr?.Length > 0)
			{
				return arr.Last();
			}
			throw new InvalidDataException("invalid input file");
		}
	}
}