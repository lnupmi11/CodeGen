using System.IO;
using System.Linq;
using System.Net;

namespace CodeGen.utils
{
	/// <summary>
	/// Utils
	/// </summary>
	public static class Utils
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
		/// <exception cref="InvalidDataException">Throws if some of arguments are invalid</exception>
		public static void ValidateArgs(string lang, string file)
		{
			if (lang == "")
			{
				throw new InvalidDataException("specify language (-l) flag");
			}

			if (file == "")
			{
				throw new InvalidDataException("specify file path (-f) flag");
			}
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
			if (!(arr?.Length > 1)) throw new InvalidDataException("invalid input file");
			if (arr.Last().Any(char.IsLetter))
			{
				return arr.Last();
			}

			throw new InvalidDataException("invalid input file");
		}

		/// <summary>
		/// Converts first letter of the string to upper case and others to lower case
		/// </summary>
		/// <param name="string">String to transform</param>
		/// <returns>Transformed string</returns>
		public static string Title(string @string)
		{
			return string.IsNullOrEmpty(@string)
				? @string
				: (
					@string.Length > 1 
						? char.ToUpper(@string[0]) + @string.Substring(1) 
						: @string.ToUpper()
				);
		}
	}
}