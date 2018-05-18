using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using CodeGen.generators;
using CodeGen.utils;
using Newtonsoft.Json;
using YamlDotNet.Core;
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
		public static void Execute(string langName, string fileName, bool toStdout)
		{
			var lang = GeneratorConf.GetLanguage(langName);
			var gen = lang.Generator;
			if (gen == null) throw new NullReferenceException("This language has no generator");

			var pkg = string.IsNullOrWhiteSpace(fileName)
				? Program.DefaultPkg
				: ParseFile(fileName);

			lang.Normalizer?.NormalizePackage(ref pkg);

			var data = gen.Generate(pkg);

			if (toStdout)
			{
				PutDataToStdout(data, lang);
			}
			else
			{
				PutDataToFiles(data, lang);
			}
		}

		private static Package ParseFile(string filename)
		{
			return ParseFileByFormat(GetSerializedData(filename), filename);
		}

		/// <summary>
		/// Parses file by given fromat
		/// </summary>
		/// <param name="body">Document in string representation</param>
		/// <param name="fileName">Name of file</param>
		/// <returns>Parsed package by format</returns>
		/// <exception cref="InvalidDataException"></exception>
		public static Package ParseFileByFormat(string body, string fileName)
		{
			Package pkg;

			var fileFormat = Utils.GetFileFormat(fileName);

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

		private static string GetSerializedData(string fileName)
		{
			var allowedSchemes = new[] {Uri.UriSchemeHttp, Uri.UriSchemeHttps, Uri.UriSchemeFtp, Uri.UriSchemeFtp};

			var result = Uri.TryCreate(fileName, UriKind.Absolute, out var uriResult)
						 && Array.IndexOf(allowedSchemes, uriResult.Scheme) > -1
				? Utils.Download(fileName)
				: Utils.Read(fileName);

			return result;
		}

		private static void PutDataToFiles(Dictionary<string, string> fileContextMap, Languange lang)
		{
			var ext = lang.Extension;
			Directory.CreateDirectory(OutputDir);
			foreach (var file in fileContextMap)
			{
				Utils.Write(OutputDir + "/" + file.Key + '.' + ext, file.Value);
			}

			Console.WriteLine("Generated successfully.");
		}

		private static void PutDataToStdout(Dictionary<string, string> fileContextMap, Languange lang)
		{
			var ext = lang.Extension;
			var comment = lang.Comment;
			var filename = "";
			foreach (var file in fileContextMap)
			{
				if (comment != "")
				{
					filename = string.Format(comment, file.Key + "." + ext);
				}

				Console.WriteLine(filename);
				Console.WriteLine(file.Value);
			}
		}

		/// <summary>
		/// Deserializes Package from Yaml data
		/// </summary>
		/// <param name="body"></param>
		/// <returns></returns>
		public static Package DeserializeYaml(string body)
		{
			try
			{
				var deserializer = new DeserializerBuilder()
					.WithNamingConvention(new CamelCaseNamingConvention())
					.Build();
				var pkg = deserializer.Deserialize<Package>(new StringReader(body));
				return pkg;
			}
			catch (YamlException)
			{
				throw new InvalidDataException("invalid Yaml file content");
			}
		}

		/// <summary>
		/// Deserializes Package from Json data
		/// </summary>
		/// <param name="body">Json data in text format</param>
		/// <returns>parsed Package</returns>
		/// <exception cref="InvalidDataException"></exception>
		public static Package DeserializeJson(string body)
		{
			try
			{
				return JsonConvert.DeserializeObject<Package>(body);
			}
			catch (Exception)
			{
				throw new InvalidDataException("invalid Json file content");
			}
		}

		/// <summary>
		/// Deserialized Package from Xml data
		/// </summary>
		/// <param name="body">Xml data in text format</param>
		/// <returns>Parsed Package</returns>
		/// <exception cref="InvalidDataException"></exception>
		public static Package DeserializeXml(string body)
		{
			try
			{
				var serializer = new XmlSerializer(typeof(Package));
				using (TextReader reader = new StringReader(body))
				{
					return (Package) serializer.Deserialize(reader);
				}
			}
			catch (Exception)
			{
				throw new InvalidDataException("invalid XML file content");
			}
		}
	}
}
