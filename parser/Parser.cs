using System.IO;
using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace CodeGen.parser
{
	/// <summary>
	/// Parser
	/// </summary>
	public class Parser
	{
		/// <summary>
		/// Reads file by file path
		/// </summary>
		/// <param name="name">Path to file</param>
		/// <returns>File content</returns>
		/// <exception cref="FileNotFoundException">Throws if file does not exist</exception>
		public static string Read(string name)
		{
			if (File.Exists(name))
			{
				return File.ReadAllText(name);
			}
			throw new FileNotFoundException("file does not exit");
		}

		/// <summary>
		/// Writes string to a file
		/// </summary>
		/// <param name="path">Path to new file</param>
		/// <param name="fileContext">Content of a file</param>
		public static void Write(string path, string fileContext)
		{
			File.WriteAllText(path, fileContext);
		}
		
		/// <summary>
		/// Downloads file from the server by url
		/// </summary>
		/// <param name="url">Path to file on the server</param>
		/// <returns>File content</returns>
		public static string Download(string url)
		{
			var webClient = new WebClient();
			return webClient.DownloadString(url);
		}

		/// <summary>
		/// Validates arguments given from command line
		/// </summary>
		/// <param name="lang">Programming language</param>
		/// <param name="file">Path to local file</param>
		/// <param name="url">Path to file on the server</param>
		/// <exception cref="InvalidDataException">Throws if some of arguments are invalid</exception>
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

		/// <summary>
		/// Creates file extension by given language
		/// </summary>
		/// <param name="language">Programming language</param>
		/// <returns>File extension</returns>
		/// <exception cref="KeyNotFoundException">Throws if given language does not exist</exception>
		public static string GetExtension(string language)
		{
			language = generators.GeneratorConf.NormalizeLang(language);
			if (generators.GeneratorConf.Languanges.ContainsKey(language))
			{
				return "." + generators.GeneratorConf.Languanges[language].Extension;
			}

			throw new KeyNotFoundException("invalid language");
		}

		/// <summary>
		/// Gets file format by extension
		/// </summary>
		/// <param name="name">File name</param>
		/// <returns>File extension</returns>
		/// <exception cref="InvalidDataException">Throws if file has no extension</exception>
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